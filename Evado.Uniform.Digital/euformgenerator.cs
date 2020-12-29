/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\PageGenerator.cs" company="EVADO HOLDING PTY. LTD.">
 *     fieldGroup.ParametersfieldGroup.Parameters
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
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;

// Evado specific references
using Evado.Model;
using Evado.Bll;
using Evado.Bll.Clinical;
using Evado.Model.Digital;

namespace Evado.UniForm.Clinical
{
  //  =================================================================================
  /// <summary>
  ///  This class dynamically generate Evado Forms using class libraries, using (methods)
  /// </summary>
  //  ---------------------------------------------------------------------------------
  public class EuFormGenerator : EuClassAdapterBase
  {
    #region Class initialisation methods.

    //===================================================================================
    /// <summary>
    /// THis is the PageGenerator intialisation method.
    /// </summary>
    /// <param name="ModuleList">String: encoded list of loaded application modules.</param>
    //-----------------------------------------------------------------------------------
    public EuFormGenerator (
      EuApplicationObjects ApplicationObjects,
      EuSession Session,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = "Evado.Model.UniForm.EuFormGenerator.";
      this.ApplicationObjects = ApplicationObjects;
      this.Session = Session;
      this.ClassParameters = Settings;
      this._ModuleList = ApplicationObjects.PlatformSettings.LoadedModuleList;

      this.LoggingLevel = Settings.LoggingLevel;
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Constants and enumerators

    private const string CONST_FORM_FIELD_PREFIX = "FF_";
    private const string CONST_SUBJECT_PREFIX = "S_";
    private const string CONST_STATIC_FIELD_PREFIX = "ST_";
    private const string CONST_FORM_PREFIX = "F_";
    private const string CONST_Subject_Milestone_PREFIX = "V_";
    public const string CONST_DISPLAY_PREFIX = "DISP_";

    public const string CONST_FORM_COMMENT_FIELD_ID = "F_Comments";
    public const string CONST_FORM_DISP_COMMENT_FIELD_ID = "fcm_dsp";
    public const string CONST_FORM_DISP_SOF_FIELD_ID = "sof_dsp";

    private string RecordQueryAnnotation = String.Empty;
    // <summary>
    // This constant defines the eClinical application field layout default setting.
    // </summary>
    public const Evado.Model.UniForm.FieldLayoutCodes ApplicationFieldLayout = Evado.Model.UniForm.FieldLayoutCodes.Left_Justified;


    //
    // Initialise the page labels
    //
    private string _Subject_Demographics_Label = EvLabels.Subject_Demographics_Group_Title;
    private string _Subject_Id_Label = EvLabels.Label_Subject_Id;

    // ***********************************************************************************
    #endregion

    #region Properties and variables..
    //
    // To hide the annotation field during editing.
    //
    private bool _OnEdit_HideFieldAnnotation = false;

    //
    // To hide the signature field.
    //
    private bool _HideSignatureField = false;

    //
    // The form access role
    //
    EdRecordObjectStates _FormState = EdRecordObjectStates.Null;

    //
    // The form access role
    //
    EdRecord.FormAccessRoles _FormAccessRole = EdRecord.FormAccessRoles.Null;

    //
    // Lists the Evado Clinical modules that are enabled.
    //
    private List<EvModuleCodes> _ModuleList = new List<EvModuleCodes> ( );

    // 
    // this list is used to create the field value change comment when a user updates a record's values.
    // 
    private List<EvDataChangeItem> _FieldValueChange = new List<EvDataChangeItem> ( );

    // 
    // Used internally selection field enablement.
    // 
    private List<Evado.Model.Digital.EvFormSection> _Sections = new List<Evado.Model.Digital.EvFormSection> ( );

    // 
    // Used internally selection field enablement.
    // 
    private List<Evado.Model.Digital.EdRecordField> _Fields = new List<Evado.Model.Digital.EdRecordField> ( );

    // 
    // Used internally for validation field enablement for FirstSubject sex
    // 
    private Evado.Model.Digital.EvcStatics.SexOptions _SubjectSex = Evado.Model.Digital.EvcStatics.SexOptions.Null;

    // <summary>
    // The patient date of birth of setting validateion rules.
    // </summary>
    private DateTime _DateOfBirth = Evado.Model.UniForm.EuStatics.CONST_DATE_NULL;

    // <summary>
    // The patient date of consent for the triel setting validateion rules.
    // </summary>
    private DateTime _ConsentDate = Evado.Model.UniForm.EuStatics.CONST_DATE_NULL;

    private String _PageId = String.Empty;


    private bool _FieldDebug = false;
    //  =================================================================================
    // <summary>
    //  Set class field debug
    // </summary>
    //  ---------------------------------------------------------------------------------
    public bool FieldDebug
    {
      set
      {
        this._FieldDebug = value;
      }
    }

    // <summary>
    // The class java debug property setting
    // </summary>
    private bool _JavaDebug = false;

    public bool JavaDebug
    {
      set
      {
        this._JavaDebug = value;
      }
    }


    // ***********************************************************************************
    #endregion

    #region public methods.

    /************************************************************************************
     * 
     * This section contains the classes public methods, for generating the forms at HTML
     * objects.
     * 
     ************************************************************************************/

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates an instance of the form object. Using class library 
    ///   methods.
    /// 
    /// </summary>
    /// <param name="Form">  Evado.Model.Digital.EvForm object</param>
    /// <param name="ClientPage">Evado.Model.UniForm.Page object</param>
    /// <param name="FormDisplayState"> Evado.Model.Digital.EvForm.FormDisplayStates</param>
    /// <param name="MultiSiteUser">boolean multisite</param>
    /// <param name="BinaryFilePath">String: the path to the UniForm binary file store.</param>
    /// <returns>bool:  true = page generated without error.</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. Display the form based on the current view state.
    /// 2. Set the global value field from form field values.
    /// 3. create form record header group.
    /// 4. Create common record group and form field group.
    /// 5. Add javascripts to the form
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public bool generateForm (
       Evado.Model.Digital.EdRecord Form,
      Evado.Model.UniForm.Page ClientPage,
      String BinaryFilePath )
    {
      this.LogMethod ( "generateForm public" );
      this.LogDebug ( "Form.Title: " + Form.Title );
      this.LogDebug ( "Form.State: " + Form.State );
      this.LogDebug ( "Form.Design.OnEdit_HideFieldAnnotation: " + Form.Design.OnEdit_HideFieldAnnotation );
      Form.setFormRole ( this.Session.UserProfile );

      this.LogDebug ( "UserProfile.RoleId: " + this.Session.UserProfile.RoleId );
      this.LogDebug ( "FormAccessRole: " + Form.FormAccessRole );
      // 
      // Set the default pageMenuGroup type to annotated fields.  This will enable the 
      // client annotation functions and initiate the service to including fields for
      // earlier uniform clients.
      // 
      ClientPage.DefaultGroupType = Evado.Model.UniForm.GroupTypes.Annotated_Fields;
      this._OnEdit_HideFieldAnnotation = Form.Design.OnEdit_HideFieldAnnotation;
      this._FormAccessRole = Form.FormAccessRole;
      this._FormState = Form.State;
      this._Fields = Form.Fields;

      //
      // IF the form does not display annotations when being completed
      // hide the annotations by setting hide annotations to true
      //
      if ( Form.Design.TypeId == EvFormRecordTypes.Questionnaire
        || Form.Design.TypeId == EvFormRecordTypes.Informed_Consent
        || Form.Design.TypeId == EvFormRecordTypes.Patient_Record )
      {
        this.LogDebug ( "Questionnaire, Patient Consent or Patient Record so hide annotations. " );
        ClientPage.DefaultGroupType = Evado.Model.UniForm.GroupTypes.Default;
        this._OnEdit_HideFieldAnnotation = true;

        //
        // Hide signature from all non-record editors.
        //
        if ( this._FormAccessRole != EdRecord.FormAccessRoles.Patient
          && this._FormAccessRole != EdRecord.FormAccessRoles.Record_Author )
        {
          this._HideSignatureField = true;
        }
      }

      //this.LogDebug ( "OnEdit_HideFieldAnnotation: " + this._OnEdit_HideFieldAnnotation );
      //this.LogDebug ( "ClientPage.DefaultGroupType: " + ClientPage.DefaultGroupType );
      //this.LogDebug ( "Form.QueryState: " + Form.QueryState );
      //this.LogDebug ( "Form.State: " + Form.State );
      //this.LogDebug ( "HideSignatureField: " + this._HideSignatureField );
      // 
      // Set all groups and field to inherited access
      // 
      switch ( this._FormAccessRole )
      {
        case Evado.Model.Digital.EdRecord.FormAccessRoles.Patient:
        case Evado.Model.Digital.EdRecord.FormAccessRoles.Record_Author:
          {
            //this.LogDebug ( "Record Author "  );
            ClientPage.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

            // 
            // If the hide annotation of the form is not queried and being edited.
            // 
            if ( Form.QueryState != EdRecord.QueryStates.None
              && Form.QueryState != EdRecord.QueryStates.Null )
            {
              this._OnEdit_HideFieldAnnotation = false;
              ClientPage.DefaultGroupType = Evado.Model.UniForm.GroupTypes.Annotated_Fields;
            }

            if ( this._OnEdit_HideFieldAnnotation == true )
            {
              ClientPage.DefaultGroupType = Evado.Model.UniForm.GroupTypes.Default;
            }
            break;
          }
        case Evado.Model.Digital.EdRecord.FormAccessRoles.Monitor:
          {
            //this.LogDebug ( "Monitor" );
            ClientPage.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
            ClientPage.DefaultGroupType = Evado.Model.UniForm.GroupTypes.Review_Fields;
            break;
          }
        case Evado.Model.Digital.EdRecord.FormAccessRoles.Data_Manager:
          {
            //this.LogDebug ( "data manager" );
            ClientPage.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
            ClientPage.DefaultGroupType = Evado.Model.UniForm.GroupTypes.Review_Fields;

            if ( this._FormState == EdRecordObjectStates.Locked_Record )
            {
              ClientPage.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
              ClientPage.DefaultGroupType = Evado.Model.UniForm.GroupTypes.Annotated_Fields;
            }
            break;
          }
        default:
          {
            //this.LogDebug ( "default" );
            ClientPage.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;
            this._OnEdit_HideFieldAnnotation = false;
            ClientPage.DefaultGroupType = Evado.Model.UniForm.GroupTypes.Default;
            break;
          }
      }//END view state switch

      this.LogDebug ( "Final: OnEdit_HideFieldAnnotation: " + this._OnEdit_HideFieldAnnotation );
      this.LogDebug ( "Final: ClientPage.DefaultGroupType: " + ClientPage.DefaultGroupType );
      this.LogDebug ( "Final: HideSignatureField: " + this._HideSignatureField );


      // 
      // Create the form record header groups
      //  
      this.createFormHeader ( Form, ClientPage );

      // 
      // create the form record common field groups
      // 
      this.createCommonFormFields ( Form, ClientPage );

      // 
      // Call the form section create method.
      // 
      this.createFormSections ( Form, ClientPage );

      // 
      // if there is more that one pageMenuGroup create pageMenuGroup category indexes.
      // 
      if ( ClientPage.GroupList.Count > 0 )
      {
        this.getFieldCategories ( Form, ClientPage.GroupList [ 0 ] );
      }

      // 
      // Create the form record fooder groups.
      // 
      this.createFormFooter ( Form, ClientPage );

      // 
      // Add the form specific java scripts
      // 
      this.getFormJavaScript (
        Form.Guid,
        Form.Fields,
        ClientPage,
        BinaryFilePath );

      // 
      // Fill the form 
      //  
      //this.debugGroup ( Form, ClientPage, ViewState );

      return true;

    }//END public generateForm Method.

    // ***********************************************************************************
    #endregion

    #region private form header generation method

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the page header form record page.
    /// </summary>
    /// <param name="Form"> Evado.Model.Digital.EvForm object</param>
    /// <param name="PageObject">Evado.Model.UniForm.Page object</param>
    //  ---------------------------------------------------------------------------------
    private void createFormHeader (
       Evado.Model.Digital.EdRecord Form,
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "createFormHeader" );
      this.LogDebug ( "Form.Design.TypeId: " + Form.Design.TypeId );
      // 
      // Initialise local variables.
      // 
      Evado.Model.UniForm.Group formHeaderGroup;
      Evado.Model.UniForm.Field groupField = new Model.UniForm.Field ( );

      // 
      // Initialise the group if the user is not a patient.
      // 
      if ( this._FormAccessRole != EdRecord.FormAccessRoles.Patient )
      {
        formHeaderGroup = PageObject.AddGroup (
          String.Empty,
          String.Empty,
          Evado.Model.UniForm.EditAccess.Inherited_Access );
        formHeaderGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        formHeaderGroup.GroupType = Model.UniForm.GroupTypes.Default;


        if ( this._FormAccessRole == EdRecord.FormAccessRoles.Monitor
          || this._FormAccessRole == EdRecord.FormAccessRoles.Data_Manager
          || this._FormAccessRole == EdRecord.FormAccessRoles.Record_Reader )
        {
          if ( Form.VisitTitle != String.Empty )
          {
            groupField = formHeaderGroup.createReadOnlyTextField (
              String.Empty,
              EvLabels.Subject_Milestone_Page_Title,
              Form.VisitId
              + EvLabels.Space_Arrow_Right
              + Form.MilestoneId
              + EvLabels.Space_Hypen
              + Form.VisitTitle );
            groupField.Layout = Model.UniForm.FieldLayoutCodes.Center_Justified;
          }

          if ( Form.RecordId == String.Empty )
          {
            Form.RecordId = "RECORD-ID";
          }
          // 
          // if the design reference object exists include the 
          // reference as a html link in the header.
          // 
          if ( Form.Design.Reference != String.Empty )
          {
            groupField = formHeaderGroup.createHtmlLinkField (
              EuFormGenerator.CONST_DISPLAY_PREFIX + EvIdentifiers.FORM_TITLE,
              Form.RecordId
              + EvLabels.Space_Arrow_Right
              + Form.LayoutId
              + EvLabels.Space_Hypen
              + Form.Title,
              Form.Design.Reference );
            groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
          }
          else
          {
            groupField = formHeaderGroup.createTextField (
              String.Empty,
              EvLabels.Label_Form_Id,
              Form.RecordId
              + EvLabels.Space_Arrow_Right
              + Form.LayoutId
              + EvLabels.Space_Hypen
              + Form.Title, 50 );
            groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
            groupField.Layout = Model.UniForm.FieldLayoutCodes.Center_Justified;
          }
        }
        else
        {
          if ( Form.VisitTitle != String.Empty )
          {
            groupField = formHeaderGroup.createReadOnlyTextField (
              String.Empty,
              EvLabels.Subject_Milestone_Page_Title,
              Form.MilestoneId
              + EvLabels.Space_Hypen
              + Form.VisitTitle );
            groupField.Layout = Model.UniForm.FieldLayoutCodes.Center_Justified;
          }
          // 
          // if the design reference object exists include the 
          // reference as a html link in the header.
          // 
          if ( Form.Design.Reference != String.Empty )
          {
            groupField = formHeaderGroup.createHtmlLinkField (
              EuFormGenerator.CONST_DISPLAY_PREFIX + EvIdentifiers.FORM_TITLE,
              Form.LayoutId
              + EvLabels.Space_Hypen
              + Form.Title,
              Form.Design.Reference );
            groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
          }
          else
          {
            groupField = formHeaderGroup.createTextField (
              String.Empty,
              EvLabels.Label_Form_Id,
              Form.RecordId
              + EvLabels.Space_Arrow_Right
              + Form.LayoutId
              + EvLabels.Space_Hypen
              + Form.Title, 50 );
            groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
            groupField.Layout = Model.UniForm.FieldLayoutCodes.Center_Justified;
          }

        }
      }
      // 
      // Add form record instructions if they exist.
      // 
      if ( Form.Design.Instructions != String.Empty )
      {
        // 
        // create the page header pageMenuGroup containing the instructions.
        // 
        formHeaderGroup = PageObject.AddGroup (
          String.Empty,
          String.Empty,
          Evado.Model.UniForm.EditAccess.Inherited_Access );
        formHeaderGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        formHeaderGroup.DescriptionAlignment = Model.UniForm.GroupDescriptionAlignments.Center_Align ;
        formHeaderGroup.Description = Form.Design.Instructions ;
      }

    }//END public fillHeader Method.

    // ***********************************************************************************
    #endregion

    #region private form navigation methods

    //  =============================================================================== 
    /// <summary>
    /// Description:
    ///   This method generates the Java script computed fields.
    /// 
    /// </summary>
    /// <param name="Form">   Evado.Model.Digital.EvForm object containing form ResultData.</param>
    /// <returns>Returns a string containing the Java Scripts.</returns>
    //  ----------------------------------------------------------------------------------
    public Evado.Model.Digital.EvFormSection getSection (
       Evado.Model.Digital.EdRecord Form )
    {
      this.LogMethod ( "hasSection method. "
        + " NextSection: " + Form.NextSection );

      foreach ( Evado.Model.Digital.EvFormSection section in Form.Design.FormSections )
      {
        if ( section.Title.Trim ( ) == Form.NextSection.Trim ( )
          || Form.NextSection.Trim ( ) == ( section.No ).ToString ( ).Trim ( ) )
        {
          this.LogDebug ( "section.No: " + section.No
            + " Form.NextSection: " + Form.NextSection );
          return section;
        }
      }

      return new Evado.Model.Digital.EvFormSection ( );

    }//END getSection Method

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates a form categoristion field lists.
    /// 
    /// </summary>
    /// <param name="Form">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="PageGroup">Evado.Model.UniForm.Group object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. Bulid category list for specific data types.
    /// 2. To each category in the list, add the field corresponding to that category.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private void getFieldCategories (
       Evado.Model.Digital.EdRecord Form,
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "getFieldCategories" );
      // 
      // Initialise local variables.
      //
      string stCategories = String.Empty;
      string stCategoryFields = String.Empty;
      StringBuilder sbHtml = new StringBuilder ( );

      //
      // build the category list by iterating through the list of fields.
      //
      foreach ( Evado.Model.Digital.EdRecordField field in Form.Fields )
      {
        // 
        // Free text, text, date, time, table and matrix fields are not available for categorisation.
        // 
        if ( field.TypeId == Evado.Model.EvDataTypes.Text
          || field.TypeId == Evado.Model.EvDataTypes.Time
          || field.TypeId == Evado.Model.EvDataTypes.Date
          || field.TypeId == Evado.Model.EvDataTypes.Table
          || field.TypeId == Evado.Model.EvDataTypes.Special_Matrix )
        {
          continue;
        }

        //
        // if the category exists add to the list.
        //
        if ( stCategories.Contains ( field.Design.FieldCategory ) == false
          && field.Design.FieldCategory != String.Empty )
        {
          stCategories += field.Design.FieldCategory + ";";
        }
      }
      this.LogDebug ( "Categories: " + stCategories );

      /*
       * create the array of categories.
       */
      string [ ] arCategories = stCategories.Split ( ';' );

      //
      // Iterate through the categories
      //
      for ( int i = 0; i < arCategories.Length; i++ )
      {
        //
        // Initialise loop variables.
        //
        string stCategory = arCategories [ i ].Trim ( );
        stCategoryFields = String.Empty;
        this.LogDebug ( "Category: " + stCategory );

        //
        // If check that the category exists.
        //
        if ( stCategory != String.Empty )
        {
          this.LogDebug ( " > Exists so process it." );
          //
          // iterate through the fields to fine the fields in the category
          //
          foreach ( Evado.Model.Digital.EdRecordField field in Form.Fields )
          {
            // 
            // Free text, text, date, time, table and matrix fields are not available for categorisation.
            // 
            if ( field.TypeId == Evado.Model.EvDataTypes.Text
              || field.TypeId == Evado.Model.EvDataTypes.Time
              || field.TypeId == Evado.Model.EvDataTypes.Date
              || field.TypeId == Evado.Model.EvDataTypes.Table
              || field.TypeId == Evado.Model.EvDataTypes.Special_Matrix )
            {
              continue;
            }

            //
            // if the field is in the category add it to the field.
            //
            if ( stCategory == field.Design.FieldCategory )
            {
              stCategoryFields += field.FieldId + ";";
            }

          }//END field iteration loop

          this.LogDebug ( " > Field List: " + stCategoryFields.Replace ( ";", ", " ) );
          
          // 
          // add a hidden field containing the categories.
          // 
          PageGroup.createHiddenField ( "Category_" + stCategory, stCategoryFields );

        }//END Category exists.

      }//END category iteration loop

      return;

    }//END public getFieldCategories Method.

     //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates a form footer header as html markup.
    /// 
    /// </summary>
    /// <param name="Form">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="ClientDataObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void createFormFooter (
       Evado.Model.Digital.EdRecord Form,
      Evado.Model.UniForm.Page ClientDataObject )
    {
      this.LogMethod ( "createFormFooter" );
      this.LogDebug ( "Form.TypeId: " + Form.TypeId );
      this.LogDebug ( "Form.State: " + Form.State );
      // 
      // Initialise local variables.
      // 
      Evado.Model.UniForm.Field groupField = null;

      //
      // Do not display the form footer for questionnaire forms, when they are being edited.
      //
      if ( Form.FormAccessRole == EdRecord.FormAccessRoles.Patient )
      {
        this.LogDebug ( "EXIT: form footer for patient access and if annotations are hidden during editing." );
        return;
      }

      //
      // if there are comment and hide field annotation then exit.
      //
      if ( Form.CommentList.Count == 0
        && this._OnEdit_HideFieldAnnotation == true )
      {
        this.LogDebug ( "EXIT: Hide annotations and no annotations to display." );
        return;
      }

      // 
      // Display the comments.
      // 
      if ( Form.CommentList.Count > 0
        || this._FormAccessRole == Evado.Model.Digital.EdRecord.FormAccessRoles.Record_Author
        || this._FormAccessRole == Evado.Model.Digital.EdRecord.FormAccessRoles.Monitor
        || this._FormAccessRole == Evado.Model.Digital.EdRecord.FormAccessRoles.Data_Manager )
      {
        this.LogDebug ( "display comments." );
        // 
        // Define the comment pageMenuGroup.
        // 
        Evado.Model.UniForm.Group pageGroup = ClientDataObject.AddGroup (
          String.Empty,
          String.Empty,
          Evado.Model.UniForm.EditAccess.Enabled );
        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        pageGroup.GroupType = Model.UniForm.GroupTypes.Default;

        // 
        // Display the comment list.
        // 
        if ( Form.CommentList.Count > 0 )
        {
          groupField = pageGroup.createReadOnlyTextField (
            EuFormGenerator.CONST_FORM_DISP_COMMENT_FIELD_ID,
            EvLabels.Label_Comments,
             Evado.Model.Digital.EvFormRecordComment.getCommentMD ( Form.CommentList, false ) );

          groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
        }

        // 
        // If in edit mode display a new comment field.
        // 
        if ( this._FormAccessRole == Evado.Model.Digital.EdRecord.FormAccessRoles.Record_Author
          || this._FormAccessRole == Evado.Model.Digital.EdRecord.FormAccessRoles.Monitor
          || this._FormAccessRole == Evado.Model.Digital.EdRecord.FormAccessRoles.Data_Manager )
        {
          this.LogDebug ( "Add Comment Field" );
          groupField = pageGroup.createFreeTextField (
            EuFormGenerator.CONST_FORM_COMMENT_FIELD_ID,
            EvLabels.Label_New_Comment,
            String.Empty,
            50,
            5 );

          groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
          groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

        }//END new comment to be added.

      }//END Display Comments

      // 
      // Enter the Signoff 
      // 
      if ( Form.RecordContent.Signoffs.Count > 0 )
      {
        // 
        // Define the comment pageMenuGroup.
        // 
        Evado.Model.UniForm.Group pageGroup = ClientDataObject.AddGroup (
          String.Empty,
          String.Empty,
          Evado.Model.UniForm.EditAccess.Inherited_Access );
        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        pageGroup.GroupType = Model.UniForm.GroupTypes.Default;

        StringBuilder sbSignoffLog = new StringBuilder ( );

        // 
        // Interate through the signoff objects extracting the signoff content.
        // 
        foreach ( EvUserSignoff signoff in Form.RecordContent.Signoffs )
        {
          // 
          // If the signoff has a description output it.
          // 
          if ( signoff.SignedOffBy != String.Empty )
          {
            sbSignoffLog.AppendLine ( signoff.Description
            + " " + EvLabels.Label_by + " "
            + signoff.SignedOffBy
             + " " + EvLabels.Label_on + " "
            + signoff.stSignOffDate );

          }//END signoff exists.

        }//END interation loop

        groupField = pageGroup.createReadOnlyTextField (
          "sol_dsp",
          EvLabels.Label_Signoff_Log_Field_Title,
          sbSignoffLog.ToString ( ) );

        groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
      }

      // 
      // Initialise pageMenuGroup object.
      // 
      Evado.Model.UniForm.Group footerGroup = ClientDataObject.AddGroup (
        String.Empty,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Enabled );
      footerGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      // 
      // Add the form record approval as a readonly field.
      // 
      groupField = footerGroup.createReadOnlyTextField (
        String.Empty,
        String.Empty,
        Form.Design.Approval );
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Center_Justified;


      return;

    }//END public getFormFooter Method.

    // ***********************************************************************************
    #endregion

    #region private Subject and Common Field methods

     //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method fills the common form static fields for the form as html markup.
    /// 
    /// </summary>
    /// <param name="Form">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="ClientDataObject">  Evado.Model.UniForm.Page object</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void createCommonFormFields (
       Evado.Model.Digital.EdRecord Form,
      Evado.Model.UniForm.Page ClientDataObject )
    {
      this.LogMethod ( "getCommonFormFields" );
      // 
      // Initialise local variables.
      // 
      string stRecordSubjectPrompt = String.Empty;
      string stStartDatePrompt = String.Empty;
      string stFinishDatePrompt = String.Empty;
      Evado.Model.Digital.EvFormSection section = new Evado.Model.Digital.EvFormSection ( );
      Evado.Model.UniForm.Field field = new Evado.Model.UniForm.Field ( );

      // 
      // Only AE SAE, DLT CMM forms have static field headers.
      // 
      if ( Form.Design.TypeId != Evado.Model.Digital.EvFormRecordTypes.Adverse_Event_Report
        && Form.Design.TypeId != Evado.Model.Digital.EvFormRecordTypes.Serious_Adverse_Event_Report
        && Form.Design.TypeId != Evado.Model.Digital.EvFormRecordTypes.Concomitant_Medication )
      {
        return;
      }

      // 
      // Initialise pageMenuGroup object.
      // 
      Evado.Model.UniForm.Group subjectFormFields = ClientDataObject.AddGroup (
        EvLabels.Common_Record_Group_Label,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited_Access );
      subjectFormFields.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      // 
      // The FirstSubject Event FirstSubject.
      // 
      stRecordSubjectPrompt = "Event:";
      stStartDatePrompt = "Onset date:";
      stFinishDatePrompt = "Resolved date:";

      if ( Form.Design.TypeId == Evado.Model.Digital.EvFormRecordTypes.Concomitant_Medication )
      {
        stRecordSubjectPrompt = "Drug Name:";
        stStartDatePrompt = "Commencement date:";
        stFinishDatePrompt = "Completion date:";

      }//END Medication prompts


      // 
      // Create the Record milestone field
      // 
      this.createMultiLineStaticField (
        subjectFormFields,
        EvIdentifiers.RECORD_SUBJECT_FIELD_ID,
        stRecordSubjectPrompt,
         Form.RecordContent.RecordSubject,
        String.Empty,
        Form.RecordSubject,
        true );

      // 
      // Create the start date field
      // 
      this.createStaticField (
        subjectFormFields,
        EvIdentifiers.START_DATE_FIELD_ID,
        stStartDatePrompt,
        Form.RecordContent.StartDate,
        String.Empty,
        Form.stStartDate,
        Evado.Model.EvDataTypes.Date,
        true );

      // 
      // Create the start date field
      // 
      this.createStaticField (
        subjectFormFields,
        EvIdentifiers.FINISH_DATE_FIELD_ID,
        stFinishDatePrompt,
        Form.RecordContent.FinishDate,
        String.Empty,
        Form.stFinishDate,
        Evado.Model.EvDataTypes.Date,
        false );

      //
      // Output the reference field
      //
      if ( Form.AeSelectionList.Count > 1 )
      {
        String optionList = String.Empty;

        foreach ( EvOption option in Form.AeSelectionList )
        {
          optionList = option.Value + " : " + option.Description + ";";
        }
        // 
        // Create the reference field.
        // 

        this.createStaticField (
          subjectFormFields,
          EvIdentifiers.FINISH_DATE_FIELD_ID,
          stFinishDatePrompt,
          Form.RecordContent.FinishDate,
          String.Empty,
          Form.ReferenceId,
          optionList,
          Evado.Model.EvDataTypes.Selection_List,
          false );

      }//END Reference list exists.

      return;

    }//END getCommonFormFields method.

    //  =================================================================================
    /// <summary>
    ///   This method generates a static form field.
    /// </summary>
    /// <param name="FieldGroup">Evado.Model.UniForm.Group object.</param>
    /// <param name="FieldId"> String field identifier .</param>
    /// <param name="FieldTitle">String: field title</param>
    /// <param name="FieldInstructions">String: field instructions</param>
    /// <param name="FieldValue">String field value.</param>
    /// <param name="Field">EvFormStaticField objectss</param>
    /// <param name="FieldType"> Evado.Model.EvDataTypes enumerated value.</param>
    /// <param name="Mandatory">boolean mandatory field.</param>
    /// <param name="ViewState"> Evado.Model.Digital.EvForm.FormDisplayStates defining the form state.</param>
    //  ---------------------------------------------------------------------------------
    private void createStaticField (
      Evado.Model.UniForm.Group FieldGroup,
      String FieldId,
      String FieldTitle,
      EvFormStaticField Field,
      String FieldInstructions,
      String FieldValue,
      Evado.Model.EvDataTypes FieldType,
      bool Mandatory )
    {
      this.LogMethod ( "createStaticField" );
      this.LogDebug ( "FieldId: " + FieldId );
      this.LogDebug ( "FieldTitle: " + FieldTitle );
      this.LogDebug ( "FieldInstructions: " + FieldInstructions );
      this.LogDebug ( "CommentList.Count: " + Field.CommentList.Count );
      this.LogDebug ( "Field.Queried: " + Field.Queried );
      //this.LogDebugValue ( "FieldValue: " + FieldValue );
      this.LogDebug ( "FieldType: " + FieldType );

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.Digital.EdRecordField field = new EdRecordField ( );

      field.State = EdRecordField.FieldStates.Empty;
      field.TypeId = FieldType;
      field.FieldId = FieldId;
      field.Design.Title = FieldTitle;
      field.Design.Instructions = FieldInstructions;
      field.ItemValue = FieldValue;
      field.Design.Mandatory = Mandatory;
      field.CommentList = Field.CommentList;
      field.cDashMetadata = Field.cDashMetadata;
      if ( FieldValue != String.Empty )
      {
        field.State = EdRecordField.FieldStates.With_Value;
      }
      if ( Field.Queried == true )
      {
        field.State = EdRecordField.FieldStates.Queried;
      }
      this.LogDebug ( "field.State: " + field.State );

      if ( field.TypeId == Evado.Model.EvDataTypes.Free_Text )
      {
        field.ItemValue = String.Empty;
        field.ItemText = FieldValue;
      }

      //
      // Generate the form field.
      //
      this.createFormField ( field, FieldGroup, EdRecordObjectStates.Null );

    }//END public createStaticField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates a static form field.
    /// </summary>
    /// <param name="FieldGroup">Evado.Model.UniForm.Group object.</param>
    /// <param name="FieldId"> String field identifier .</param>
    /// <param name="FieldTitle">String: field title</param>
    /// <param name="FieldInstructions">String: field instructions</param>
    /// <param name="FieldValue">String field value.</param>
    /// <param name="Field">EvFormStaticField objectss</param>
    /// <param name="FieldType"> Evado.Model.EvDataTypes enumerated value.</param>
    /// <param name="Mandatory">boolean mandatory field.</param>
    /// <param name="ViewState"> Evado.Model.Digital.EvForm.FormDisplayStates defining the form state.</param>
    //  ---------------------------------------------------------------------------------
    private void createMultiLineStaticField (
      Evado.Model.UniForm.Group FieldGroup,
      String FieldId,
      String FieldTitle,
      EvFormStaticField Field,
      String FieldInstructions,
      String FieldValue,
      bool Mandatory )
    {
      this.LogMethod ( ".createMultiLineStaticField" );
      this.LogDebug ( "FieldId: " + FieldId );
      this.LogDebug ( "FieldTitle: " + FieldTitle );
      this.LogDebug ( "FieldInstructions: " + FieldInstructions );
      //this.LogDebugValue ( "FieldValue: " + FieldValue );

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.Digital.EdRecordField field = new EdRecordField ( );

      field.State = EdRecordField.FieldStates.Empty;
      field.TypeId = EvDataTypes.Text;
      field.FieldId = FieldId;
      field.Design.Title = FieldTitle;
      field.Design.Instructions = FieldInstructions;
      field.ItemValue = FieldValue;
      field.Design.Mandatory = Mandatory;
      field.Design.MultiLineTextField = true;
      field.CommentList = Field.CommentList;
      field.cDashMetadata = Field.cDashMetadata;
      if ( FieldValue != String.Empty )
      {
        field.State = EdRecordField.FieldStates.With_Value;
      }
      if ( Field.Queried == true )
      {
        field.State = EdRecordField.FieldStates.Queried;
      }

      if ( field.TypeId == Evado.Model.EvDataTypes.Free_Text )
      {
        field.ItemValue = String.Empty;
        field.ItemText = FieldValue;
      }

      //
      // Generate the form field.
      //
      this.createFormField ( field, FieldGroup, EdRecordObjectStates.Null );

    }//END public createMultiLineStaticField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates a static form field.
    /// </summary>
    /// <param name="FieldGroup">Evado.Model.UniForm.Group object.</param>
    /// <param name="FieldId"> String field identifier .</param>
    /// <param name="FieldTitle">String: field title</param>
    /// <param name="FieldInstructions">String: field instructions</param>
    /// <param name="FieldValue">String field value.</param>
    /// <param name="Field">EvFormStaticField objectss</param>
    /// <param name="FieldType"> Evado.Model.EvDataTypes enumerated value.</param>
    /// <param name="RangeMinimum">int: minimum range of field</param>
    /// <param name="RangeMaximum">int: maximum range of field.</param>
    /// <param name="Mandatory">boolean mandatory field.</param>
    /// <param name="ViewState"> Evado.Model.Digital.EvForm.FormDisplayStates defining the form state.</param>
    //  ---------------------------------------------------------------------------------
    private void createStaticField (
      Evado.Model.UniForm.Group FieldGroup,
      String FieldId,
      String FieldTitle,
      EvFormStaticField Field,
      String FieldInstructions,
      String FieldValue,
      int RangeMinimum,
      int RangeMaximum,
      Evado.Model.EvDataTypes FieldType,
      bool Mandatory )
    {
      this.LogMethod ( "createStaticField" );
      this.LogDebug ( "FieldId: " + FieldId );
      this.LogDebug ( "FieldTitle: " + FieldTitle );
      this.LogDebug ( "FieldInstructions: " + FieldInstructions );
      this.LogDebug ( "FieldValue: " + FieldValue );
      this.LogDebug ( "RangeMaximum: " + RangeMaximum );
      this.LogDebug ( "RangeMaximum: " + RangeMaximum );
      this.LogDebug ( "FieldType: " + FieldType );

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.Digital.EdRecordField field = new EdRecordField ( );

      field.State = EdRecordField.FieldStates.Empty;
      field.TypeId = FieldType;
      field.FieldId = FieldId;
      field.Design.Title = FieldTitle;
      field.Design.Instructions = FieldInstructions;
      field.ItemValue = FieldValue;
      field.Design.Mandatory = Mandatory;
      field.CommentList = Field.CommentList;
      field.cDashMetadata = Field.cDashMetadata;
      if ( FieldValue != String.Empty )
      {
        field.State = EdRecordField.FieldStates.With_Value;
      }
      if ( Field.Queried == true )
      {
        field.State = EdRecordField.FieldStates.Queried;
      }

      if ( field.TypeId == Evado.Model.EvDataTypes.Free_Text )
      {
        field.ItemValue = String.Empty;
        field.ItemText = FieldValue;
      }
      else
      {
        field.ValidationRules = new EvFormFieldValidationRules ( );
        field.ValidationRules.ValidationLowerLimit = RangeMinimum;
        field.ValidationRules.ValidationUpperLimit = RangeMaximum;
      }

      //
      // Generate the form field.
      //
      this.createFormField ( field, FieldGroup, EdRecordObjectStates.Null );

    }//END public createStaticField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates a static form field.
    /// </summary>
    /// <param name="FieldGroup">Evado.Model.UniForm.Group object.</param>
    /// <param name="FieldId"> String field identifier .</param>
    /// <param name="FieldTitle">String: field title</param>
    /// <param name="Field"> EvFormStaticField Field .</param>
    /// <param name="FieldInstructions">String: field instructions</param>
    /// <param name="FieldValue">String field value.</param>
    /// <param name="FieldOptions">String field options.</param>
    /// <param name="FieldType"> Evado.Model.EvDataTypes enumerated value.</param>
    /// <param name="Mandatory"> boolean mandatory field .</param>
    /// <param name="ViewState"> Evado.Model.Digital.EvForm.FormDisplayStates defining the form state.</param>
    //  ---------------------------------------------------------------------------------
    private void createStaticField (
      Evado.Model.UniForm.Group FieldGroup,
      String FieldId,
      String FieldTitle,
      EvFormStaticField Field,
      String FieldInstructions,
      String FieldValue,
      String FieldOptions,
      Evado.Model.EvDataTypes FieldType,
      bool Mandatory )
    {
      this.LogMethod ( "createStaticField" );
      this.LogDebug ( "FieldId: " + FieldId );
      this.LogDebug ( "FieldTitle: " + FieldTitle );
      this.LogDebug ( "FieldInstructions: " + FieldInstructions );
      this.LogDebug ( "FieldValue: " + FieldValue );
      this.LogDebug ( "FieldOptions: " + FieldOptions );
      this.LogDebug ( "FieldType: " + FieldType );

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.Digital.EdRecordField field = new EdRecordField ( );

      field.State = EdRecordField.FieldStates.Empty;
      field.TypeId = FieldType;
      field.FieldId = FieldId;
      field.Design.Title = FieldTitle;
      field.Design.Instructions = FieldInstructions;
      field.ItemValue = FieldValue;
      field.Design.Mandatory = Mandatory;
      field.CommentList = Field.CommentList;
      field.cDashMetadata = Field.cDashMetadata;
      if ( FieldValue != String.Empty )
      {
        field.State = EdRecordField.FieldStates.With_Value;
      }
      if ( Field.Queried == true )
      {
        field.State = EdRecordField.FieldStates.Queried;
      }
      field.Design.Options = FieldOptions;
      if ( FieldOptions.Contains ( ":" ) == true )
      {
        field.Design.SelectByCodingValue = true;
      }

      //
      // Generate the form field.
      //
      this.createFormField ( field, FieldGroup, EdRecordObjectStates.Null );

    }//END public createStaticField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates a static form field.
    /// </summary>
    /// <param name="FieldGroup">Evado.Model.UniForm.Group object.</param>
    /// <param name="FieldId"> String field identifier .</param>
    /// <param name="FieldTitle">String: field title</param>
    /// <param name="Field"> EvFormStaticField .</param>
    /// <param name="FieldInstructions">String: field instructions</param>
    /// <param name="FieldValue">String field value.</param>
    /// <param name="RangeMinimum">int: minimum range of field</param>
    /// <param name="RangeMaximum">int: maximum range of field.</param>
    /// <param name="Unit">string unit of measurement</param>
    /// <param name="Size">int: field size (width)</param>
    /// <param name="FieldType"> Evado.Model.EvDataTypes enumerated value.</param>
    /// <param name="Mandatory"> boolean: mandatory field.</param>
    /// <param name="ViewState"> Evado.Model.Digital.EvForm.FormDisplayStates defining the form state.</param>
    //  ---------------------------------------------------------------------------------
    private void createStaticField (
      Evado.Model.UniForm.Group FieldGroup,
      String FieldId,
      String FieldTitle,
      EvFormStaticField Field,
      String FieldInstructions,
      String FieldValue,
      int RangeMinimum,
      int RangeMaximum,
      String Unit,
      int Size,
      Evado.Model.EvDataTypes FieldType,
      bool Mandatory )
    {
      this.LogMethod ( "createStaticField" );
      this.LogDebug ( "FieldId: " + FieldId );
      this.LogDebug ( "FieldTitle: " + FieldTitle );
      this.LogDebug ( "FieldInstructions: " + FieldInstructions );
      this.LogDebug ( "FieldValue: " + FieldValue );
      this.LogDebug ( "RangeMinimum: " + RangeMinimum );
      this.LogDebug ( "RangeMaximum: " + RangeMaximum );
      this.LogDebug ( "RangeMaximum: " + RangeMaximum );
      this.LogDebug ( "Unit: " + Unit );
      this.LogDebug ( "Size: " + Size );

      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.Digital.EdRecordField field = new EdRecordField ( );

      field.State = EdRecordField.FieldStates.Empty;
      field.TypeId = FieldType;
      field.FieldId = FieldId;
      field.Design.Title = FieldTitle;
      field.Design.Instructions = FieldInstructions;
      field.Design.Unit = Unit;
      field.ItemValue = FieldValue;
      field.Design.Mandatory = Mandatory;
      field.CommentList = Field.CommentList;
      field.cDashMetadata = Field.cDashMetadata;
      if ( FieldValue != String.Empty )
      {
        field.State = EdRecordField.FieldStates.With_Value;
      }
      if ( Field.Queried == true )
      {
        field.State = EdRecordField.FieldStates.Queried;
      }

      field.ValidationRules = new EvFormFieldValidationRules ( );
      field.ValidationRules.ValidationLowerLimit = RangeMinimum;
      field.ValidationRules.ValidationUpperLimit = RangeMaximum;

      //
      // Generate the form field.
      //
      this.createFormField ( field, FieldGroup, EdRecordObjectStates.Null );

    }//END public createStaticField method.

    // ***********************************************************************************
    #endregion

    #region private Field methods

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the form field objects as html markup.
    /// 
    /// </summary>
    /// <param name="Form">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="PageObject">  Evado.Model.UniForm.Page Object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void createFormSections (
         Evado.Model.Digital.EdRecord Form,
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "createFormSections" );

      // 
      // Initialise local variables.
      // 
      int sectionFieldCount = 0;
      this._PageId = PageObject.PageId;
      Evado.Model.UniForm.Group fieldGroup;

      // 
      // Entering the form section iteration loop.
      // 
      foreach ( Evado.Model.Digital.EvFormSection section in Form.Design.FormSections )
      {
        sectionFieldCount = 0;
        // 
        // Determine how many fields are in this section.
        //
        foreach ( Evado.Model.Digital.EdRecordField field in Form.Fields )
        {
          if ( field.Design.Section == section.No.ToString ( )
            || field.Design.Section == section.Title )
          {
            sectionFieldCount++;
          }
        }

        if ( sectionFieldCount == 0 )
        {
          this.LogDebug ( "No: '" + section.No + "' "
            + " Section: '" + section.Title + "' >> SKIP NOT FIELDS " );
          continue;
        }

        //
        // Skip section if in patient access mode.
        //

        this.LogDebug ( "No: '" + section.No + "' "
          + " Section: '" + section.Title + "' Role: " + section.UserDisplayRoles );

        if ( section.hasRole ( this._FormAccessRole ) == false )
        {
          this.LogDebug ( "No: '" + section.No + "' "
            + " Section: '" + section.Title + "' >> SKIP USER DOES NOT HAVE ACCESS " );
          continue;
        }

        this.LogDebug ( "No: '" + section.No + "' "
          + " Section: '" + section.Title + "' " );

        fieldGroup = PageObject.AddGroup (
          section.Title,
          section.Instructions,
          PageObject.EditAccess );

        fieldGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

        if ( Form.Design.TypeId == EvFormRecordTypes.Informed_Consent )
        {
          this.LogDebug ( "set value width to 20%" );

          fieldGroup.SetValueColumnWidth ( Model.UniForm.FieldValueWidths.Twenty_Percent );
        }

        //
        // If the section has field name then add the pageMenuGroup hide parameters.
        //
        if ( section.FieldId != String.Empty )
        {
          fieldGroup.AddParameter ( Model.UniForm.GroupParameterList.Hide_Group_If_Field_Id,
            section.FieldId );

          fieldGroup.AddParameter ( Model.UniForm.GroupParameterList.Hide_Group_If_Field_Value,
            section.FieldValue );
        }

        this.LogDebug ( "GroupType:  " + fieldGroup.GroupType );

        // 
        // Iterate through each form field in the section.
        // 
        foreach ( Evado.Model.Digital.EdRecordField field in Form.Fields )
        {
          // 
          // If the field is in the section identified by its section name (backward compatibility) or
          // the section number.
          // 
          if ( field.Design.Section.Trim ( ) != section.Title.Trim ( )
            && field.Design.Section.Trim ( ) != section.No.ToString ( ) )
          {
            continue;
          }

          this.createFormField ( field, fieldGroup, Form.State );

        }//END field iteration loop.

        this.LogDebug ( "Value Column Width: " + fieldGroup.GetParameter ( Evado.Model.UniForm.GroupParameterList.Field_Value_Column_Width ) );

      }//END Section interation loop.

      sectionFieldCount = 0;
      // 
      // Determine how many fields are in this section.
      //
      foreach ( Evado.Model.Digital.EdRecordField field in Form.Fields )
      {
        if ( field.Design.Section == String.Empty )
        {
          sectionFieldCount++;
        }
      }

      //
      // IF there are not fields that are not allocated to section exit the section 
      // generation method.
      //
      if ( sectionFieldCount == 0 )
      {
        this.LogDebug ( "EXIT - NO NON-SECTON FIELDS" );
        return;
      }

      //
      // Add an empty section if fields are note allocated to a section
      //
      fieldGroup = PageObject.AddGroup (
        String.Empty,
        PageObject.EditAccess );

      fieldGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      if ( Form.Design.TypeId == EvFormRecordTypes.Informed_Consent )
      {
        this.LogDebug ( "set value width to 20%" );

        fieldGroup.SetValueColumnWidth ( Model.UniForm.FieldValueWidths.Twenty_Percent );
      }

      // 
      // Iterate through each form field in the section.
      // 
      foreach ( Evado.Model.Digital.EdRecordField field in Form.Fields )
      {
        if ( field.Design.Section == String.Empty )
        {
          this.createFormField ( field, fieldGroup, Form.State );
        }

      }//END field iteration loop.

      this.LogDebug ( "Value Column Width: " + fieldGroup.GetParameter ( Evado.Model.UniForm.GroupParameterList.Field_Value_Column_Width ) );


    }//END createFormSections method

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates a form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">EvSubject object containing the FirstSubject and form to be generated.</param>
    /// <param name="FieldGroup">Evado.Model.UniForm.Group object.</param>
    /// <param name="FormState">  Evado.Model.Digital.EvFormObjectStates enumerated value.</param>
    /// <param name="ViewState">  Evado.Model.Digital.EvForm.FormDisplayStates enumerated value.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void createFormField (
       Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Group FieldGroup,
      Evado.Model.Digital.EdRecordObjectStates FormState )
    {
      this.LogMethod ( "getFormField" );
      this.LogDebug ( "FieldId: " + Field.FieldId );
      this.LogDebug ( "TypeId: " + Field.TypeId );
      this.LogDebug ( "Field.State: " + Field.State );

      // 
      // If the not valid sex rules match the FirstSubject's sex then
      // only display the field.
      // 
      if ( Field.ValidationRules.NotValidForFemale == true
        && this._SubjectSex == Evado.Model.Digital.EvcStatics.SexOptions.Female )
      {
        this.LogDebug ( "No valid for females." );
        return;
      }
      if ( Field.ValidationRules.NotValidForMale == true
        && this._SubjectSex == Evado.Model.Digital.EvcStatics.SexOptions.Male )
      {
        this.LogDebug ( "No valid for males." );
        return;
      }

      if ( Field.Design.HideField == true )
      {
        this.LogDebug ( "Hidden field." );
        return;
      }

      // 
      // Initialise the UniFORm field object.
      // 
      Evado.Model.UniForm.Field groupField = FieldGroup.addField (
        Field.FieldId,
        Field.Title,
        Evado.Model.EvDataTypes.Text,
        Field.ItemValue );

      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;

      //
      // IF the field had static readonly field display them across the entire page.
      //
      if ( Field.TypeId == EvDataTypes.Read_Only_Text
        || Field.TypeId == EvDataTypes.External_Image
        || Field.TypeId == EvDataTypes.Streamed_Video
        || Field.TypeId == EvDataTypes.Html_Content )
      {
        groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;
      }

      //
      // If the field is mandatory and is empty then shade the background to identify
      // the field as a mandatory field.
      //
      if ( Field.Design.Mandatory == true )
      {
        groupField.Mandatory = true;
      }

      // 
      // Define the field header.
      // 
      this.createFormFieldHeader ( Field, groupField );

      // 
      // if the field is queried then add the queried status parameter.
      // 
      if ( Field.State == EdRecordField.FieldStates.Queried )
      {
        this.LogDebug ( "Queried field" );
        groupField.AddParameter (
          Evado.Model.UniForm.FieldParameterList.Status,
           Evado.Model.Digital.EdRecordField.FieldStates.Queried.ToString ( ) );

        groupField.setDefaultBackBroundColor ( Model.UniForm.Background_Colours.Orange );
      }

      // 
      // Select the method to generate the correct mobile field type.
      // 
      switch ( Field.TypeId )
      {
        case Evado.Model.EvDataTypes.Computed_Field:
          {
            this.getComputedField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Text:
          {
            this.getTextField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Free_Text:
          {
            this.getFreeTextField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Boolean:
        case Evado.Model.EvDataTypes.Yes_No:
          {
            this.getYesNoField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Special_Query_YesNo:
          {
            this.getQueryYesNoField ( Field, groupField );
            return;
          }

        case Evado.Model.EvDataTypes.Numeric:
        case Evado.Model.EvDataTypes.Integer:
          {
            this.getNumericField ( Field, groupField );
            break;
          }
        case Evado.Model.EvDataTypes.Integer_Range:
          {
            this.getIntegerRangeField ( Field, groupField );
            break;
          }
        case Evado.Model.EvDataTypes.Float_Range:
          {
            this.getFloatRangeField ( Field, groupField );
            break;
          }
        case Evado.Model.EvDataTypes.Date:
          {
            this.getDateField ( Field, groupField );
            break;
          }
        case Evado.Model.EvDataTypes.Time:
          {
            this.getTimeField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Selection_List:
        case Evado.Model.EvDataTypes.External_Selection_List:
          {
            this.getSelectonField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Radio_Button_List:
          {
            this.getRadioButtonListField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Check_Box_List:
          {
            this.getCheckButtonListField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Special_Query_Checkbox:
          {
            this.getQueryCheckButtonListField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Horizontal_Radio_Buttons:
          {
            this.getHorizontalRadioButtonField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Analogue_Scale:
          {
            this.getAnalogueScaleField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Read_Only_Text:
        case Evado.Model.EvDataTypes.Special_Subsitute_Data:
          {
            this.getReadOnlyField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Streamed_Video:
          {
            this.getStreamedVideoField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.External_Image:
          {
            this.getExternalImageField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Html_Link:
          {
            this.getHttpLinkField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Signature:
          {
            this.getSignatureField ( Field, groupField, FormState );
            return;
          }
        case Evado.Model.EvDataTypes.User_Endorsement:
          {
            this.getUserEndorsementField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Special_Quiz_Radio_Buttons:
          {
            this.getQuizRadioButtonField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Special_Medication_Summary:
        case Evado.Model.EvDataTypes.Special_Subject_Demographics:
          {
            this.getTableField ( Field, groupField );
            groupField.Layout = Evado.Model.UniForm.FieldLayoutCodes.Column_Layout;
            groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

            return;
          }
        case Evado.Model.EvDataTypes.Table:
        case Evado.Model.EvDataTypes.Special_Matrix:
          {
            this.getTableField ( Field, groupField );
            return;
          }
        default:
          {
            this.getTextField ( Field, groupField );
            return;
          }

      }//END Field type switch.

    }//END public getFormField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the form field object header as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    /// <param name="ViewState">  Evado.Model.Digital.EvForm.FormDisplayStates enumerated value.</param>
    //  ---------------------------------------------------------------------------------
    private void createFormFieldHeader (
       Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "createFormFieldHeader" );
      this.LogDebug ( "Field.CommentList.Count: " + Field.CommentList.Count );
      // 
      // Add the field header 
      // 
      this.createFieldHeaderText ( Field, GroupField );

      // 
      // If readonly field then don't display the annotation fields.
      // 
      if ( Field.isReadOnly == true )
      {
        return;
      }//END A field without annotation.

      // 
      // Display the annotation in edit, design, or Review_Signoff mode or if annotations exist.
      // 
      switch ( this._FormAccessRole )
      {
        case Evado.Model.Digital.EdRecord.FormAccessRoles.Data_Manager:
          {
            // 
            // Display the Data Cleansing components
            // 
            this.getFieldDataCleansingAnnotationField ( Field, GroupField );

            break;
          }

        case Evado.Model.Digital.EdRecord.FormAccessRoles.Monitor:
          {
            // 
            // Display the Data Cleansing components
            // 
            this.getFieldReviewAnnotationField ( Field, GroupField );

            break;
          }

        case Evado.Model.Digital.EdRecord.FormAccessRoles.Patient:
          {
            break;
          }

        default:
          {
            if ( this._FormState == EdRecordObjectStates.Submitted_Record )
            {
            }

            // 
            // Display the Data Cleansing components
            // 
            this.getFieldEditAnnotationField ( Field, GroupField );

            break;
          }
      }//END display annotation prompt.

      return;

    }//END getFormFieldHeader

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the field header texts if the form display state is in design mode.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    /// <param name="ViewState">  Evado.Model.Digital.EvForm.FormDisplayStates enumerated value.</param>
    //  ---------------------------------------------------------------------------------
    private void createFieldHeaderText (
       Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "createFieldHeaderText" );
      // 
      // Initialise local varibles
      // 
      StringBuilder sbDescription = new StringBuilder ( );

      //
      // set the field to reaonly if the field state is not edit or data cleansing.
      //
      if ( this._FormAccessRole == Evado.Model.Digital.EdRecord.FormAccessRoles.Record_Reader
        || this._FormAccessRole == Evado.Model.Digital.EdRecord.FormAccessRoles.Monitor )
      {
        GroupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      // 
      // design header information
      // 
      if ( this._FormAccessRole == Evado.Model.Digital.EdRecord.FormAccessRoles.Form_Designer )
      {
        sbDescription.Append (
           "Form field identifier: " + Field.FieldId );

        if ( Field.Design.SelectByCodingValue == true )
        {
          sbDescription.Append ( ", SBCV" );
        }
        if ( Field.Design.SummaryField == true )
        {
          sbDescription.Append ( ", SF" );
        }
        if ( Field.Design.Mandatory == true )
        {
          sbDescription.Append ( ", MF" );
        }
        if ( Field.Design.SafetyReport == true )
        {
          sbDescription.Append ( ", SR" );
        }
        if ( Field.Design.AiDataPoint == true )
        {
          sbDescription.Append ( ", DP" );
        }
        if ( Field.Design.HideField == true )
        {
          sbDescription.Append ( ", HF" );
        }
        if ( Field.Design.MultiLineTextField == true )
        {
          sbDescription.Append ( ", MLTF" );
        }
        if ( Field.ValidationRules.IsAfterBirthDate == true
          || Field.ValidationRules.IsAfterConsentDate == true )
        {
          sbDescription.Append ( "\r\n" );
          if ( Field.ValidationRules.IsAfterBirthDate == true )
          {
            sbDescription.Append ( " DV-ADOB" );
          }
          if ( Field.ValidationRules.IsAfterConsentDate == true )
          {
            sbDescription.Append ( " DV-ACD" );
          }
        }

        sbDescription.AppendLine ( "NV-VR: " + Field.ValidationRules.ValidationLowerLimit
           + " - " + Field.ValidationRules.ValidationUpperLimit
           + ", NV-AR: " + Field.ValidationRules.AlertLowerLimit
           + " - " + Field.ValidationRules.AlertUpperLimit );

        sbDescription.AppendLine ( "Field order: " + Field.Order );

        if ( Field.Design.FieldCategory != String.Empty )
        {
          sbDescription.AppendLine ( "Field category: " + Field.Design.FieldCategory );
        }
      }
      else
      {
        if ( Field.Design.HttpReference == String.Empty )
        {
          GroupField.Title = Field.Design.Title;
        }
        else
        {
          GroupField.Title = "<a href='" + Field.Design.HttpReference + "' target='_blank'>"
          + Field.Design.Title + "</a></strong>";
        }
      }

      // 
      // Display manadatory field identifier.
      // 
      if ( Field.Design.Mandatory == true )
      {
        GroupField.Mandatory = true;
      }

      if ( this.DebugOn == true
        && this._FieldDebug == true )
      {
        sbDescription.AppendLine (
           "Type: " + Field.TypeId
           + "\r\nState: " + Field.State
           + "\r\nValidation Error: " + Field.ValidationError );
      }

      if ( Field.Design.Instructions != String.Empty
        && Field.TypeId != Evado.Model.EvDataTypes.Read_Only_Text )
      {
        sbDescription.AppendLine ( Field.Design.Instructions );
      }

      GroupField.Description =  sbDescription.ToString ( ) ;

      return;

    }//END getFormFieldHeader

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the ResultData cleansing annotation fields html.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    /// <param name="ViewState">  Evado.Model.Digital.EvForm.FormDisplayStates enumerated value.</param>
    //  ---------------------------------------------------------------------------------
    private void getFieldDataCleansingAnnotationField (
       Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getFieldDataCheansingAnnotationField" );

      // 
      // Initialise the local variables.
      // 
      System.Text.StringBuilder sbMarkDown = new System.Text.StringBuilder ( );

      //
      // Add the annotation header
      //
      sbMarkDown.AppendLine ( "__" + EvLabels.Label_Field_Comments_Log + "__" );

      // 
      // create the annotation
      // 
      sbMarkDown.AppendLine ( Evado.Model.Digital.EvFormRecordComment.getFieldAnnotationMD ( Field.CommentList ) );

      // 
      // Add the annotation as a parameter.
      // 
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Annotation, sbMarkDown.ToString ( ) );

    }//END getFieldDataCheansingAnnotationField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the review annotation fields html.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    /// <param name="ViewState">  Evado.Model.Digital.EvForm.FormDisplayStates enumerated value.</param>
    //  ---------------------------------------------------------------------------------
    private void getFieldReviewAnnotationField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getFieldReviewAnnotationField" );

      // 
      // Initialise the local variables.
      // 
      System.Text.StringBuilder sbMarkDown = new System.Text.StringBuilder ( );

      //
      // Add the annotation header
      //
      sbMarkDown.AppendLine ( "__" + EvLabels.Label_Field_Comments_Log + "__" );

      // 
      // create the annotation
      // 
      sbMarkDown.AppendLine ( Evado.Model.Digital.EvFormRecordComment.getFieldAnnotationMD ( Field.CommentList ) );

      // 
      // Add the annotation as a parameter.
      // 
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Annotation, sbMarkDown.ToString ( ) );


    }//END getFieldReviewAnnotationField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the review annotation fields html.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    /// <param name="ViewState">  Evado.Model.Digital.EvForm.FormDisplayStates enumerated value.</param>
    //  ---------------------------------------------------------------------------------
    private void getFieldEditAnnotationField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getFieldEditAnnotationField" );
      this.LogDebug ( "OnEdit_HideFieldAnnotation {0}.", this._OnEdit_HideFieldAnnotation );
      this.LogDebug ( "Field.CommentList.Count {0}.", Field.CommentList.Count );
      // 
      // Initialise the local variables.
      // 
      System.Text.StringBuilder sbMarkDown = new System.Text.StringBuilder ( );

      // 
      // Display the Edit components
      // 
      if ( this._OnEdit_HideFieldAnnotation == true
        || Field.CommentList.Count == 0 )
      {
        return;
      }

      //
      // Add the annotation header
      //
      sbMarkDown.AppendLine ( "__" + EvLabels.Label_Field_Comments_Log + "__" );

      // 
      // create the annotation
      // 
      sbMarkDown.AppendLine ( Evado.Model.Digital.EvFormRecordComment.getFieldAnnotationMD ( Field.CommentList ) );

      // 
      // Add the annotation as a parameter.
      // 
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Annotation, sbMarkDown.ToString ( ) );

    }//END getFieldEditAnnotationField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getTextField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getTextField method. MultiLineTextField: "
        + Field.Design.MultiLineTextField );

      // 
      // Initialise local variables.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Text;
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Width, 50 );

      // 
      // If multi-line that use a free text field.
      // 
      if ( Field.Design.MultiLineTextField == true )
      {
        GroupField.Type = Evado.Model.EvDataTypes.Free_Text;
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Width, 50 );
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Height, 2 );
      }

      return;

    }//END getTextField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    /// <param name="FormState">Evado.Model.Digital.EvFormObjectStates enumeration object.</param>
    //  ---------------------------------------------------------------------------------
    private void getSignatureField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField,
      Evado.Model.Digital.EdRecordObjectStates FormState )
    {
      this.LogMethod ( "getSignatureField" );
      this.LogDebug ( "FormState: " + FormState );

      // 
      // Insert a Yes No field if the user is not allowed to see the signature
      // 
      if ( this._HideSignatureField == true )
      {
        this.LogDebug ( "Hidden Signature field." );
        GroupField.Type = Evado.Model.EvDataTypes.Text;
        GroupField.Value = EvLabels.Signature_Field_Status_No_Text;
        GroupField.AddParameter ( Model.UniForm.FieldParameterList.Width, "50" );
        GroupField.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;

        if ( Field.ItemText != String.Empty )
        {
          GroupField.Value = EvLabels.Signature_Field_Status_Yes_Text;
        }

        this.LogDebug ( "GroupField.Type: " + GroupField.Type );
        this.LogDebug ( "GroupField.Value: " + GroupField.Value );

        this.LogMethodEnd ( "getSignatureField" );
        return;
      }//END Hide signature field.

      GroupField.Type = Evado.Model.EvDataTypes.Signature;
      GroupField.Value = Field.ItemText;
      GroupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      GroupField.AddParameter ( Model.UniForm.FieldParameterList.Width, "600" );
      GroupField.AddParameter ( Model.UniForm.FieldParameterList.Height, "200" );
      //
      // the state is not empty or draft and a signature exists it is readonly.
      //
      if ( Field.ItemText != String.Empty
        && FormState != EdRecordObjectStates.Empty_Record
        && FormState != EdRecordObjectStates.Draft_Record
        && FormState != EdRecordObjectStates.Completed_Record)
      {
        GroupField.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;
      }

      this.LogDebug ( "GroupField.Value: " + GroupField.Value );
      this.LogDebug ( "GroupField.EditAccess: " + GroupField.EditAccess );
      this.LogMethodEnd ( "getSignatureField" );

    }//END getTextField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the free text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getFreeTextField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getFreeTextField" );
      // 
      // Set the field parameters
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Free_Text;
      GroupField.Value = Field.ItemText;
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Width, 50 );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Height, 5 );

    }//END getFreeTextField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getReadOnlyField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getReadOnlyField" );

      this.LogDebug ( "Instructions: " + Field.Design.Instructions );
      // 
      // Initialise local variables.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Read_Only_Text;
      GroupField.Value = Field.Design.Instructions;
      GroupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;
      GroupField.Description =String.Empty ;


      //this.LogDebugValue ( "Value: " + GroupField.Value );

      return;

    }//END getReadOnlyField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the quz question field object.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getQuizRadioButtonField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getQuizField" );

      this.LogDebug ( "Instructions: " + Field.Design.Instructions );
      // 
      // Initialise local variables.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Special_Quiz_Radio_Buttons;
      GroupField.Value = Field.ItemValue;
      GroupField.OptionList = Field.Design.OptionList;
      GroupField.Description =  Field.Design.Instructions ;
      GroupField.AddParameter ( Model.UniForm.FieldParameterList.Quiz_Value, Field.Design.QuizValue );
      GroupField.AddParameter ( Model.UniForm.FieldParameterList.Quiz_Answer, Field.Design.QuizAnswer );

      this.LogDebug ( "Value: " + GroupField.Value );

      return;

    }//END getQuizField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the streamed video form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvFormField object containing the field to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getStreamedVideoField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getStreamedVideoField" );

      this.LogDebug ( "URL: Instructions: " + Field.Design.Instructions );
      // 
      // Initialise local variables.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Streamed_Video;
      GroupField.Value = Field.Design.Instructions;
      GroupField.Description =  String.Empty;

      this.LogDebug ( "JavaScript: " + Field.Design.JavaScript );
      int iWidth = 0;
      int iHeight = 0;
      if ( Field.Design.JavaScript != String.Empty )
      {
        String [ ] arParms = Field.Design.JavaScript.Split ( ';' );
        if ( int.TryParse ( arParms [ 0 ], out iWidth ) == false )
        {
          iWidth = 0;
        }
        if ( arParms.Length > 1 )
        {
          if ( int.TryParse ( arParms [ 1 ], out iHeight ) == false )
          {
            iHeight = 0;
          }
        }
      }

      this.LogDebug ( "iWidth: " + iWidth );
      this.LogDebug ( "iHeight: " + iHeight );
      if ( iWidth > 0 )
      {
        GroupField.AddParameter ( Model.UniForm.FieldParameterList.Width, iWidth.ToString ( ) );
      }
      if ( iHeight > 0 )
      {
        GroupField.AddParameter ( Model.UniForm.FieldParameterList.Height, iHeight.ToString ( ) );
      }

      this.LogDebug ( "Value: " + GroupField.Value );

      return;

    }//END getStreamedVideoField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the external image form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getExternalImageField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getExternalImageField" );

      this.LogDebug ( "Instructions: " + Field.Design.Instructions );
      // 
      // Initialise local variables.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.External_Image;
      GroupField.Value = Field.Design.Instructions;
      GroupField.Description = String.Empty ;

      this.LogDebug ( "JavaScript: " + Field.Design.JavaScript );
      int iWidth = 0;
      int iHeight = 0;
      if ( Field.Design.JavaScript != String.Empty )
      {
        String [ ] arParms = Field.Design.JavaScript.Split ( ';' );
        if ( int.TryParse ( arParms [ 0 ], out iWidth ) == false )
        {
          iWidth = 0;
        }
        if ( arParms.Length > 1 )
        {
          if ( int.TryParse ( arParms [ 1 ], out iHeight ) == false )
          {
            iHeight = 0;
          }
        }
      }

      this.LogDebug ( "iWidth: " + iWidth );
      this.LogDebug ( "iHeight: " + iHeight );

      GroupField.AddParameter ( Model.UniForm.FieldParameterList.Width, iWidth.ToString ( ) );
      GroupField.AddParameter ( Model.UniForm.FieldParameterList.Height, iHeight.ToString ( ) );

      this.LogDebug ( "Value: " + GroupField.Value );

      return;

    }//END getExternalImageField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the external image form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getHttpLinkField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getHttpLinkField" );

      this.LogDebug ( "Instructions: " + Field.Design.Instructions );
      this.LogDebug ( "Value: " + GroupField.Value );
      // 
      // Initialise local variables.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Html_Link;
      GroupField.Description = Field.Design.Instructions ;

      return;

    }//END getHttpLinkField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the computed form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getComputedField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( ".getComputedField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Computed_Field;
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Width, 20 );

      return;

    }//END getComputedField method.

    // =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the numeric form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getNumericField (
       Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getNumericField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Numeric;

      GroupField.Value = Field.ItemValue;
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Width, 12 );

      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Min_Value, Field.ValidationRules.ValidationLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Max_Value, Field.ValidationRules.ValidationUpperLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Min_Alert, Field.ValidationRules.AlertLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Max_Alert, Field.ValidationRules.AlertUpperLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Min_Normal, Field.ValidationRules.NormalRangeLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Max_Normal, Field.ValidationRules.NormalRangeUpperLimit.ToString ( ) );
      GroupField.setBackgroundColor ( Evado.Model.UniForm.FieldParameterList.BG_Validation, Model.UniForm.Background_Colours.Red );
      GroupField.setBackgroundColor ( Evado.Model.UniForm.FieldParameterList.BG_Alert, Model.UniForm.Background_Colours.Orange );
      GroupField.setBackgroundColor ( Evado.Model.UniForm.FieldParameterList.BG_Normal, Model.UniForm.Background_Colours.Yellow );

      // 
      // Add the field unit if it exists.
      // 
      if ( Field.Design.UnitHtml != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Unit, Field.Design.UnitHtml );
      }

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }
      if ( Field.TypeId == Evado.Model.EvDataTypes.Integer )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Form.onIntegerValidation" );
      }

    }//END getNumericField method.


    // =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the numeric form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getIntegerRangeField (
       Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getIntegerRangeField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Integer_Range;
      GroupField.Value = Field.ItemValue;
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Width, 12 );

      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Min_Value, Field.ValidationRules.ValidationLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Max_Value, Field.ValidationRules.ValidationUpperLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Min_Alert, Field.ValidationRules.AlertLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Max_Alert, Field.ValidationRules.AlertUpperLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Min_Normal, Field.ValidationRules.NormalRangeLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Max_Normal, Field.ValidationRules.NormalRangeUpperLimit.ToString ( ) );
      GroupField.setBackgroundColor ( Evado.Model.UniForm.FieldParameterList.BG_Validation, Model.UniForm.Background_Colours.Red );
      GroupField.setBackgroundColor ( Evado.Model.UniForm.FieldParameterList.BG_Alert, Model.UniForm.Background_Colours.Orange );
      GroupField.setBackgroundColor ( Evado.Model.UniForm.FieldParameterList.BG_Normal, Model.UniForm.Background_Colours.Yellow );

      // 
      // Add the field unit if it exists.
      // 
      if ( Field.Design.UnitHtml != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Unit, Field.Design.UnitHtml );
      }

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }
      if ( Field.TypeId == Evado.Model.EvDataTypes.Integer )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Form.onIntegerValidation" );
      }

    }//END getIntegerRangeField method.

    // =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the numeric form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getFloatRangeField (
       Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getFloatRangeField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Float_Range;
      GroupField.Value = Field.ItemValue;
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Width, 12 );

      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Min_Value, Field.ValidationRules.ValidationLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Max_Value, Field.ValidationRules.ValidationUpperLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Min_Alert, Field.ValidationRules.AlertLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Max_Alert, Field.ValidationRules.AlertUpperLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Min_Normal, Field.ValidationRules.NormalRangeLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Max_Normal, Field.ValidationRules.NormalRangeUpperLimit.ToString ( ) );
      GroupField.setBackgroundColor ( Evado.Model.UniForm.FieldParameterList.BG_Validation, Model.UniForm.Background_Colours.Red );
      GroupField.setBackgroundColor ( Evado.Model.UniForm.FieldParameterList.BG_Alert, Model.UniForm.Background_Colours.Orange );
      GroupField.setBackgroundColor ( Evado.Model.UniForm.FieldParameterList.BG_Normal, Model.UniForm.Background_Colours.Yellow );

      // 
      // Add the field unit if it exists.
      // 
      if ( Field.Design.UnitHtml != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Unit, Field.Design.UnitHtml );
      }

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }
      if ( Field.TypeId == Evado.Model.EvDataTypes.Integer )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Form.onIntegerValidation" );
      }

    }//END getFloatRangeField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the date form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getDateField ( Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getDateField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Date;
      GroupField.Value = Field.ItemValue;

      if ( Field.ValidationRules.IsAfterBirthDate == true )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Min_Value,
          this._DateOfBirth.ToString ( "dd MMM yyyy" ) );
      }

      if ( Field.ValidationRules.IsAfterConsentDate == true )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Min_Value,
          this._ConsentDate.ToString ( "dd MMM yyyy" ) );
      }

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getDateField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the time form  field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getTimeField ( Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getDateField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Time;
      GroupField.Value = Field.ItemValue;

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getTimeField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the Yes No form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getYesNoField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getYesNoField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Boolean;
      GroupField.Value = Field.ItemValue;
      GroupField.Mandatory = Field.Design.Mandatory;
      GroupField.setBackgroundColor ( Model.UniForm.FieldParameterList.BG_Mandatory, Model.UniForm.Background_Colours.Red );

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getYesNoField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the Yes No form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getQueryYesNoField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getQueryYesNoField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Boolean;
      GroupField.Value = Field.ItemValue;
      GroupField.Mandatory = Field.Design.Mandatory;
      GroupField.setBackgroundColor ( Model.UniForm.FieldParameterList.BG_Mandatory, Model.UniForm.Background_Colours.Red );

      GroupField.SetValueColumnWidth ( Model.UniForm.FieldValueWidths.Twenty_Percent );
      GroupField.AddParameter ( Model.UniForm.FieldParameterList.Field_Value_Legend, EvLabels.FormField_YesNo_Query_Legend );

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getQueryYesNoField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method determines whether the option value is in the validation disable rule list.
    /// 
    /// </summary>
    /// <param name="OptionDisableRules">  Evado.Model.Digital.EvFormFieldValidationNotValid object containing the validation rules.</param>
    /// <param name="OptionList">The option to be searched for.</param>
    /// <returns>True: found, False: not found.</returns>
    //  ---------------------------------------------------------------------------------
    private void TrimOptionList (
      List<Evado.Model.EvOption> OptionList,
        Evado.Model.Digital.EvFormFieldValidationNotValid OptionDisableRules )
    {
      // 
      // If the option rules are null then exit no rules to envoke.
      // 
      if ( OptionDisableRules == null )
      {
        return;
      }

      // 
      // Iterate through options removing the options that are disabled.
      // 
      for ( int optionCount = 0; optionCount < OptionList.Count; optionCount++ )
      {
        Evado.Model.EvOption option = OptionList [ optionCount ];

        this.LogDebug ( "Description: " + option.Description );

        // 
        // If the option exists the slip this option as it is not needed.
        // 
        if ( this.isDisableOption ( OptionDisableRules, option.Description ) == true )
        {
          this.LogDebug ( "Option: " + option.Description + " skipped" );

          OptionList.RemoveAt ( optionCount );
          optionCount--;
        }

        if ( OptionDisableRules.hasRule ( EvIdentifiers.SEX,
             this._SubjectSex.ToString ( ),
             option.Description ) == true )
        {
          this.LogDebug ( "Option: " + option.Description + " skipped" );

          OptionList.RemoveAt ( optionCount );
          optionCount--;
        }


      }//END Field iteration

    }//END TrimOptionList method

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method determines whether the option value is in the validation disable rule list.
    /// 
    /// </summary>
    /// <param name="OptionDisableRules">  Evado.Model.Digital.EvFormFieldValidationNotValid object containing the validation rules.</param>
    /// <param name="Option">The option to be searched for.</param>
    /// <returns>True: found, False: not found.</returns>
    //  ---------------------------------------------------------------------------------
    private bool isDisableOption (
        Evado.Model.Digital.EvFormFieldValidationNotValid OptionDisableRules,
      String Option )
    {
      this.LogMethod ( "isDisableOption method.  Option: " + Option );

      // 
      // If rules are null then exit true
      // 
      if ( OptionDisableRules == null )
      {
        // this._DebugLog.Append( " > Rules Null" );

        return false;
      }

      // 
      // Iteration through the fields looking for a match.
      // 
      foreach ( Evado.Model.Digital.EdRecordField field in this._Fields )
      {
        string fieldName = field.FieldId;

        // 
        // If the values match then return true
        // 
        if ( OptionDisableRules.hasRule ( fieldName, field.ItemValue, Option ) == true )
        {
          // this._DebugLog.Append( " > Disable Option" );

          return true;
        }

      }//END interation loop

      // this._DebugLog.Append( " > Enable Option" );

      return false;

    }//END isDisableOption method

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the radio button list form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getRadioButtonListField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getRadioButtonoField method. Value: " + Field.ItemValue );
      this.LogDebug ( "Options: " + Field.Design.Options );

      if ( Field.ValidationRules.NotValidOptions != null )
      {
        foreach ( string rule in Field.ValidationRules.NotValidOptions.Rules )
        {
          this.LogDebug ( rule );
        }
      }

      // 
      // Initialise the methods object and variables.
      // 
      List<Evado.Model.EvOption> optionlist = Evado.Model.UniForm.EuStatics.getStringAsOptionList (
        Field.Design.Options,
        Field.Design.SelectByCodingValue );

      // 
      // trim thelist of disabled rules.
      // 
      this.TrimOptionList (
        optionlist,
        Field.ValidationRules.NotValidOptions );

      optionlist.Add ( new Evado.Model.EvOption ( "", "Not Selected" ) );

      if ( Field.ItemValue == "Null" )
      {
        Field.ItemValue = String.Empty;
      }

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Radio_Button_List;
      GroupField.Value = Field.ItemValue;
      GroupField.OptionList = optionlist;
      GroupField.Mandatory = Field.Design.Mandatory;
      GroupField.setBackgroundColor ( Model.UniForm.FieldParameterList.BG_Mandatory, Model.UniForm.Background_Colours.Red );

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getRadioButtonListField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the selection list form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField"> Evado.Model.UniForm.Field  object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void getSelectonField ( Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getSelectonField" );
      this.LogDebug ( "Options: " + Field.Design.Options );

      if ( Field.ValidationRules.NotValidOptions != null )
      {
        foreach ( string rule in Field.ValidationRules.NotValidOptions.Rules )
        {
          this.LogDebug ( rule );
        }
      }

      // 
      // Initialise the methods object and variables.
      // 
      List<Evado.Model.EvOption> optionlist = Evado.Model.UniForm.EuStatics.getStringAsOptionList (
        Field.Design.Options,
        Field.Design.SelectByCodingValue );

      // 
      // trim thelist of disabled rules.
      // 
      this.TrimOptionList (
        optionlist,
        Field.ValidationRules.NotValidOptions );

      if ( Field.ItemValue == "Null" )
      {
        Field.ItemValue = String.Empty;
      }

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Selection_List;
      GroupField.Value = Field.ItemValue;
      GroupField.OptionList = optionlist;
      GroupField.Mandatory = Field.Design.Mandatory;
      GroupField.setBackgroundColor ( Model.UniForm.FieldParameterList.BG_Mandatory, Model.UniForm.Background_Colours.Red );

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getSelectonField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the check button list form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField"> Evado.Model.UniForm.Field object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void getCheckButtonListField ( Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getCheckButtonoField" );
      this.LogDebug ( "ItemValue: " + Field.ItemValue );
      this.LogDebug ( "Options: " + Field.Design.Options );

      if ( Field.ValidationRules.NotValidOptions != null )
      {
        foreach ( string rule in Field.ValidationRules.NotValidOptions.Rules )
        {
          //this.writeDebugLogLine ( rule );
        }
      }

      if ( Field.ItemValue == "Null" )
      {
        Field.ItemValue = String.Empty;
      }

      // 
      // Initialise the methods object and variables.
      // 
      List<Evado.Model.EvOption> optionlist = Evado.Model.UniForm.EuStatics.getStringAsOptionList (
        Field.Design.Options,
        false );

      // 
      // trim thelist of disabled rules.
      // 
      this.TrimOptionList (
        optionlist,
        Field.ValidationRules.NotValidOptions );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Check_Box_List;
      GroupField.Value = Field.ItemValue;
      GroupField.OptionList = optionlist;
      GroupField.Mandatory = Field.Design.Mandatory;

      this.LogDebug ( "GroupField.Value: " + GroupField.Value );

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getCheckButtonListField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the check button list form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField"> Evado.Model.UniForm.Field object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void getQueryCheckButtonListField ( Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getQueryCheckButtonListField" );
      this.LogDebug ( "ItemValue: " + Field.ItemValue );
      this.LogDebug ( "Options: " + Field.Design.Options );

      if ( Field.ValidationRules.NotValidOptions != null )
      {
        foreach ( string rule in Field.ValidationRules.NotValidOptions.Rules )
        {
          //this.writeDebugLogLine ( rule );
        }
      }

      if ( Field.ItemValue == "Null" )
      {
        Field.ItemValue = String.Empty;
      }

      // 
      // Initialise the methods object and variables.
      // 
      List<Evado.Model.EvOption> optionlist = Evado.Model.UniForm.EuStatics.getStringAsOptionList (
        Field.Design.Options,
        false );

      // 
      // trim thelist of disabled rules.
      // 
      this.TrimOptionList (
        optionlist,
        Field.ValidationRules.NotValidOptions );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Check_Box_List;
      GroupField.Value = Field.ItemValue;
      GroupField.OptionList = optionlist;
      GroupField.Mandatory = Field.Design.Mandatory;

      GroupField.SetValueColumnWidth ( Model.UniForm.FieldValueWidths.Twenty_Percent );
      GroupField.AddParameter ( Model.UniForm.FieldParameterList.Field_Value_Legend, EvLabels.FormField_Checkbox_Query_Legend );

      this.LogDebug ( "GroupField.Value: " + GroupField.Value );

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getCheckButtonListField method.


    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the horzontal radio buttons form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField"> Evado.Model.UniForm.Field object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void getHorizontalRadioButtonField ( Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getHorizontalRadioButtonField" );

      this.LogDebug ( "Options: " + Field.Design.Options );

      if ( Field.ValidationRules.NotValidOptions != null )
      {
        foreach ( string rule in Field.ValidationRules.NotValidOptions.Rules )
        {
          this.LogDebug ( "" + rule );
        }
      }

      // 
      // Initialise the methods object and variables.
      // 
      List<Evado.Model.EvOption> optionlist = Evado.Model.UniForm.EuStatics.getStringAsOptionList (
        Field.Design.Options,
        Field.Design.SelectByCodingValue );

      // 
      // trim thelist of disabled rules.
      // 
      this.TrimOptionList (
        optionlist,
        Field.ValidationRules.NotValidOptions );


      optionlist.Add ( new Evado.Model.EvOption ( "", "Not Selected" ) );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Horizontal_Radio_Buttons;
      GroupField.Value = Field.ItemValue;
      GroupField.OptionList = optionlist;
      GroupField.Mandatory = Field.Design.Mandatory;
      GroupField.setBackgroundColor ( Model.UniForm.FieldParameterList.BG_Mandatory, Model.UniForm.Background_Colours.Red );

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getHorizontalRadioButtonField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the analogue scale form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">  Evado.Model.UniForm.Fields object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void getAnalogueScaleField ( Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getAnalogueScale" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Analogue_Scale;
      GroupField.Value = Field.ItemValue;

      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Min_Label,
       Field.Design.AnalogueLegendStart );

      GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Max_Label,
       Field.Design.AnalogueLegendFinish );

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getAnalogueScale method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getUserEndorsementField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getPasswordField" );

      // 
      // Initialise local variables.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Read_Only_Text;
      GroupField.Value = Field.ItemValue;

      return;

    }//END getTextField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the table form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Model.Digital.EvForm object containing the form to be generated.</param>
    /// <param name="ViewState">  Evado.Model.Digital.EvForm.FormDisplayStates enumerated value.</param>
    /// <param name="GroupField">Evado.Model.UniForm.Field object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void getTableField (
        Evado.Model.Digital.EdRecordField Field,
      Evado.Model.UniForm.Field GroupField )
    {
      this.LogMethod ( "getFormFieldTable" );

      if ( Field.Table == null )
      {
        this.LogDebug ( "FieldId: " + Field.FieldId + ": Table object is null." );

        return;
      }

      this.LogDebug ( "Field table row count: " + Field.Table.Rows.Count );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Table = new Evado.Model.UniForm.Table ( );
      GroupField.Type = Evado.Model.EvDataTypes.Table;
      GroupField.EditAccess = Evado.Model.UniForm.EditAccess.Inherited_Access;
      GroupField.Layout = Evado.Model.UniForm.FieldLayoutCodes.Column_Layout;

      // 
      // Initialise the field object.
      // 
      if ( Field.Design.htmInstructions != String.Empty )
      {
        GroupField.Description =  Field.Design.htmInstructions ;
      }

      // 
      // Initialise the table header
      // 
      for ( int column = 0; column < Field.Table.Header.Length; column++ )
      {
        String columNo = ( column + 1 ).ToString ( );

        GroupField.Table.Header [ column ].No = Field.Table.Header [ column ].No;
        GroupField.Table.Header [ column ].Text = Field.Table.Header [ column ].Text;
        GroupField.Table.Header [ column ].TypeId = Field.Table.Header [ column ].TypeId;
        GroupField.Table.Header [ column ].Width = Field.Table.Header [ column ].Width;

        // 
        // Proces the Options or Unit field value.
        // 
        if ( Field.Table.Header [ column ].TypeId == Evado.Model.Digital.EdRecordTableHeader.ItemTypeMatrix )
        {
          GroupField.Table.Header [ column ].TypeId = Evado.Model.UniForm.TableColHeader.ItemTypeReadOnly;
        }

        if ( GroupField.Table.Header [ column ].TypeId == Evado.Model.UniForm.TableColHeader.ItemTypeNumeric )
        {
          GroupField.Table.Header [ column ].OptionsOrUnit = Field.Table.Header [ column ].OptionsOrUnit;
        }

        if ( GroupField.Table.Header [ column ].TypeId == Evado.Model.UniForm.TableColHeader.ItemTypeRadioButton
          || GroupField.Table.Header [ column ].TypeId == Evado.Model.UniForm.TableColHeader.ItemTypeSelectionList )
        {
          GroupField.Table.Header [ column ].OptionList = Evado.Model.UniForm.EuStatics.getStringAsOptionList (
            Field.Table.Header [ column ].OptionsOrUnit, false );
        }

      }//END Column interation loop

      // 
      // Iterate through the table rows processing the cells in in row.
      // 
      for ( int inRow = 0; inRow < Field.Table.Rows.Count; inRow++ )
      {
        // 
        // Initialise iteration variables and objects.
        // 
        Evado.Model.UniForm.TableRow row = new Evado.Model.UniForm.TableRow ( );
        Evado.Model.Digital.EdRecordTableRow evRow = Field.Table.Rows [ inRow ];

        // 
        // iterate through the table columns processing the table cells.
        // 
        for ( int inColumn = 0; inColumn < Field.Table.ColumnCount; inColumn++ )
        {

          this.LogDebug ( "Col: " + inColumn + " > " + evRow.Column [ inColumn ] );

          row.Column [ inColumn ] = evRow.Column [ inColumn ];

          // 
          // check for empty field values.
          // 
          if ( row.Column [ inColumn ] == String.Empty )
          {
            this.LogDebug ( " >> Data is empty \r\n" );
          }
          this.LogDebug ( " >> '" + row.Column [ inColumn ] + "'\r\n" );

        }//END table columns
        this.LogDebug ( "Appended table row." );
        // 
        // Append the row to the table.
        // 
        GroupField.Table.Rows.Add ( row );

      }//END table row

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getTableField method.

    // ***********************************************************************************
    #endregion

    #region Update Subject object with text values.

   // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Update form object with form values.

    //  =============================================================================== 
    /// <summary>
    /// Description:
    ///   This method updates the test object with field ResultData.
    /// 
    /// </summary>
    /// <param name="CommandParameters">The a list of the returned html form field value</param>
    /// <param name="Form">The form object to be updated.</param>
    /// <param name="User">the user profile.</param>
    //  ----------------------------------------------------------------------------------
    public void updateFormObject (
      List<Evado.Model.UniForm.Parameter> CommandParameters,
         Evado.Model.Digital.EdRecord Form )
    {
      this.LogMethod ( "updateFormObject" );
      this.LogDebug ( "Returned Field count = " + CommandParameters.Count );
      this.LogDebug ( "FormId: " + Form.LayoutId );
      this.LogDebug ( "RecordId: " + Form.RecordId );
      this.LogDebug ( "Form State: " + Form.State );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Model.Digital.EvFormStaticField staticField = new Evado.Model.Digital.EvFormStaticField ( );

      //
      // Set the form role for this user.
      //
      Form.setFormRole ( this.Session.UserProfile );
      this._FormAccessRole = Form.FormAccessRole;
      this.LogDebug ( "FormAccessRole: " + Form.FormAccessRole );

      this._Sections = Form.Design.FormSections;
      // 
      // If the state is editable then update the TestReport.
      // 
      if ( this._FormAccessRole != EdRecord.FormAccessRoles.Form_Designer
        && this._FormAccessRole != EdRecord.FormAccessRoles.Record_Reader )
      {
        this.LogDebug ( "Updating field values." );
        // 
        // Update the common TestReport static test fields.
        // 
        this.updateCommonRecordFields ( CommandParameters, Form );

        // 
        // Iterate through the test fields updating the fields that have changed.
        // 
        for ( int count = 0; count < Form.Fields.Count; count++ )
        {
          if ( Form.Fields [ count ].Design.HideField == true )
          {
            //this.LogDebugValue ( "Field: " + Form.Fields [ count ].FieldId + " a hidden value." );
            continue;
          }

          // 
          // Retrieve the form field value and update the field object if the value has changed.
          // 
          this.updateFormField (
            CommandParameters,
            Form.Fields [ count ],
            Form.State );

          //
          // If the record is a patient record and a form field had the Field ID of
          // RecordSubject then update the record subject value with the its vale.
          // This will produce a summary value that can be displayed to users.
          //
          if ( Form.TypeId == EvFormRecordTypes.Patient_Record 
            && Form.Fields[ count].FieldId == EdRecord.FormClassFieldNames.RecordSubject.ToString() )
          {
            Form.RecordSubject = Form.Fields [ count ].ItemValue;
          }

          //this.LogDebugValue ( "Field: " + Form.Fields [ count ].FieldId + " value: " + Form.Fields [ count ].ItemValue );

        }//END test field iteration.

        // 
        // Update the test comments
        // 
        this.updateFormComments (
        CommandParameters,
        Form );

      }//END Record is editable 

      // 
      // If a standard test has been queried then set the test object to queried.
      // 
      if ( Form.hasQueredItems == true )
      {
        Form.IsQueried = true;

        // 
        // Add the static field query annotation.
        // 
        if ( this.RecordQueryAnnotation != String.Empty )
        {
          EvFormRecordComment.AuthorTypeCodes authorCode = EvFormRecordComment.AuthorTypeCodes.Reviewer;

          if ( this._FormAccessRole == EdRecord.FormAccessRoles.Data_Manager )
          {
            authorCode = EvFormRecordComment.AuthorTypeCodes.Data_Manager;
          }

          this.RecordQueryAnnotation = "Queried static fields are:" + this.RecordQueryAnnotation;

          EvFormRecordComment comment = new EvFormRecordComment (
            Form.Guid,
             authorCode,
             this.Session.UserProfile.UserId,
             this.Session.UserProfile.CommonName,
             RecordQueryAnnotation );

          Form.CommentList.Add ( comment );

        }//END test static fields queried.

      }
      this.LogDebug ( "Record queried status: " + Form.IsQueried );
      this.LogDebug ( "hasQueredItems: " + Form.hasQueredItems );

    }//END updateFormObject method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the groupCommand's parameter list..
    /// </summary>
    /// <param name="ParameterList">List of MethodParameter objects.</param>
    /// <param name="ParameterName">String: name of the parameter to be retrieved.</param>
    //  ---------------------------------------------------------------------------------
    private String GetParameterValue (
      List<Evado.Model.UniForm.Parameter> ParameterList,
      String ParameterName )
    {
      // 
      // Search the parmeters for existing parameters.
      // and exit if update the value.
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in ParameterList )
      {
        if ( parameter.Name.ToLower ( ) == ParameterName.ToLower ( ) )
        {
          return parameter.Value;
        }
      }

      return null;

    }//END AddParameter method

    //  =============================================================================== 
    /// <summary>
    /// Description:
    ///   This method updates the common Record fields with the parameter list.
    /// 
    /// </summary>
    /// <param name="Form">The form field object to be updated.</param>
    /// <param name="CommandParameters">The list of html form field values</param>
    /// <param name="User">The user's profile</param>
    //  ----------------------------------------------------------------------------------
    private void updateCommonRecordFields (
      List<Evado.Model.UniForm.Parameter> CommandParameters,
         Evado.Model.Digital.EdRecord Form )
    {
      this.LogMethod ( "updateCommonRecordFields" );
      this.LogDebug ( " RecordId: " + Form.RecordId );
      this.LogDebug ( "Form State: " + Form.State );
      this.LogDebug ( "Form Design.TypeId: " + Form.Design.TypeId );
      // 
      // Initialise the methods variables and objects.
      // 
      string commentText = String.Empty;
      string stValue = String.Empty;
      Evado.Model.Digital.EvFormStaticField staticField = new Evado.Model.Digital.EvFormStaticField ( );

      // 
      // Get the static test comment field value.
      // 
      if ( Form.Design.TypeId != Evado.Model.Digital.EvFormRecordTypes.Adverse_Event_Report
        && Form.Design.TypeId != Evado.Model.Digital.EvFormRecordTypes.Concomitant_Medication
        && Form.Design.TypeId != Evado.Model.Digital.EvFormRecordTypes.Serious_Adverse_Event_Report
        && Form.Design.TypeId != Evado.Model.Digital.EvFormRecordTypes.Periodic_Followup )
      {
        return;
      }//END Processing Common FormRecord fields.


      if ( this._FormAccessRole == EdRecord.FormAccessRoles.Record_Author
        || this._FormAccessRole == EdRecord.FormAccessRoles.Data_Manager )
      {
        //  
        // Update FormRecord Therapy.
        // 
        Form.RecordSubject = this.getStaticFieldValue (
          CommandParameters,
          Form.RecordContent.RecordSubject,
          EvIdentifiers.RECORD_SUBJECT_FIELD_ID,
          Form.RecordSubject );

        // 
        // Update Start Date
        // 
        Form.stStartDate = this.getStaticFieldValue (
          CommandParameters,
          Form.RecordContent.StartDate,
          EvIdentifiers.START_DATE_FIELD_ID,
          Form.stStartDate );

        // 
        // Update Finish Date
        // 
        Form.stFinishDate = this.getStaticFieldValue (
          CommandParameters,
          Form.RecordContent.FinishDate,
          EvIdentifiers.FINISH_DATE_FIELD_ID,
          Form.stFinishDate );

        // 
        // Update Reference FormRecord
        // 
        Form.ReferenceId = this.getStaticFieldValue (
          CommandParameters,
          Form.RecordContent.ReferenceId,
         EvIdentifiers.REFERENCE_ID_FIELD,
          Form.ReferenceId );
      }

      // 
      // Add the annotation.
      // 
      if ( this._FormAccessRole == EdRecord.FormAccessRoles.Record_Author
        || this._FormAccessRole == EdRecord.FormAccessRoles.Monitor
        || this._FormAccessRole == EdRecord.FormAccessRoles.Data_Manager )
      {
        //  
        // Update FormRecord Therapy.
        //  
        this.updateStaticFieldAnnotation (
          CommandParameters,
          EvIdentifiers.RECORD_SUBJECT_FIELD_ID,
          Form.RecordContent.RecordSubject,
          Form.State );

        // 
        // Update Start Date
        // 
        this.updateStaticFieldAnnotation (
          CommandParameters,
          EvIdentifiers.START_DATE_FIELD_ID,
          Form.RecordContent.StartDate,
          Form.State );

        // 
        // Update Finish Date
        // 
        this.updateStaticFieldAnnotation (
          CommandParameters,
          EvIdentifiers.FINISH_DATE_FIELD_ID,
          Form.RecordContent.FinishDate,
          Form.State );

        // 
        // Update Reference FormRecord
        // 
        this.updateStaticFieldAnnotation (
          CommandParameters,
          EvIdentifiers.REFERENCE_ID_FIELD,
          Form.RecordContent.ReferenceId,
          Form.State );
      }


    }//END updateCommonRecordFields method.

    //  =============================================================================== 
    /// <summary>
    /// Description:
    ///   This method gets the value in the static field from teh list of parameters.
    /// 
    /// </summary>
    /// <param name="CommandParameters">List<Evado.Model.UniForm.Parameter>: list of parameters passed</param>
    /// <param name="Field">The list of html form field values</param>
    /// <param name="FieldId">The html form field to be updated.</param>
    /// <param name="CurrentValue">The curren form field value.</param>
    /// <returns>Return a string with the form field value.</returns>
    //  ----------------------------------------------------------------------------------
    private string getStaticFieldValue (
      List<Evado.Model.UniForm.Parameter> CommandParameters,
        Evado.Model.Digital.EvFormStaticField Field,
      string FieldId,
      string CurrentValue )
    {
      this.LogMethod ( "getStaticFieldValue" );
      this.LogDebug ( "FieldId: " + FieldId );
      this.LogDebug ( "CurrentValue: '" + CurrentValue + "'" );
      // 
      // Initialise methods variables and objects.
      // 
      string stValue = CurrentValue;
      string stThisValue = String.Empty;

      // 
      // Iterate through the option list to compare values.
      // 
      stThisValue = this.GetParameterValue ( CommandParameters, FieldId );

      // 
      // Does the returned field value exist
      // 
      if ( stThisValue == null )
      {
        this.LogDebug ( "Null returned value. CurrentValue: '" + CurrentValue + "'" );

        return CurrentValue;
      }

      // 
      // Reset the escape values to periods
      // 
      stThisValue = stThisValue.Replace ( "\r", " " );
      stThisValue = stThisValue.Replace ( "\n", " " );
      stThisValue = stThisValue.Replace ( "\t", " " );
      stThisValue = stThisValue.Replace ( "  ", " " );
      stThisValue = stThisValue.Replace ( "  ", " " );

      if ( stThisValue != CurrentValue )
      {
        this.LogDebug ( "static Field Change: FieldId: '" + Field.FieldTitle
         + "' Old: '" + CurrentValue + "' New: '" + stThisValue + "' " );

        // 
        // If the field has a field title then add a ResultData change item
        // 
        if ( Field.FieldTitle != String.Empty )
        {
          // 
          // add the value change object to generate the record change comment.
          // 
          this._FieldValueChange.Add ( new EvDataChangeItem (
            Field.FieldTitle,
            CurrentValue,
            stThisValue ) );

        }

        stValue = stThisValue.Trim ( );

        // 
        // reset the queied field value.
        // 
        if ( Field.Queried == true )
        {
          Field.Queried = false;
        }
      }

      this.LogDebug ( "stValue: '" + stValue + "'" );

      return stValue;

    }//END getStaticFieldValue method

    //  =============================================================================== 
    /// <summary>
    /// Description:
    ///   This method get the pateient data form the list of parameters.
    /// 
    /// </summary>
    /// <param name="CommandParameters">The list of html form field values</param>
    /// <param name="FieldId">The html form field to be updated.</param>
    /// <param name="CurrentValue">The curren form field value.</param>
    /// <returns>Return a string with the form field value.</returns>
    //  ----------------------------------------------------------------------------------
    private string getPatientFieldValue (
      List<Evado.Model.UniForm.Parameter> CommandParameters,
      string FieldId,
      string CurrentValue )
    {
      this.LogMethod ( "getStaticFieldValue" );
      this.LogDebug ( "FieldId: " + FieldId );
      this.LogDebug ( "CurrentValue: '" + CurrentValue + "'" );
      // 
      // Initialise methods variables and objects.
      // 
      string stValue = CurrentValue;
      string stThisValue = String.Empty;

      // 
      // Iterate through the option list to compare values.
      // 
      stThisValue = this.GetParameterValue ( CommandParameters, FieldId );

      // 
      // Does the returned field value exist
      // 
      if ( stThisValue == null )
      {
        this.LogDebug ( "Null returned value. CurrentValue: '" + CurrentValue + "'" );

        return CurrentValue;
      }

      // 
      // Reset the escape values to periods
      // 
      stThisValue = stThisValue.Replace ( "\r", " " );
      stThisValue = stThisValue.Replace ( "\n", " " );
      stThisValue = stThisValue.Replace ( "\t", " " );
      stThisValue = stThisValue.Replace ( "  ", " " );
      stThisValue = stThisValue.Replace ( "  ", " " );

      if ( stValue != stThisValue )
      {
        return stThisValue;
      }

      return stValue;

    }//END getStaticFieldValue method

    //  =============================================================================== 
    /// <summary>
    /// Description:
    ///   This method updates the queries field 
    /// 
    /// </summary>
    /// <param name="CommandParameters">List if returned html form field values</param>
    /// <param name="FieldId">The html form field id to be updated</param>
    /// <param name="Field">The form field object to be updated.</param>
    /// <param name="FormState">The form state</param>
    /// <param name="DisplayState">The form display state</param>
    /// <param name="User">The user's profile</param>
    /// <returns>Returns true for queried, false for not queried.</returns>
    //  ----------------------------------------------------------------------------------
    private void updateStaticFieldAnnotation (
      List<Evado.Model.UniForm.Parameter> CommandParameters,
      string FieldId,
        Evado.Model.Digital.EvFormStaticField Field,
        Evado.Model.Digital.EdRecordObjectStates FormState )
    {
      this.LogMethod ( "updateStaticFieldAnnotation" );
      this.LogDebug ( "FieldId: " + FieldId );
      // 
      // Initialise methods variables and objects.
      // 

      // 
      // Process the test field for test edit mode.
      // 
      if ( this._FormAccessRole == EdRecord.FormAccessRoles.Record_Author )
      {
        // 
        // Update comments fields for an queried field
        // 
        if ( FormState == Evado.Model.Digital.EdRecordObjectStates.Queried_Record )
        {
          Field.Queried = false;
        }

      }//END test record field state 'Edit'

      // 
      // Process the test field for test edit mode.
      // 
      if ( this._FormAccessRole == EdRecord.FormAccessRoles.Data_Manager
        || this._FormAccessRole == EdRecord.FormAccessRoles.Monitor )
      {
        // 
        // Get the query check box value.
        // 
        string stQuery = this.GetParameterValue (
          CommandParameters,
          FieldId + Evado.Model.UniForm.Field.CONST_FIELD_QUERY_SUFFIX );

        this.LogDebug ( "Review  >> stQuery: " + stQuery );

        // 
        // If the value exists then process the query
        // 
        if ( stQuery != null )
        {
          if ( stQuery.ToLower ( ) == "on"
            || stQuery.ToLower ( ) == "true"
            || stQuery.ToLower ( ) == "checked"
            || stQuery.ToLower ( ) == "yes" )
          {
            Field.Queried = true;
          }//END query checked

        }//END END query exits.
        else
        {
          Field.Queried = false;
        }

      }//END test form field state 'Review_Signoff'

      // 
      // Get form comment value.
      // 
      string stAnnotation = this.GetParameterValue (
        CommandParameters,
        FieldId + Evado.Model.UniForm.Field.CONST_FIELD_ANNOTATION_SUFFIX );

      this.LogDebug ( "stAnnotation: " + stAnnotation );

      // 
      // If the annotation exists then add it to the field annotation list.
      // 
      if ( stAnnotation != null )
      {
        if ( stAnnotation != String.Empty )
        {
          Evado.Model.Digital.EvFormRecordComment comment = new Evado.Model.Digital.EvFormRecordComment ( );
          comment.UserId = this.Session.UserProfile.UserId;
          comment.UserCommonName = this.Session.UserProfile.CommonName;
          comment.CommentDate = DateTime.Now;
          comment.NewComment = true;
          comment.CommentType = Evado.Model.Digital.EvFormRecordComment.CommentTypeCodes.Form_Field;

          // 
          // Set the annotation author type.
          // 
          comment.AuthorType = Evado.Model.Digital.EvFormRecordComment.AuthorTypeCodes.Record_Author;

          if ( this._FormAccessRole == EdRecord.FormAccessRoles.Monitor
             && Field.Queried == true )
          {
            comment.AuthorType = Evado.Model.Digital.EvFormRecordComment.AuthorTypeCodes.Monitor;
            comment.Content = "Value Queried: " + stAnnotation;
          }
          if ( this._FormAccessRole == EdRecord.FormAccessRoles.Data_Manager
             && Field.Queried == true )
          {
            comment.AuthorType = Evado.Model.Digital.EvFormRecordComment.AuthorTypeCodes.Data_Manager;
            comment.Content = "Value Queried: " + stAnnotation;
          }

          if ( FormState == Evado.Model.Digital.EdRecordObjectStates.Queried_Record )
          {
            comment.Content = "Value Updated: " + stAnnotation;
          }
          else
          {
            comment.Content = stAnnotation;
          }

          // 
          // add the comment to the field list.
          // 
          Field.CommentList.Add ( comment );

        }//END stAnnotation exists.

      }//END stAnnotation exists.

      return;

    }//END getStaticFieldAnnotation method

    //  =============================================================================== 
    /// <summary>
    /// Description:
    ///   This method updates the test annotation field.
    /// 
    /// </summary>
    /// <param name="Form">   Evado.Model.Digital.EvForm object containing test ResultData.</param>
    /// <param name="CommandParameters">The returned test field values.</param>
    /// <param name="User">The users profile.</param>
    /// <returns>Returns a string containing the Java Scripts.</returns>
    //  ----------------------------------------------------------------------------------
    private void updateFormComments (
      List<Evado.Model.UniForm.Parameter> CommandParameters,
         Evado.Model.Digital.EdRecord Form )
    {
      this.LogMethod ( "updateFormComments" );
      this.LogDebug ( "RecordId: " + Form.RecordId );
      this.LogDebug ( "Form State: " + Form.State );
      this.LogDebug ( "Changed Field Count: " + this._FieldValueChange.Count );
      // 
      // Initialise the methods variables and objects.
      // 
      string commentText = String.Empty;
      string stValue = String.Empty;
      Evado.Model.Digital.EvFormRecordComment.AuthorTypeCodes authorType = Evado.Model.Digital.EvFormRecordComment.AuthorTypeCodes.Record_Author;

      if ( this._FormAccessRole == EdRecord.FormAccessRoles.Data_Manager )
      {
        authorType = Evado.Model.Digital.EvFormRecordComment.AuthorTypeCodes.Data_Manager;
      }
      else if ( this._FormAccessRole == EdRecord.FormAccessRoles.Monitor )
      {
        authorType = Evado.Model.Digital.EvFormRecordComment.AuthorTypeCodes.Monitor;
      }

      // 
      // Get the static test comment field value.
      // 
      if ( this._FormAccessRole == EdRecord.FormAccessRoles.Data_Manager
        || this._FormAccessRole == EdRecord.FormAccessRoles.Monitor
        || this._FormAccessRole == EdRecord.FormAccessRoles.Record_Author )
      {
        stValue = this.GetParameterValue ( CommandParameters, EuFormGenerator.CONST_FORM_COMMENT_FIELD_ID );

        // 
        // a list of the form field values that have changed.
        // 
        if ( this._FieldValueChange.Count > 0 )
        {
          if ( stValue != String.Empty )
          {
            stValue += "\r\n";
          }
          stValue += "The following field values have been changed:";

          // 
          // Iterate through the values.
          // 
          foreach ( EvDataChangeItem item in this._FieldValueChange )
          {
            stValue += "\r\n" + item.ItemId + EvLabels.Label_ChangeInitialValue + item.InitialValue + EvLabels.Label_ChangeNewValue + item.NewValue;
          }
        }

        // 
        // Does the returned field value exist
        // 
        if ( stValue != null )
        {
          if ( stValue != String.Empty )
          {
            Evado.Model.Digital.EvFormRecordComment comment = new Evado.Model.Digital.EvFormRecordComment (
            Form.Guid,
            Guid.Empty,
            authorType,
            this.Session.UserProfile.UserId,
            this.Session.UserProfile.CommonName,
            stValue
          );

            comment.NewComment = true;

            Form.CommentList.Add ( comment );

          }//END value has content.

        }//END Value exists.
      }

    }//END updateFormComments method.

    //  =============================================================================== 
    /// <summary>
    /// Description:
    ///   This method updates a test field object.
    /// 
    /// </summary>
    /// <param name="FormField">  Evado.Model.Digital.EvFormField object containing test field ResultData.</param>
    /// <param name="CommandParameters">Containing the returned formfield values.</param>
    /// <param name="FormState">Current FormRecord state</param>
    /// <param name="DisplayState">Channel display state</param>
    /// <param name="User">The user's profile.</param>
    /// <returns>Returns a   Evado.Model.Digital.EvFormField object.</returns>
    //  ----------------------------------------------------------------------------------
    private void updateFormField (
      List<Evado.Model.UniForm.Parameter> CommandParameters,
        Evado.Model.Digital.EdRecordField FormField,
         Evado.Model.Digital.EdRecordObjectStates FormState )
    {

      this.LogMethod ( "updateFormField" );
      this.LogDebug ( "FieldId: " + FormField.FieldId );
      this.LogDebug ( "TypeId: " + FormField.TypeId );
      //this.LogDebugValue ( "Field State: " + FormField.State );
      // 
      // Initialise methods variables and objects.
      // 
      string stValue = String.Empty;
      string stAnnotation = String.Empty;
      string stQuery = String.Empty;

      /**********************************************************************************/
      // Update computed fields.
      //
      if ( FormField.TypeId == Evado.Model.EvDataTypes.Computed_Field )
      {
        //this.LogDebugValue ( "Process Computed Field" );
        //
        // Update the field value if it has changed.
        //
        this.updateTextField (
          CommandParameters,
          FormField,
          false );

      }// END Update single value fields.

      /**********************************************************************************/
      // 
      // If the test is in EDIT mode update the fields values.
      // 
      if ( this._FormAccessRole == EdRecord.FormAccessRoles.Record_Author
        || this._FormAccessRole == EdRecord.FormAccessRoles.Data_Manager
        || this._FormAccessRole == EdRecord.FormAccessRoles.Patient )
      {

        switch ( FormField.TypeId )
        {
          // 
          // If field type is a free lowerText value update it 
          // 
          case Evado.Model.EvDataTypes.Free_Text:
          case Evado.Model.EvDataTypes.Signature:
            {
              //this.LogDebugValue ( "Process free text or signature field." );
              //
              // Update the field value if it has changed.
              //
              this.updateTextField (
                CommandParameters,
                FormField,
                true );

              //this.LogDebugValue ( "Value: " + FormField.ItemText );
              break;
            }// END Update single value fields.

          case Evado.Model.EvDataTypes.Special_Matrix:
          case Evado.Model.EvDataTypes.Table:
            {
              //this.LogDebugValue ( "Process table field." );

              this.updateFormTableFields ( CommandParameters, FormField );
              break;

            }//END processing table or matrix
          case EvDataTypes.User_Endorsement:
            {
              this.updateUserEndorcementField (
                CommandParameters,
                FormField);
              break;
            }
          default:
            {
              //this.LogDebugValue ( "Process single value field." );
              //
              // Update the field value if it has changed.
              //
              this.updateTextField (
                CommandParameters,
                FormField,
                false );

              break;

            }// END Update single value fields.

        }//END stiwtch statement.

        //this.LogDebugValue ( "Final field state: " + FormField.State );

      }//END updating field

      // 
      // Get the field annotation
      // 
      this.updateFormFieldAnnotation (
        CommandParameters,
        FormField,
        FormState );

      // 
      // return the form comment for value change.
      // 
      this.LogMethodEnd ( "updateFormField" );
      return;

    }//END updateFormField method

    //  =============================================================================== 
    /// <summary>
    /// This method updates the text field object in the form field with the values from parameter list.
    /// 
    /// Description:
    ///   This method updates the test table field values.
    /// 
    /// </summary>
    /// <param name="FormField">  Evado.Model.Digital.EvFormField object containing test field ResultData.</param>
    /// <param name="CommandParameters">Containing the returned formfield values.</param>
    /// <param name="IsFreeText">boolean: if the field is free text or not.</param>
    /// <returns>String Returns a form field commenet.</returns>
    //  ----------------------------------------------------------------------------------
    private void updateTextField (
      List<Evado.Model.UniForm.Parameter> CommandParameters,
        Evado.Model.Digital.EdRecordField FormField,
       bool IsFreeText )
    {
      //this.LogMethod ( "updateTextField method " );
      //this.LogDebugValue ( "FormField.FieldId: " + FormField.FieldId );
      //this.LogDebugValue ( "IsFreeText: " + IsFreeText );

      String stValue = this.GetParameterValue ( CommandParameters, FormField.FieldId );
      //this.LogDebugValue ( "stValue: " + stValue );

      // 
      // Does the returned field value exist
      // 
      if ( stValue != null )
      {
        // 
        // Update the form field value if it has changed.
        // 
        if ( FormField.ItemValue != stValue )
        {
          if ( FormField.TypeId == Evado.Model.EvDataTypes.Numeric
            && stValue.ToLower ( ) == Evado.Model.Digital.EvcStatics.CONST_NUMERIC_NOT_AVAILABLE.ToLower ( ) )
          {
            float errors = Evado.Model.Digital.EvcStatics.CONST_NUMERIC_NULL;
            stValue = errors.ToString ( );
          }

          // 
          // Set field value.
          // 
          if ( IsFreeText == true )
          {
            FormField.ItemText = stValue;
          }
          else
          {
           // this.LogDebugValue ( "Field Change: FieldId: '" + FormField.FieldId
           //  + "' Old: '" + FormField.ItemValue + "' New: '" + stValue + "' " );

            FormField.ItemValue = stValue;

            // 
            // add the value change object to generate the record change comment.
            // 
            this._FieldValueChange.Add ( new EvDataChangeItem (
              FormField.FieldId + " - " + FormField.Title,
              FormField.ItemValue,
              stValue ) );
          }

        }//END Update field value.

      }//END Value exists.

    }//END updateTextField method

    //  =============================================================================== 
    /// <summary>
    /// This method updates the text field object in the form field with the values from parameter list.
    /// 
    /// Description:
    ///   This method updates the test table field values.
    /// 
    /// </summary>
    /// <param name="FormField">  Evado.Model.Digital.EvFormField object containing test field ResultData.</param>
    /// <param name="CommandParameters">Containing the returned formfield values.</param>
    /// <param name="IsFreeText">boolean: if the field is free text or not.</param>
    /// <returns>String Returns a form field commenet.</returns>
    //  ----------------------------------------------------------------------------------
    private void updateUserEndorcementField (
      List<Evado.Model.UniForm.Parameter> CommandParameters,
        Evado.Model.Digital.EdRecordField FormField )
    {
      //this.LogMethod ( "updateUserEndorcementField method " );
      //this.LogDebugValue ( "FormField.FieldId: " + FormField.FieldId );
      //this.LogDebugValue ( "FormField.Section: " + FormField.Design.Section );

      String stValue = this.GetParameterValue ( CommandParameters, FormField.FieldId );
      //this.LogDebugValue ( "stValue: " + stValue );

      EvFormSection section = this.getSection ( FormField );
      if ( section != null )
      {
        //this.LogDebugValue ( "Title: " + section.Title );
        if ( section.hasRole ( this._FormAccessRole ) == false )
        {
          //this.LogDebugValue ( "The user does not have update access to the endorsment field" );
          //this.LogMethodEnd ( "updateUserEndorcementField" );
          return;
        }
      }

      // 
      // Does the returned field value exist
      // 
      if ( stValue != null )
      {
        // 
        // Update the form field value if it has changed.
        // 
        if ( FormField.ItemValue != stValue )
        {
          //  this.LogDebugValue ( "Field Change: FieldId: '" + FormField.FieldId
          //   + "' Old: '" + FormField.ItemValue + "' New: '" + stValue + "' " );

            FormField.ItemValue = stValue;

            // 
            // add the value change object to generate the record change comment.
            // 
            this._FieldValueChange.Add ( new EvDataChangeItem (
              FormField.FieldId + " - " + FormField.Title,
              FormField.ItemValue,
              stValue ) );

        }//END Update field value.

      }//END Value exists.

      //this.LogMethodEnd ( "updateUserEndorcementField" );
    }//END updateTextField method

    //  =============================================================================== 
    /// <summary>
    /// this method gets the section object for a section Id
    /// </summary>
    /// <param name="SectionId">String Section idenifier</param>
    /// <returns>EvFormSection object</returns>
    //  ----------------------------------------------------------------------------------
    private EvFormSection getSection ( EdRecordField FormField )
    {
      //this.LogMethod ( "updateUserEndorcementField method " );
      //this.LogDebugValue ( "Design.Section: " + FormField.Design.Section );
      //
      // Iterate through teh sections and return the matching section object
      //
      foreach ( EvFormSection section in this._Sections )
      {
        //this.LogDebugValue ( "section.Section: " + section.Section );

        if ( section.Section == FormField.Design.Section
          || section.No.ToString() == FormField.Design.Section)
        {
          return section;
        }
      }

      return null;
    }

    //  =============================================================================== 
    /// <summary>
    /// updateFormFieldTable method.
    /// 
    /// Description:
    ///   This method updates the test table field values.
    /// 
    /// </summary>
    /// <param name="FormField">  Evado.Model.Digital.EvFormField object containing test field ResultData.</param>
    /// <param name="CommandParameters">Containing the returned formfield values.</param>
    /// <returns>String Returns a form field commenet.</returns>
    //  ----------------------------------------------------------------------------------
    private void updateFormTableFields (
      List<Evado.Model.UniForm.Parameter> CommandParameters,
        Evado.Model.Digital.EdRecordField FormField )
    {
      this.LogMethod ( "updateFormTableFields method " );
      //this.LogDebugValue ( "FieldId: " + FormField.FieldId );
      //this.LogDebugValue ( "PreFilledColumnList: " + FormField.Table.PreFilledColumnList );

      // 
      // Iterate through the rows and columns of the table filling the 
      // ResultData object with the test values.
      // 
      for ( int inRow = 0; inRow < FormField.Table.Rows.Count; inRow++ )
      {
        for ( int inCol = 0; inCol < FormField.Table.ColumnCount; inCol++ )
        {
          // 
          // construct the test table field name.
          // 
          string tableFieldId = FormField.FieldId + "_" + ( inRow + 1 ) + "_" + ( inCol + 1 );
          //this.LogDebugValue ( "form field column control Id: " + tableFieldId );

          // 
          // Get the table field and update the test field object.
          // 
          string stValue = this.GetParameterValue ( CommandParameters, tableFieldId );
          // 
          // Does the returned field value exist
          // 
          if ( stValue != null )
          {
           // this.LogDebugValue ( " value: " + stValue
           //     + " TypeId: " + FormField.Table.Header [ inCol ].TypeId );

            // 
            // Update it if it has changed.
            // 
            if ( FormField.Table.Rows [ inRow ].Column [ inCol ] != stValue )
            {
              // 
              // If NA is entered set to numeric null.
              // 
              if ( FormField.Table.Header [ inCol ].TypeId == Evado.Model.Digital.EdRecordTableHeader.ItemTypeNumeric )
              {
                if ( stValue.ToLower ( ) == Evado.Model.Digital.EvcStatics.CONST_NUMERIC_NOT_AVAILABLE.ToLower ( ) )
                {
                  stValue = Evado.Model.Digital.EvcStatics.CONST_NUMERIC_NULL.ToString ( );
                }
              }

              // 
              // add the value change object to generate the record change comment.
              // 
              this._FieldValueChange.Add ( new EvDataChangeItem (
                FormField.FieldId
                + " - " + FormField.Title
                + " R: " + inRow
                + " C: " + inCol,
                 FormField.Table.Rows [ inRow ].Column [ inCol ],
                stValue ) );

              FormField.Table.Rows [ inRow ].Column [ inCol ] = stValue;

              // If there are test table value the set the field to entered.
              if ( stValue != String.Empty
                && FormField.State == Evado.Model.Digital.EdRecordField.FieldStates.Empty )
              {
                FormField.State = Evado.Model.Digital.EdRecordField.FieldStates.With_Value;
                FormField.ItemValue = "Entered";
              }

            }//END cell value changed

          }//END value exists.

        }//END column interation loop

      }//END row interation loop

      return;

    }//END updateFormFieldTable method

    //  =============================================================================== 
    /// <summary>
    /// Description:
    ///   This method generates the Java script object variables for the test.
    /// 
    /// </summary>
    /// <param name="CommandParameters">The list of returned fields.</param>
    /// <param name="FormField">The form field to be updated.</param>
    /// <param name="FormState">The field state</param>
    /// <param name="DisplayState">The display state</param>
    /// <param name="User">The user's profile</param>
    //  ----------------------------------------------------------------------------------

    private void updateFormFieldAnnotation (
      List<Evado.Model.UniForm.Parameter> CommandParameters,
        Evado.Model.Digital.EdRecordField FormField,
        Evado.Model.Digital.EdRecordObjectStates FormState )
    {
      this.LogMethod ( "updateFormFieldAnnotation method" );
      // 
      // Initialise the method variables
      // 
      string sAnnotation = String.Empty;

      // 
      // In this currentSchedule state not annotation can be added to the field so exit.
      // 
      if ( this._FormAccessRole == EdRecord.FormAccessRoles.Form_Designer
        || this._FormAccessRole == EdRecord.FormAccessRoles.Record_Reader )
      {
        this.LogDebug ( "No annotation are collected" );
        return;
      }

      //this.LogDebugValue ( "FieldId: " + FormField.FieldId );
      //this.LogDebugValue ( "Query control ID: " + FormField.FieldId + Evado.Model.UniForm.Field.CONST_FIELD_QUERY_SUFFIX );
      //this.LogDebugValue ( "Annotation control ID: " + FormField.FieldId + Evado.Model.UniForm.Field.CONST_FIELD_ANNOTATION_SUFFIX );
      // 
      // Process the test field for test edit mode.
      // 
      if ( this._FormAccessRole == EdRecord.FormAccessRoles.Record_Author
        || this._FormAccessRole == EdRecord.FormAccessRoles.Monitor
        || this._FormAccessRole == EdRecord.FormAccessRoles.Data_Manager )
      {
        this.LogDebug ( " FormDisplayStates Edit" );
        // 
        // Update comments fields for an queried letter
        // 
        if ( FormState == Evado.Model.Digital.EdRecordObjectStates.Queried_Record )
        {
          FormField.State = Evado.Model.Digital.EdRecordField.FieldStates.With_Value;
          FormField.Action = Evado.Bll.Clinical.EvFormRecordFields.ActionSaveItem;
          sAnnotation = "Value Updated: ";
        }

      }//END test currentSchedule state 'Edit'

      // 
      // Process the test field for test edit mode.
      // 
      if ( this._FormAccessRole == EdRecord.FormAccessRoles.Data_Manager
        || this._FormAccessRole == EdRecord.FormAccessRoles.Monitor )
      {
        this.LogDebug ( "FormDisplayStates Review" );
        // 
        // Get the query check box value.
        // 
        string stQuery = this.GetParameterValue (
          CommandParameters,
          FormField.FieldId + Evado.Model.UniForm.Field.CONST_FIELD_QUERY_SUFFIX );

        // 
        // If the value exists then process the query
        // 
        if ( stQuery != null )
        {
          if ( stQuery.ToLower ( ) == "on"
            || stQuery.ToLower ( ) == "true"
            || stQuery.ToLower ( ) == "checked"
            || stQuery.ToLower ( ) == "yes" )
          {
            FormField.State = Evado.Model.Digital.EdRecordField.FieldStates.Queried;
            FormField.Action = Evado.Bll.Clinical.EvFormRecordFields.ActionQueryItem;
            sAnnotation = "Value Queried: ";
          }//END query checked

        }//END END query exits.
        else
        {
          sAnnotation = "Value Confirmed: ";
          FormField.State = Evado.Model.Digital.EdRecordField.FieldStates.Confirmed;
          FormField.Action = Evado.Bll.Clinical.EvFormRecordFields.ActionConfirmItem;
        }

      }//END Reviewing field value     

      // 
      // Get xml test field values
      // 
      string stAnnotation = this.GetParameterValue (
        CommandParameters,
        FormField.FieldId + Evado.Model.UniForm.Field.CONST_FIELD_ANNOTATION_SUFFIX );

      // 
      // Process annotation if not null.
      // 
      if ( stAnnotation != null )
      {
        // 
        // If the annotation exists then add it to the field annotation list.
        // 
        if ( stAnnotation != String.Empty )
        {
          stAnnotation.Replace ( "\r", String.Empty );
          stAnnotation.Replace ( "\n", String.Empty );
          stAnnotation.Replace ( "\t", String.Empty );

          this.LogDebug ( "Annotation: '" + stAnnotation + "'" );

          // 
          // Initialise the comment object.
          // 
          Evado.Model.Digital.EvFormRecordComment comment = new Evado.Model.Digital.EvFormRecordComment ( );
          comment.RecordGuid = FormField.RecordGuid;
          comment.RecordFieldGuid = FormField.Guid;
          comment.UserId = this.Session.UserProfile.UserId;
          comment.UserCommonName = this.Session.UserProfile.CommonName;
          comment.CommentDate = DateTime.Now;
          comment.NewComment = true;
          comment.CommentType = Evado.Model.Digital.EvFormRecordComment.CommentTypeCodes.Form_Field;

          // 
          // Set the annotation author type.
          // 
          comment.AuthorType = Evado.Model.Digital.EvFormRecordComment.AuthorTypeCodes.Record_Author;

          // 
          // set the annotation content.
          // 
          comment.Content = sAnnotation + stAnnotation;

          // 
          // Set the annotation type
          // 
          if ( this._FormAccessRole == EdRecord.FormAccessRoles.Monitor
            && FormField.State == Evado.Model.Digital.EdRecordField.FieldStates.Queried )
          {
            comment.AuthorType = Evado.Model.Digital.EvFormRecordComment.AuthorTypeCodes.Monitor;
          }

          if ( this._FormAccessRole == EdRecord.FormAccessRoles.Data_Manager
            && FormField.State == Evado.Model.Digital.EdRecordField.FieldStates.Queried )
          {
            comment.AuthorType = Evado.Model.Digital.EvFormRecordComment.AuthorTypeCodes.Data_Manager;
          }

          // 
          // Update the annotation property
          // 
          FormField.CommentList.Add ( comment );

        }//END annotation not empty.

      }//END Annotation not null.

    }//END updateFormFieldAnnotation method

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region    Evado.Model.Digital.EvForm Java Script generation methods.

    //  =============================================================================== 
    /// <summary>
    ///   This method generates the Java script object variables for the form.
    /// 
    /// </summary>
    /// <param name="FormGuid"> Guid: containing the page Guid.</param>
    /// <param name="FieldList">List<  Evado.Model.Digital.EvFormField> object containing a list of fields.</param>
    /// <param name="ClientDataObject">UniForm.Model.Page objectthe Uniform client page content.</param>
    /// <param name="BinaryFilePath">string binary file path.</param>
    //  ----------------------------------------------------------------------------------
    private void getFormJavaScript (
      Guid PageGuid,
      List<Evado.Model.Digital.EdRecordField> FieldList,
      Evado.Model.UniForm.Page ClientDataObject,
      String BinaryFilePath )
    {
      this.LogMethod ( "getFormJavaScript" );
      this.LogDebug ( "BinaryFilePath: " + BinaryFilePath );

      //
      // Initialise the methods variables and objects.
      //
      String stJavaFileName = PageGuid + ".js";
      String stJavaScript = String.Empty;

      //
      // Exit if the field list length is 0
      //
      if ( FieldList.Count == 0 )
      {
        this.LogMethodEnd ( "getFormJavaScript" );
        return;
      }

      this.LogDebug ( "javaScriptFileName: " + stJavaFileName );

      // 
      // Add the computed field scripts
      // 
      stJavaScript += this.getComputedFieldScripts ( FieldList );

      // 
      // Add the custom validation scripts
      // 
      stJavaScript += this.getCustomValidationScripts ( FieldList );

      //
      // replace null with undefined (JS compatibility).
      //
      stJavaScript = stJavaScript.Replace ( "null", "undefined" );

      if ( stJavaScript.Length < 145 )
      {
        this.LogMethodEnd ( "getFormJavaScript" );
        return;
      }

      // 
      // Open the script tag
      // 
      ClientDataObject.JsLibrary = stJavaFileName;

      this.LogDebug ( "Java Script: \r\n" + stJavaScript );

      //
      // Save the java script to the UniForm binary file path
      //
      Evado.Model.Digital.EvcStatics.Files.saveFile ( BinaryFilePath, stJavaFileName, stJavaScript.ToString ( ) );

      this.LogMethodEnd ( "getFormJavaScript" );
    }//END getFormJavaScript method

    //  =============================================================================== 
    /// <summary>
    /// Description:
    ///   This method generates the Java script computed fields.
    /// 
    /// </summary>
    /// <param name="FieldList">List<  Evado.Model.Digital.EvFormField> object containing a list of fields.</param>
    /// <returns>String:  Java Script source code</returns>
    //  ----------------------------------------------------------------------------------
    private String getComputedFieldScripts (
      List<Evado.Model.Digital.EdRecordField> FieldList )
    {
      this.LogMethod ( "getComputedFieldScripts smethod. " );
      // 
      // Initialise the methods objects and variables.
      // 
      System.Text.StringBuilder sbJScript = new StringBuilder ( );

      //
      // Initialise the computedField script.
      //
      sbJScript.AppendLine ( "computedScript = true ;" );

      sbJScript.AppendLine ( "function computedFields()\r\n{" );
      sbJScript.AppendLine ( " var value = 0;" );
      sbJScript.AppendLine ( " var field  ;" );
      sbJScript.AppendLine ( " var fieldDisp ;" );

      if ( this._JavaDebug == true )
      {
        sbJScript.AppendLine ( "console.log( \"Executing: computedFields method.\" );" );
      }

      // 
      // Iterate through the form fields to extract field values.
      // 
      foreach ( Evado.Model.Digital.EdRecordField field in FieldList )
      {
        // 
        // Extract the computed fields script and add it to the function.
        // 
        if ( field.TypeId == Evado.Model.EvDataTypes.Computed_Field
          && field.Design.JavaScript != String.Empty )
        {
          this.LogDebug ( "FieldId: " + field.FieldId
          + ", Type: " + field.TypeId
          + ", Section: " + field.Design.Section
          + " >> JavaScript: Exists " );

          // 
          // Generate the form field name
          // 
          string fieldId = field.FieldId;

          if ( this._JavaDebug == true )
          {
            sbJScript.AppendLine ( " console.log( \"Computed Field: " + fieldId + ", executing.\" );" );
          }

          String stVlidationScript = field.Design.JavaScript.Replace ( "\n", String.Empty );
          stVlidationScript = stVlidationScript.Replace ( "\r", String.Empty );
          stVlidationScript = stVlidationScript.Replace ( "}", "}\r\n " );
          stVlidationScript = stVlidationScript.Replace ( CONST_FORM_FIELD_PREFIX, String.Empty );
          stVlidationScript = stVlidationScript.Replace ( CONST_SUBJECT_PREFIX, String.Empty );
          stVlidationScript = stVlidationScript.Replace ( CONST_FORM_PREFIX, String.Empty );
          stVlidationScript = stVlidationScript.Replace ( CONST_Subject_Milestone_PREFIX, String.Empty );
          // 
          // Generate the java script to be added to the script
          // 
          sbJScript.AppendLine ( " // Field: " + fieldId + " computed field script." );
          sbJScript.AppendLine ( " field = document.getElementById( \"" + fieldId + "\" );" );
          sbJScript.AppendLine ( " fieldDisp = document.getElementById( \"" + fieldId + "_Disp\" );" );
          sbJScript.AppendLine ( " try{" );
          sbJScript.AppendLine ( " if ( field != undefined ){" );
          sbJScript.AppendLine ( "  " + stVlidationScript );
          sbJScript.AppendLine ( "  field.value = value; " );
          sbJScript.AppendLine ( " }" );
          sbJScript.AppendLine ( " }" );
          sbJScript.AppendLine ( " catch(error){" );
          sbJScript.AppendLine ( "  var errText = \"Field: " + fieldId + " has computation a JavaScript Error.\"; " );
          sbJScript.AppendLine ( "  errText += \"\\r\\n\\r\\nError Description: \" + error.description ; " );
          sbJScript.AppendLine ( "  alert( errText ); " );
          sbJScript.AppendLine ( " }" );
          sbJScript.AppendLine ( "//----------------------------------------" );

        }//END process computed field script.

      }//END field iteration loop.

      if ( this._JavaDebug == true )
      {
        sbJScript.AppendLine ( "console.log( \"Computed fields updates successfully.\" );" );
      }

      // 
      // Close the defineVariables java method.
      // 
      sbJScript.AppendLine ( "}//END Computed Field function\r\n" );

      //
      // Return the java Script
      //
      return sbJScript.ToString ( );

    }//END generateComputedFieldScripts method

    //  =============================================================================== 
    /// <summary>
    /// Description:
    ///   This method generates the Java script custom validation.
    /// 
    /// </summary>
    /// <param name="FieldList">  List< Evado.Model.Digital.EvFormField> object.</param>
    /// <returns>Returns a string containing the Java Scripts.</returns>
    //  ----------------------------------------------------------------------------------
    public String getCustomValidationScripts (
      List<Evado.Model.Digital.EdRecordField> FieldList )
    {
      this.LogMethod ( "getCustomValidationScripts" );
      // 
      // Initialise the methods objects and variables.
      // 
      System.Text.StringBuilder sbJScript = new StringBuilder ( );

      // 
      // Iterate through the form fields to extract field values.
      // 
      foreach ( Evado.Model.Digital.EdRecordField field in FieldList )
      {
        // 
        // Extract the computed fields script and add it to the function.
        // 
        if ( field.TypeId != Evado.Model.EvDataTypes.Computed_Field
          && field.Design.JavaScript != String.Empty )
        {
          // 
          // Generate the form field name
          // 
          string FieldId = field.FieldId;

          //
          // Extract the custom validation script from the field validation rules.
          //
          String stComputedFieldScript = field.Design.JavaScript.Replace ( "\n", String.Empty );
          stComputedFieldScript = stComputedFieldScript.Replace ( "\r", String.Empty );
          stComputedFieldScript = stComputedFieldScript.Replace ( "}", "}\r\n " );
          stComputedFieldScript = stComputedFieldScript.Replace ( CONST_FORM_FIELD_PREFIX, String.Empty );
          stComputedFieldScript = stComputedFieldScript.Replace ( CONST_SUBJECT_PREFIX, String.Empty );
          stComputedFieldScript = stComputedFieldScript.Replace ( CONST_FORM_PREFIX, String.Empty );
          stComputedFieldScript = stComputedFieldScript.Replace ( CONST_Subject_Milestone_PREFIX, String.Empty );
          // 
          // Generate the java script to be added to the script
          // 
          sbJScript.AppendLine ( "//Field: " + FieldId + " custom validation  script." );
          sbJScript.AppendLine ( "function " + FieldId + "_Validation( source, FieldId, OldValue ){" );
          sbJScript.AppendLine ( " try{" );
          sbJScript.AppendLine ( stComputedFieldScript );
          sbJScript.AppendLine ( "computedFields(); " );
          sbJScript.AppendLine ( " }" );
          sbJScript.AppendLine ( " catch(error){" );
          sbJScript.AppendLine ( "  var errText = \"Field: " + FieldId + " validation has a JavaScript Error.\"; " );
          sbJScript.AppendLine ( "  errText += \"\\r\\n\\r\\nError Description: \" + error.description ;" );
          sbJScript.AppendLine ( "  alert( errText ); " );
          sbJScript.AppendLine ( " }" );
          sbJScript.AppendLine ( "}" );

        }//END process computed field script.

      }//END field iteration loop.

      // 
      // Close the defineVariables java method.
      // 
      sbJScript.AppendLine ( "\r\n" );

      //
      // Return the java Script
      //
      return sbJScript.ToString ( );

    }//END generateCustomValidationScripts method

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END EvHtmlFormGeneration
}
