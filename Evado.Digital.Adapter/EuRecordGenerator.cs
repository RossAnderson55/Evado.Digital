/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\PageGenerator.cs" company="EVADO HOLDING PTY. LTD.">
 *     fieldGroup.ParametersfieldGroup.Parameters
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
 
using Evado.Digital.Bll;
using Evado.Digital.Model;
using MarkdownSharp;

namespace Evado.Digital.Adapter
{
  //  =================================================================================
  /// <summary>
  ///  This class dynamically generate Evado Forms using class libraries, using (methods)
  /// </summary>
  //  ---------------------------------------------------------------------------------
  public class EuRecordGenerator : EuClassAdapterBase
  {
    #region Class initialisation methods.

    // ==================================================================================
    /// <summary>
    /// This method initialises the class and passs in the initialisation objects.
    /// </summary>
    /// <param name="AdapterObjects">EuGlobalObjects object</param>
    /// <param name="ServiceUserProfile">EvUserProfileBase object</param>
    /// <param name="SessionObjects">EuSession object</param>
    /// <param name="UniFormBinaryFilePath">String: UniForm Binary file path.</param>
    /// <param name="UniForm_BinaryServiceUrl">String UniFORm binary service URL</param>
    /// <param name="ClassParameters">EvClassParameters class parameters</param>
    //  ----------------------------------------------------------------------------------
    public EuRecordGenerator (
      EuGlobalObjects AdapterObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      String UniForm_BinaryServiceUrl,
      EvClassParameters ClassParameters )
    {
      this.ClassNameSpace = "Evado.UniForm.Model.EuRecordGenerator.";
      this.AdapterObjects = AdapterObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.UniForm_BinaryServiceUrl = UniForm_BinaryServiceUrl;
      this.ClassParameters = ClassParameters;

      this.LogInit ( "UniForm_ImageFilePath: " + UniForm_ImageFilePath );

      this._ModuleList = new List<EdModuleCodes> ( );

      this.LoggingLevel = ClassParameters.LoggingLevel;
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

    /// <summary>
    /// This constand define the field to enable editing.
    /// </summary>
    private const string CONST_ENABLE_EDIT_FIELD = "ENED";

    private string RecordQueryAnnotation = String.Empty;

    //
    // Initialise the page labels
    //

    // ***********************************************************************************
    #endregion

    #region Properties and variables..

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
    EdRecordDesign _Design = new EdRecordDesign ( );

    //
    // The form access role
    //
    EdRecord.FormAccessRoles _FormAccessRole = EdRecord.FormAccessRoles.Null;

    //
    // Lists the Evado Clinical modules that are enabled.
    //
    private List<EdModuleCodes> _ModuleList = new List<EdModuleCodes> ( );

    // 
    // this list is used to create the field value change comment when a user updates a record's values.
    // 
    private List<EvDataChangeItem> _FieldValueChange = new List<EvDataChangeItem> ( );

    // 
    // Used internally selection field enablement.
    // 
    private List<Evado.Digital.Model.EdRecordSection> _Sections = new List<Evado.Digital.Model.EdRecordSection> ( );

    // 
    // Used internally selection field enablement.
    // 
    private List<Evado.Digital.Model.EdRecordField> _Fields = new List<Evado.Digital.Model.EdRecordField> ( );

    private Evado.UniForm.Model.FieldLayoutCodes _DefaultFieldLayout = EuAdapter.DefaultFieldLayout;

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
    ///   This method generates an instance of the Entity object, merging entity data with 
    ///   the entity definition.  
    /// </summary>
    /// <param name="Entity"> Evado.Digital.Model.EdRecord object</param>
    /// <param name="PageObject">Evado.UniForm.Model.Page object</param>
    /// <param name="BinaryFilePath">String: the path to the UniForm binary file store.</param>
    /// <returns>bool:  true = page generated without error.</returns>
    /// <remarks>
    /// This method consists of following steps: 
    /// 1. Find the matching record layout
    /// 2. Update the record's layout descriptions and field definitions.
    /// 3. Call the generateLayout method to generate the page layout.
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public bool generateEntityLayout (
       Evado.Digital.Model.EdRecord Entity,
      Evado.UniForm.Model.Page PageObject,
      String BinaryFilePath )
    {
      this.LogMethod ( "generateEntityLayout" );
      this.LogDebug ( "Entity.Title: " + Entity.Title );
      this.LogDebug ( "Entity.State: " + Entity.State );
      this.LogDebug ( "Entity.Design.FooterFormat: " + Entity.Design.FooterFormat );
      //
      // Initialise the methods variables and objects.
      //
      EdRecord layout = new EdRecord ( );

      foreach ( EdRecord lay in this.AdapterObjects.AllEntityLayouts )
      {
        if ( Entity.LayoutGuid == lay.Guid )
        {
          layout = lay;
          break;
        }
      }//End layout selection iteration loop.

      this.LogDebug ( "G: {0}, T: {1}.", layout.Guid, layout.Title );

      //
      // Merge the layout with the record.
      //
      Entity.AiIndex = layout.AiIndex;
      Entity.Design = layout.Design;

      for ( int i = 0; i < Entity.Fields.Count; i++ )
      {
        for ( int j = 0; j < layout.Fields.Count; j++ )
        {
          if ( Entity.Fields [ i ].FieldGuid == layout.Fields [ j ].Guid )
          {
            Entity.Fields [ i ].LayoutId = layout.Fields [ j ].LayoutId;
            Entity.Fields [ i ].FieldId = layout.Fields [ j ].FieldId;
            Entity.Fields [ i ].Design = layout.Fields [ j ].Design;
            if ( Entity.Fields [ i ].Table != null )
            {
              Entity.Fields [ i ].Table.Header = layout.Fields [ j ].Table.Header;
            }
          }//END field match 
        }//END layout field iteration loop
      }//END record field interatoin loop.

      //
      // generate the record layout.
      //
      bool result = this.generateLayout ( Entity, PageObject, BinaryFilePath );

      this.LogMethodEnd ( "generateEntityLayout" );
      return result;
    }//END public generateEntityLayout Method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates an instance of the Record object, merging entity data with 
    ///   the record definition.  
    /// </summary>
    /// <param name="Entity">  Evado.Digital.Model.EdRecord object</param>
    /// <param name="PageObject">Evado.UniForm.Model.Page object</param>
    /// <param name="BinaryFilePath">String: the path to the UniForm binary file store.</param>
    /// <returns>bool:  true = page generated without error.</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 1. Find the matching record layout
    /// 2. Update the record's layout descriptions and field definitions.
    /// 3. Call the generateLayout method to generate the page layout.
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public bool generateRecordLayout (
       Evado.Digital.Model.EdRecord Record,
      Evado.UniForm.Model.Page PageObject,
      String BinaryFilePath )
    {
      this.LogMethod ( "generateRecordLayout" );
      this.LogDebug ( "Record.Title: " + Record.Title );
      this.LogDebug ( "Record.State: " + Record.State );

      //
      // Initialise the methods variables and objects.
      //
      EdRecord layout = new EdRecord ( );

      foreach ( EdRecord lay in this.AdapterObjects.AllRecordLayouts )
      {
        if ( Record.LayoutGuid == lay.Guid )
        {
          layout = lay;
          break;
        }
      }//End layout selection iteration loop.

      this.LogDebug ( "G: {0}, T: {1}.", layout.Guid, layout.Title );

      //
      // Merge the layout with the record.
      //
      Record.AiIndex = layout.AiIndex;
      Record.Design = layout.Design;

      for ( int i = 0; i < Record.Fields.Count; i++ )
      {
        for ( int j = 0; j < layout.Fields.Count; j++ )
        {
          if ( Record.Fields [ i ].FieldGuid == layout.Fields [ j ].Guid )
          {
            Record.Fields [ i ].LayoutId = layout.Fields [ j ].LayoutId;
            Record.Fields [ i ].FieldId = layout.Fields [ j ].FieldId;
            Record.Fields [ i ].Design = layout.Fields [ j ].Design;
            if ( Record.Fields [ i ].Table != null )
            {
              Record.Fields [ i ].Table.Header = layout.Fields [ j ].Table.Header;
            }
          }//END field match 
        }//END layout field iteration loop
      }//END record field interatoin loop.


      //
      // generate the record layout.
      //
      bool result = this.generateLayout ( Record, PageObject, BinaryFilePath );

      this.LogMethodEnd ( "generateRecordLayout" );
      return result;

    }//END public generateRecordLayout Method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates an instance of the form object.
    /// </summary>
    /// <param name="Layout">  Evado.Digital.Model.EdRecord object</param>
    /// <param name="PageObject">Evado.UniForm.Model.Page object</param>
    /// <param name="BinaryFilePath">String: the path to the UniForm binary file store.</param>
    /// <returns>bool:  true = page generated without error.</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 1. Display the form based on the current view state.
    /// 2. Set the global value field from form field values.
    /// 3. create form record header group.
    /// 4. Create common record group and form field group.
    /// 5. Add javascripts to the form
    /// </remarks>
    // ---------------------------------------------------------------------------------
    public bool generateLayout (
       Evado.Digital.Model.EdRecord Layout,
      Evado.UniForm.Model.Page PageObject,
      String BinaryFilePath )
    {
      this.LogMethod ( "generateLayout" );
      this.LogDebug ( "Layout.Title: " + Layout.Title );
      this.LogDebug ( "Layout.State: " + Layout.State );
      /*
      this.LogDebug ( "Layout.DefaultPageLayout: " + Layout.Design.DefaultPageLayout );
      this.LogDebug ( "Layout.FieldReadonlyDisplayFormat: " + Layout.Design.FieldReadonlyDisplayFormat );
      this.LogDebug ( "Layout.FormAccessRole: " + Layout.FormAccessRole );
      */
      // 
      // Set the default pageMenuGroup type to annotated fields.  This will enable the 
      // client annotation functions and initiate the service to including fields for
      // earlier uniform clients.
      // 
      PageObject.DefaultGroupType = Evado.UniForm.Model.GroupTypes.Default;
      this._FormAccessRole = Layout.FormAccessRole;
      this._FormState = Layout.State;
      this._Fields = Layout.Fields;
      this._Design = Layout.Design;

      //this.LogDebug ( "Default FieldLayout {0}. ", this._DefaultFieldLayout );
      if ( Layout.Design.DefaultPageLayout != null )
      {
        if ( EvStatics.tryParseEnumValue<Evado.UniForm.Model.FieldLayoutCodes> (
          Layout.Design.DefaultPageLayout.ToString ( ),
          out this._DefaultFieldLayout ) == false )
        {
          this._DefaultFieldLayout = EuAdapter.DefaultFieldLayout;
        }
      }
     
      //this.LogDebug ( "FieldLayout {0}. ", this._DefaultFieldLayout );
      //
      // IF the form does not display annotations when being completed
      // hide the annotations by setting hide annotations to true
      //
      if ( Layout.Design.TypeId == EdRecordTypes.Questionnaire )
      {
       // this.LogDebug ( "Questionnaire, Patient Consent or Patient Record so hide annotations. " );
        PageObject.DefaultGroupType = Evado.UniForm.Model.GroupTypes.Default;

        //
        // Hide signature from all non-record editors.
        //
        if ( this._FormAccessRole != EdRecord.FormAccessRoles.Record_Author )
        {
          this._HideSignatureField = true;
        }
      }

     // this.LogDebug ( "ClientPage.DefaultGroupType: " + PageObject.DefaultGroupType );
     // this.LogDebug ( "Layout.State: " + Layout.State );
     // this.LogDebug ( "HideSignatureField: " + this._HideSignatureField );
      // 
      // Set all groups and field to inherited access
      // 
      switch ( this._FormAccessRole )
      {
        case Evado.Digital.Model.EdRecord.FormAccessRoles.Record_Author:
          {
           // this.LogDebug ( "Record Author " );
            PageObject.EditAccess = Evado.UniForm.Model.EditAccess.Enabled;

            break;
          }
        default:
          {
           // this.LogDebug ( "default" );
            PageObject.EditAccess = Evado.UniForm.Model.EditAccess.Disabled;
            PageObject.DefaultGroupType = Evado.UniForm.Model.GroupTypes.Default;
            break;
          }
      }//END view state switch

    //  this.LogDebug ( "Final: ClientPage.DefaultGroupType: " + PageObject.DefaultGroupType );
    //  this.LogDebug ( "Final: HideSignatureField: " + this._HideSignatureField );


      // 
      // Create the form record header groups
      //  
      this.createPageHeader ( Layout, PageObject );

      // 
      // Call the form section create method.
      // 

      this.createFormSections ( Layout, PageObject );

      // 
      // if there is more that one pageMenuGroup create pageMenuGroup category indexes.
      // 
      if ( PageObject.GroupList.Count > 0 )
      {
        this.getFieldCategories ( Layout, PageObject.GroupList [ 0 ] );
      }

      // 
      // Create the form record fooder groups.
      // 
      this.LogDebug ( "PageObject.GroupList.Count: " + PageObject.GroupList.Count );
      this.createPageFooter ( Layout, PageObject );

      this.LogDebug ( "FINAL: PageObject.GroupList.Count: " + PageObject.GroupList.Count );

      // 
      // Add the form specific java scripts
      // 
      this.getFormJavaScript (
        Layout.Guid,
        Layout.Fields,
        PageObject,
        BinaryFilePath );

      // 
      // Fill the form 
      //  
      //this.debugGroup ( Form, ClientPage, ViewState );

      this.LogMethodEnd ( "generateLayout" );
      return true;

    }//END public generateForm Method.

    //=====================================================================================
    /// <summary>
    /// This method selects and returns the selected field and returns null if not found.
    /// </summary>
    /// <param name="FieldId">String: the selected field identifier</param>
    /// <returns>EdRecordField object</returns>
    //-------------------------------------------------------------------------------------
    public EdRecordField GetFieldObject ( String FieldId )
    {
      FieldId = FieldId.Trim ( );
      //
      // iterate through the entity fields to find the selected field.
      //
      foreach ( EdRecordField field in this._Fields )
      {
        if ( field.FieldId.ToLower ( ) == FieldId.ToLower ( ) )
        {
          return field;
        }
      }

      return null;
    }//END GetFieldObject method

    // ==================================================================================
    /// <summary>
    /// This mehod generates the HTMl for a page group.
    /// </summary>
    /// <param name="command">Evado.UniForm.Model.Command command object</param>
    /// <param name="cssClass">String: Css classes</param>
    /// <returns>Html string</returns>
    // ---------------------------------------------------------------------------------
    private String encodeMarkDown (
      String MarkDownText )
    {
      this.LogMethod ( "encodeMarkDown method. " );
      //Global.LogDebugValue ( "Text length: " + MarkDownText.Length );
      //
      // Initialise the methods variables and objects.
      //
      String stMarkDown = String.Empty;

      //
      // Process the text to remove spaces.
      //
      MarkDownText = MarkDownText.Replace ( "\r\n", "~" );
      MarkDownText = MarkDownText.Replace ( "\r", "~" );
      MarkDownText = MarkDownText.Replace ( "\n", "~" );
      MarkDownText = MarkDownText.Replace ( "~~", "~" );

      string [ ] arrMarkDownText = MarkDownText.Split ( '~' );
      foreach ( String str in arrMarkDownText )
      {
        stMarkDown += str.Trim ( ) + "\r\n";
      }
      //Global.LogDebugValue ( stMarkDown );
      //Global.LogDebugValue ( "Processed test length: " + stMarkDown.Length );

      //
      // Initialise the markdown options object.
      //
      MarkdownSharp.MarkdownOptions markDownOptions = new MarkdownSharp.MarkdownOptions ( );
      markDownOptions.AutoHyperlink = true;
      markDownOptions.AutoNewlines = true;
      markDownOptions.EmptyElementSuffix = "/>";
      markDownOptions.EncodeProblemUrlCharacters = true;
      markDownOptions.LinkEmails = true;
      markDownOptions.StrictBoldItalic = true;

      //
      // Initialise the markdown object.
      //
      MarkdownSharp.Markdown markDown = new MarkdownSharp.Markdown ( markDownOptions );

      string stHtml = markDown.Transform ( stMarkDown );

      //
      // convert old markup to html.
      //
      stHtml = stHtml.Replace ( "[i]", "<i>" );
      stHtml = stHtml.Replace ( "[/i]", "</i>" );
      stHtml = stHtml.Replace ( "[b]", "<b>" );
      stHtml = stHtml.Replace ( "[/b]", "</b>" );
      stHtml = stHtml.Replace ( "[u]", "<u>" );
      stHtml = stHtml.Replace ( "[/u]", "</u>" );
      stHtml = stHtml.Replace ( "<blockquote>", "" );
      stHtml = stHtml.Replace ( "</blockquote>", "" );

      this.LogDebug ( "MarkDown HTML: " + stHtml );

      this.LogMethodEnd ( "encodeMarkDown method. " );
      return stHtml;

    }//END encodeMarkDown method.
    // ***********************************************************************************
    #endregion

    #region private page header and footer generation method

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the page header form record page.
    /// </summary>
    /// <param name="Layout"> Evado.Digital.Model.EvForm object</param>
    /// <param name="PageObject">Evado.UniForm.Model.Page object</param>
    //  ---------------------------------------------------------------------------------
    private void createPageHeader (
       Evado.Digital.Model.EdRecord Layout,
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "createFormHeader" );
      this.LogDebug ( "Layout.Design.HeaderFormat: " + Layout.Design.HeaderFormat );
      // 
      // Initialise local variables.
      // 
      Evado.UniForm.Model.Group pageGroup;
      Evado.UniForm.Model.Field groupField = new Evado.UniForm.Model.Field ( );

      if ( Layout.RecordId == String.Empty )
      {
        Layout.RecordId = "RECORD-ID";
      }

      //
      // switch statement to select the header format for the layout.
      //
      switch ( Layout.Design.HeaderFormat )
      {
        case EdRecord.HeaderFormat.No_Header:
          {
            return;
          }
        case EdRecord.HeaderFormat.Author_Header:
          {
            this.createAuthorHeader ( Layout, PageObject );
            return;
          }
        default:
          {
            this.createDefaultHeader ( Layout, PageObject );
            break;
          }
      }//END Switch Statement

      // 
      // Add form record instructions if they exist.
      // 
      if ( Layout.Design.Instructions != String.Empty )
      {
        // 
        // create the page header pageMenuGroup containing the instructions.
        // 
        pageGroup = PageObject.AddGroup (
          String.Empty,
          String.Empty,
          Evado.UniForm.Model.EditAccess.Inherited );
        pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;
        pageGroup.DescriptionAlignment = Evado.UniForm.Model.GroupDescriptionAlignments.Center_Align;
        pageGroup.Description = Layout.Design.Instructions;
      }

      this.LogMethodEnd ( "createFormHeader" );

    }//END createPageHeader Method.

    //  =================================================================================
    /// <summary>
    ///   This method generates a form footer header as html markup.
    /// </summary>
    /// <param name="Layout">   Evado.Digital.Model.EdRecord object .</param>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    //  ---------------------------------------------------------------------------------
    private void createAuthorHeader (
       Evado.Digital.Model.EdRecord Layout,
      Evado.UniForm.Model.Page PageObject )
    {
      // 
      // Initialise local variables.
      // 
      Evado.UniForm.Model.Group pageGroup;
      Evado.UniForm.Model.Field groupField = new Evado.UniForm.Model.Field ( );

      string header = String.Format (
        EdLabels.Layout_Author_Format,
        this.Session.UserProfile.CommonName,
        this.Session.Entity.RecordDate.ToString ( "dd-MMM-yy HH:mm" ) );
      // 
      // Initialise the group if the user is not a patient.
      //
      pageGroup = PageObject.AddGroup ( String.Empty );
      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;
      pageGroup.GroupType = Evado.UniForm.Model.GroupTypes.Default;

      pageGroup.Description = header;
      pageGroup.DescriptionAlignment = Evado.UniForm.Model.GroupDescriptionAlignments.Center_Align;
    }

    //  =================================================================================
    /// <summary>
    ///   This method generates a form footer header as html markup.
    /// </summary>
    /// <param name="Layout">   Evado.Digital.Model.EdRecord object .</param>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    //  ---------------------------------------------------------------------------------
    private void createDefaultHeader (
       Evado.Digital.Model.EdRecord Layout,
      Evado.UniForm.Model.Page PageObject )
    {
      // 
      // Initialise local variables.
      // 
      Evado.UniForm.Model.Group pageGroup;
      Evado.UniForm.Model.Field groupField = new Evado.UniForm.Model.Field ( );

      // 
      // Initialise the group if the user is not a patient.
      //
      pageGroup = PageObject.AddGroup ( String.Empty );
      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;
      pageGroup.GroupType = Evado.UniForm.Model.GroupTypes.Default;

      // 
      // if the design reference object exists include the 
      // using markdown markup.
      // 
      if ( Layout.Design.HttpReference != String.Empty )
      {
        pageGroup.Description =
          String.Format ( "<a href='{0}' target='_blank'>{1}</a>", Layout.Design.HttpReference, Layout.CommandTitle );
        pageGroup.DescriptionAlignment = Evado.UniForm.Model.GroupDescriptionAlignments.Center_Align;
      }
      else
      {
        pageGroup.Description = Layout.CommandTitle;
        pageGroup.DescriptionAlignment = Evado.UniForm.Model.GroupDescriptionAlignments.Center_Align;
      }
    }

    //  =================================================================================
    /// <summary>
    ///   This method generates a form footer header as html markup.
    /// </summary>
    /// <param name="Layout">   Evado.Digital.Model.EdRecord object .</param>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    //  ---------------------------------------------------------------------------------
    private void createPageFooter (
       Evado.Digital.Model.EdRecord Layout,
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "createPageFooter" );
      this.LogDebug ( "Layout.State: " + Layout.State );
      this.LogDebug ( "Layout.Design.FooterFormat: " + Layout.Design.FooterFormat );

      //
      // swtich to select the footer layout format
      //
      switch ( this.Session.Entity.Design.FooterFormat )
      {
        case EdRecord.FooterFormat.No_Footer:
          {
            return;
          }
        case EdRecord.FooterFormat.Author_Footer:
          {
            this.createAuthorHeader ( Layout, PageObject );
            return;
          }
        case EdRecord.FooterFormat.No_Comments:
          //
          // display the layout signoffs.
          //
          this.getSignatureFooter ( Layout, PageObject );
          //
          // display the layout appoval.
          //
          this.getApprovalFooter ( Layout, PageObject );
          {
            break;
          }
        case EdRecord.FooterFormat.No_Signatures:
          {
            this.getCommentFooter ( Layout, PageObject );
            //
            // display the layout appoval.
            //
            this.getApprovalFooter ( Layout, PageObject );
            break;
          }
        default:
          {
            //
            // display the layout comments.
            //
            this.getCommentFooter ( Layout, PageObject );
            //
            // display the layout signoffs.
            //
            this.getSignatureFooter ( Layout, PageObject );
            //
            // display the layout appoval.
            //
            this.getApprovalFooter ( Layout, PageObject );

            break;
          }//END defatult case.
      }//END swtich statement.

      this.LogMethodEnd ( "createPageFooter" );
      return;

    }//END public createPageFooter Method.

    //  =================================================================================
    /// <summary>
    /// This method creates a comment footer group
    /// </summary>
    /// <param name="Layout">   Evado.Digital.Model.EdRecord object.</param>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    private void getCommentFooter (
       Evado.Digital.Model.EdRecord Layout,
      Evado.UniForm.Model.Page PageObject )
    {
      // 
      // Initialise local variables.
      // 
      Evado.UniForm.Model.Group pageGroup;
      Evado.UniForm.Model.Field groupField = new Evado.UniForm.Model.Field ( );

      // 
      // Display the comments.
      // 
      if ( Layout.CommentList.Count > 0
        || this._FormAccessRole == Evado.Digital.Model.EdRecord.FormAccessRoles.Record_Author )
      {
        this.LogDebug ( "display comments." );
        // 
        // Define the comment pageMenuGroup.
        // 
        pageGroup = PageObject.AddGroup (
           String.Empty,
           String.Empty,
           Evado.UniForm.Model.EditAccess.Enabled );
        pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;
        pageGroup.GroupType = Evado.UniForm.Model.GroupTypes.Default;

        // 
        // Display the comment list.
        // 
        if ( Layout.CommentList.Count > 0 )
        {
          groupField = pageGroup.createReadOnlyTextField (
            EuRecordGenerator.CONST_FORM_DISP_COMMENT_FIELD_ID,
            EdLabels.Label_Comments,
             Evado.Digital.Model.EdFormRecordComment.getCommentMD ( Layout.CommentList, false ) );

          groupField.Layout = EuAdapter.DefaultFieldLayout;
        }

        // 
        // If in edit mode display a new comment field.
        // 
        if ( this._FormAccessRole == Evado.Digital.Model.EdRecord.FormAccessRoles.Record_Author )
        {
          this.LogDebug ( "Add Comment Field" );
          groupField = pageGroup.createFreeTextField (
            EuRecordGenerator.CONST_FORM_COMMENT_FIELD_ID,
            EdLabels.Label_New_Comment,
            String.Empty,
            50,
            5 );

          groupField.Layout = EuAdapter.DefaultFieldLayout;
          groupField.EditAccess = Evado.UniForm.Model.EditAccess.Enabled;

        }//END new comment to be added.

      }//END Display Comments
    }

    //  =================================================================================
    /// <summary>
    /// This method creates a comment footer group
    /// </summary>
    /// <param name="Layout">   Evado.Digital.Model.EdRecord object.</param>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    // ----------------------------------------------------------------------------------
    private void getSignatureFooter (
       Evado.Digital.Model.EdRecord Layout,
      Evado.UniForm.Model.Page PageObject )
    {
      //
      // edit if there are not signatures
      //
      if ( Layout.Signoffs.Count == 0 )
      {
        return;
      }

      // 
      // Initialise local variables.
      // 
      Evado.UniForm.Model.Group pageGroup;
      Evado.UniForm.Model.Field groupField = new Evado.UniForm.Model.Field ( );

      // 
      // Define the comment pageMenuGroup.
      // 
      pageGroup = PageObject.AddGroup (
        String.Empty,
        String.Empty,
        Evado.UniForm.Model.EditAccess.Inherited );
      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;
      pageGroup.GroupType = Evado.UniForm.Model.GroupTypes.Default;

      StringBuilder sbSignoffLog = new StringBuilder ( );

      // 
      // Interate through the signoff objects extracting the signoff content.
      // 
      foreach ( EdUserSignoff signoff in Layout.Signoffs )
      {
        // 
        // If the signoff has a description output it.
        // 
        if ( signoff.SignedOffBy != String.Empty )
        {
          sbSignoffLog.AppendLine ( signoff.Description
          + " " + EdLabels.Label_by + " "
          + signoff.SignedOffBy
           + " " + EdLabels.Label_on + " "
          + signoff.stSignOffDate );

        }//END signoff exists.

      }//END interation loop

      groupField = pageGroup.createReadOnlyTextField (
        "sol_dsp",
        EdLabels.Label_Signoff_Log_Field_Title,
        sbSignoffLog.ToString ( ) );

      groupField.Layout = EuAdapter.DefaultFieldLayout;

    }//END method

    //  =================================================================================
    /// <summary>
    /// This method creates a comment footer group
    /// </summary>
    /// <param name="Layout">   Evado.Digital.Model.EdRecord object.</param>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    // ----------------------------------------------------------------------------------
    private void getApprovalFooter (
       Evado.Digital.Model.EdRecord Layout,
      Evado.UniForm.Model.Page PageObject )
    {
      // 
      // Initialise pageMenuGroup object.
      // 
      Evado.UniForm.Model.Group footerGroup = PageObject.AddGroup (
        String.Empty,
        String.Empty,
        Evado.UniForm.Model.EditAccess.Enabled );
      footerGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;

      // 
      // Add the form record approval as a readonly field.
      // 
      var groupField = footerGroup.createReadOnlyTextField (
         String.Empty,
         String.Empty,
         Layout.Design.Approval );
      groupField.Layout = Evado.UniForm.Model.FieldLayoutCodes.Center_Justified;

    }//END method

    // ***********************************************************************************
    #endregion

    #region private form navigation methods

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates a form categoristion field lists.
    /// 
    /// </summary>
    /// <param name="Form">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="PageGroup">Evado.UniForm.Model.Group object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    /// <remarks>
    /// This method consists of following steps:
    /// 
    /// 1. Bulid category list for specific data types.
    /// 2. To each category in the list, add the field corresponding to that category.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private void getFieldCategories (
       Evado.Digital.Model.EdRecord Form,
      Evado.UniForm.Model.Group PageGroup )
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
      foreach ( Evado.Digital.Model.EdRecordField field in Form.Fields )
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
          foreach ( Evado.Digital.Model.EdRecordField field in Form.Fields )
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

    // ***********************************************************************************
    #endregion

    #region private Field methods

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the form field objects as html markup.
    /// 
    /// </summary>
    /// <param name="Layout">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="PageObject">  Evado.UniForm.Model.Page Object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void createFormSections (
         Evado.Digital.Model.EdRecord Layout,
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "createFormSections" );

      // 
      // Initialise local variables.
      // 
      int fieldCount = 0;
      this._PageId = PageObject.PageId;
      bool bDocumentDisplayMode = false;

      //
      // determine the content should be in display mode.
      //
      if ( ( PageObject.EditAccess == Evado.UniForm.Model.EditAccess.Disabled )
        && ( Layout.State == EdRecordObjectStates.Submitted_Record )
        && ( this._Design.FieldReadonlyDisplayFormat == EdRecord.FieldReadonlyDisplayFormats.Document_Layout
          || this._Design.FieldReadonlyDisplayFormat == EdRecord.FieldReadonlyDisplayFormats.Document_Layout_No_Titles ) )
      {
        bDocumentDisplayMode = true;
      }

      // 
      // Entering the form section iteration loop.
      // 
      foreach ( Evado.Digital.Model.EdRecordSection section in Layout.Design.FormSections )
      {
       // this.LogDebug ( "No: '{0}' Section: '{1}' Role: {2}, User Role:{3} ",
       //   section.No, section.Title, section.ReadAccessRoles, this.Session.UserProfile.Roles );

        if ( section.Title == String.Empty )
        {
          this.LogDebug ( "Empty section" );
          continue;
        }

        if ( section.hasReadAccess ( this.Session.UserProfile.Roles ) == false )
        {
        //  this.LogDebug ( "User {0} does not have access to Section {1}",
        //    this.Session.UserProfile.UserId, section.Title );
          continue;
        }

        if ( bDocumentDisplayMode == true )
        {
          this.creatSectionDocument ( Layout, section, PageObject );
        }
        else
        {
          //
          // generate the section fields
          //
          fieldCount = this.createSectionFormFields ( Layout, section, PageObject );
        }

      }//END Section interation loop.

      //
      // Generate the fields that are not allocated to a section.
      //

      if ( bDocumentDisplayMode == true )
      {
        this.creatSectionDocument ( Layout, new EdRecordSection ( ), PageObject );
      }
      else
      {
        //
        // generate the section fields
        //
        fieldCount = this.createSectionFormFields ( Layout, new EdRecordSection ( ), PageObject );
      }

      this.LogMethodEnd ( "createFormSections" );

    }//END createFormSections method


    //  =================================================================================
    /// <summary>
    ///   This method generates a form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Layout">EdRecord object .</param>
    /// <param name="SectionNo">EdRecordSection object.</param>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    /// <returns>int count of section fields.</returns>
    //  ---------------------------------------------------------------------------------
    private int createSectionFormFields (
       Evado.Digital.Model.EdRecord Layout,
      Evado.Digital.Model.EdRecordSection section,
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "createSectionFormFields" );
      //
      // Initialise the methods variables and objects.
      //
      int count = 0;
      Evado.UniForm.Model.Group pageGroup = new Evado.UniForm.Model.Group ( );

      if ( section.Title != String.Empty )
      {
        pageGroup = new Evado.UniForm.Model.Group (
            section.Title,
            section.Instructions,
            PageObject.EditAccess );
      }
      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;

      //
      // If the section has field name then add the pageMenuGroup hide parameters.
      //
      if ( section.FieldId != String.Empty )
      {
        pageGroup.AddParameter ( Evado.UniForm.Model.GroupParameterList.Hide_Group_If_Field_Id,
          section.FieldId );

        pageGroup.AddParameter ( Evado.UniForm.Model.GroupParameterList.Hide_Group_If_Field_Value,
          section.FieldValue );
      }

      this.LogDebug ( "GroupType:  " + pageGroup.GroupType );

      // 
      // Iterate through each form field in the section.
      // 
      foreach ( Evado.Digital.Model.EdRecordField field in Layout.Fields )
      {
        // 
        // If the field is in the section identified by its section name (backward compatibility) or
        // the section number.
        // 
        if ( field.Design.SectionNo != section.No )
        {
          continue;
        }


        this.createFormField ( field, pageGroup, Layout.State );

        count++;

      }//END field iteration loop.

      //
      // add an edit command if the page is in display mode and the user is the author and 
      // selectable edit has been enabled.
      //
      if ( count > 0
        && Layout.Design.AuthorAccess == EdRecord.AuthorAccessList.Only_Author_Selectable
        && PageObject.EditAccess == Evado.UniForm.Model.EditAccess.Disabled
        && Layout.Author == this.Session.UserProfile.UserId )
      {
        var groupCommand = pageGroup.addCommand (
          EdLabels.Entity_Enable_Edit_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Entities.ToString ( ),
            Evado.UniForm.Model.ApplicationMethods.Custom_Method );
        groupCommand.SetGuid ( this.Session.Entity.Guid );

        groupCommand.AddParameter (
         EuRecordGenerator.CONST_ENABLE_EDIT_FIELD, "Yes" );

        if ( Layout.Design.IsEntity == false )
        {
          groupCommand.Object = EuAdapterClasses.Records.ToString ( );
        }

      }//END command selection block.

      //
      // Add the group if it has content.
      //
      if ( pageGroup.FieldList.Count > 0
        || pageGroup.Description != String.Empty )
      {
        PageObject.AddGroup ( pageGroup );
      }

      this.LogMethodEnd ( "createSectionFormFields" );
      return count;

    }//END createSectionFormFields method

    //  =================================================================================
    /// <summary>
    /// This method creates a document layout containing all text fields and static images.
    /// </summary>
    /// <param name="Layout">EdRecord object .</param>
    /// <param name="SectionNo">EdRecordSection object.</param>
    /// <param name="PageObject">Evado.UniForm.Model.Page object.</param>
    /// <returns>int count of section fields.</returns>
    //  ---------------------------------------------------------------------------------
    private int creatSectionDocument (
       Evado.Digital.Model.EdRecord Layout,
      Evado.Digital.Model.EdRecordSection section,
      Evado.UniForm.Model.Page PageObject )
    {
      this.LogMethod ( "creatSectionDocument" );
      this.LogDebug ( "LayoutId: " + Layout.LayoutId );
      this.LogDebug ( "FieldLayout {0}. ", this._DefaultFieldLayout );
      this.LogDebug ( "HideFieldTitlesWhenReadOnly {0}. ", this._Design.FieldReadonlyDisplayFormat );

      //
      // editable pages are not displayed in document mode.
      //
      if ( PageObject.EditAccess == Evado.UniForm.Model.EditAccess.Enabled )
      {
        return 0;
      }

      //
      // If the display form is not document types exit.
      //
      if ( this._Design.FieldReadonlyDisplayFormat != EdRecord.FieldReadonlyDisplayFormats.Document_Layout
        && this._Design.FieldReadonlyDisplayFormat != EdRecord.FieldReadonlyDisplayFormats.Document_Layout_No_Titles )
      {
        return 0;
      }

      //
      // Initialise the methods variables and objects.
      //
      StringBuilder sbHtmlContent = new StringBuilder ( );
      int count = 0;
      bool displayTitle = true;

      //
      // If the title is to be hidden then delete the uniform field values.
      //
      if ( this._Design.FieldReadonlyDisplayFormat == EdRecord.FieldReadonlyDisplayFormats.Document_Layout_No_Titles )
      {
        displayTitle = false;
      }

      //
      // create the field group object.
      //
      Evado.UniForm.Model.Group pageGroup = new Evado.UniForm.Model.Group ( );

      if ( section.Title != String.Empty )
      {
        pageGroup.Title = section.Title;
      }
      pageGroup.Layout = Evado.UniForm.Model.GroupLayouts.Full_Width;

      // 
      // Iterate through each form field in the section.
      // 
      foreach ( Evado.Digital.Model.EdRecordField Field in Layout.Fields )
      {
        if ( Field.Design.SectionNo != section.No )
        {
          continue;
        }

        // 
        // If the not valid sex rules match the FirstSubject's sex then
        // only display the field.
        // 
        if ( Field.Design.HideField == true )
        {
          this.LogDebug ( "Hidden field." );
          continue;
        }

        this.LogDebug ( "FieldId {0}, TypeId {1} ", Field.FieldId, Field.TypeId );

        // 
        // Select the method to generate the correct field type.
        // 
        switch ( Field.TypeId )
        {
          case Evado.Model.EvDataTypes.Computed_Field:
          case Evado.Model.EvDataTypes.Text:
          case Evado.Model.EvDataTypes.Boolean:
          case Evado.Model.EvDataTypes.Yes_No:
          case Evado.Model.EvDataTypes.Numeric:
          case Evado.Model.EvDataTypes.Integer:
          case Evado.Model.EvDataTypes.Date:
          case Evado.Model.EvDataTypes.Time:
          case Evado.Model.EvDataTypes.Telephone_Number:
          case Evado.Model.EvDataTypes.Email_Address:
          case Evado.Model.EvDataTypes.Read_Only_Text:
          case Evado.Model.EvDataTypes.Analogue_Scale:
          case Evado.Model.EvDataTypes.Special_Subsitute_Data:
            {
              this.getDisplaySingleValueField ( Field, sbHtmlContent, displayTitle );
              break;
            }

          case Evado.Model.EvDataTypes.Free_Text:
            {
              this.getDisplayFreeTextField ( Field, sbHtmlContent, displayTitle );
              break;
            }
          case Evado.Model.EvDataTypes.Integer_Range:
          case Evado.Model.EvDataTypes.Float_Range:
          case Evado.Model.EvDataTypes.Date_Range:
            {
              this.getDisplayTwoValueField ( Field, sbHtmlContent, displayTitle );
              break;
            }
          case Evado.Model.EvDataTypes.Http_Link:
            {
              this.getDisplayHttpValueField ( Field, sbHtmlContent, displayTitle );
              break;
            }
          case Evado.Model.EvDataTypes.Streamed_Video:
            {
              this.getDisplayStreamedVideoField ( Field, sbHtmlContent, displayTitle );
              break;
            }
          case Evado.Model.EvDataTypes.Address:
            {
              this.getDisplayAddressField ( Field, sbHtmlContent, displayTitle );
              break;
            }
          case Evado.Model.EvDataTypes.Name:
            {
              this.getDisplayNameField ( Field, sbHtmlContent, displayTitle );
              break;
            }
          case Evado.Model.EvDataTypes.Check_Box_List:
          case Evado.Model.EvDataTypes.Selection_List:
          case Evado.Model.EvDataTypes.Radio_Button_List:
          case Evado.Model.EvDataTypes.Horizontal_Radio_Buttons:
          case Evado.Model.EvDataTypes.External_Selection_List:
          case Evado.Model.EvDataTypes.External_RadioButton_List:
          case Evado.Model.EvDataTypes.External_CheckBox_List:
            {
              this.getDisplaySelectionField ( Field, sbHtmlContent, displayTitle );
              break;
            }
          case Evado.Model.EvDataTypes.Image:
          case Evado.Model.EvDataTypes.External_Image:
            {
              this.getDisplayImageField ( Field, sbHtmlContent, displayTitle );
              break;
            }
          default:
            {
              this.getDisplaySingleValueField ( Field, sbHtmlContent, displayTitle );
              break;
            }
        }//END switch statement

      }//END iteration loop

      this.LogDebug ( "Html Page. " + sbHtmlContent.ToString ( ) );

      //groupField.Value = sbHtmlContent.ToString ( );
      pageGroup.Description = sbHtmlContent.ToString ( );

      //
      // Add the group if it has content.
      //
      if ( pageGroup.FieldList.Count > 0
        || pageGroup.Description != String.Empty )
      {
        PageObject.AddGroup ( pageGroup );
      }

      this.LogMethodEnd ( "creatSectionDocument" );
      return count;

    }//END creatSectionDocument method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="StringBuilder">Html content string builder.</param>
    /// <param name="displayTitle">Bool: True = display the title..</param>
    //  ---------------------------------------------------------------------------------
    private void getDisplaySingleValueField (
      Evado.Digital.Model.EdRecordField Field,
      StringBuilder sbHtmlContent,
      bool displayTitle )
    {
      this.LogMethod ( "getDisplaySingleValueField" );
      this.LogDebug ( "Title {0} V: {1}", Field.Title, Field.ItemValue );
      String title = Field.Title.ToLower ( );

      if ( Field.ItemValue == String.Empty 
        || Field.ItemValue.Contains ( "E+37" ) == true
        || Field.ItemValue.Contains ( "E-35" ) == true )  
      {
        return;
      }

      this.LogDebug ( "Value {0}. ", Field.ItemValue );

      if ( displayTitle == true
        && title.Contains ( "title" ) == false
        && title.Contains ( "name" ) == false )
      {
        sbHtmlContent.AppendFormat ( EdLabels.Generator_Disp_Field_Title_Value_Format, Field.Title, Field.ItemValue );
        return;
      }
      if ( title.Contains ( "title" ) == true
        || title.Contains ( "name" ) == true )
      {
        sbHtmlContent.AppendFormat ( EdLabels.Generator_Disp_Field_Value_Format2, Field.ItemValue );
        return;
      }
      sbHtmlContent.AppendFormat ( EdLabels.Generator_Disp_Field_Value_Format, Field.ItemValue );
      return;

    }//END getDisplaySingleValueField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="StringBuilder">Html content string builder.</param>
    /// <param name="displayTitle">Bool: True = display the title..</param>
    //  ---------------------------------------------------------------------------------
    private void getDisplayTwoValueField (
      Evado.Digital.Model.EdRecordField Field,
      StringBuilder sbHtmlContent,
      bool displayTitle )
    {
      this.LogMethod ( "getDisplayTwoValueField" );
      string [ ] arValue = Field.ItemValue.Split ( ';' );

      if ( arValue.Length == 0 )
      {
        return;
      }

      if ( arValue.Length == 1 )
      {
        this.getDisplaySingleValueField ( Field, sbHtmlContent, displayTitle );
        return;
      }

      sbHtmlContent.AppendFormat ( EdLabels.Generator_Disp_Field_Title_2Value_Format, Field.Title, arValue [ 0 ], arValue [ 1 ] );

      return;

    }//END getDisplayTwoValueField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="StringBuilder">Html content string builder.</param>
    /// <param name="displayTitle">Bool: True = display the title..</param>
    //  ---------------------------------------------------------------------------------
    private void getDisplayHttpValueField (
      Evado.Digital.Model.EdRecordField Field,
      StringBuilder sbHtmlContent,
      bool displayTitle )
    {
      this.LogMethod ( "getDisplayHttpValueField" );
      this.LogDebug ( "Title {0} V: {1}", Field.Title, Field.ItemValue );
      String title = Field.Title;
      String url = Field.ItemValue;
      String urlTitle = Field.ItemValue;

      if ( Field.ItemValue == String.Empty )
      {
        return;
      }
      this.LogDebug ( "Value {0}. ", url );

      if ( Field.ItemValue.Contains ( "^" ) == true )
      {
        string [ ] arValue = Field.ItemValue.Split ( '^' );

        url = arValue [ 0 ];
        urlTitle = arValue [ 1 ];
      }

      String format = "[[table]]\r\n"
          + "[[tr]][[td]][[strong]] {0}[[/strong]] [[/td]] [[td]]\r\n"
          + "[[a href='{1}' target='_blank' ]] {2}[[/a]] \r\n"
          + "[[/td]] [[/tr]]\r\n"
          + "[[/table]]";

      this.LogDebug ( "Url {0}, UrlTitle {1}, field title {2}. ", url, urlTitle, title );

      sbHtmlContent.AppendFormat ( format, title, url, urlTitle );

      // EdLabels.Generator_Disp_Field_Title_2Value_Format
      // EdLabels.Generator_Disp_Html_Field_Value_Format

      this.LogMethodEnd ( "getDisplayHttpValueField" );

    }//END getDisplaySingleValueField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="StringBuilder">Html content string builder.</param>
    /// <param name="displayTitle">Bool: True = display the title..</param>
    //  ---------------------------------------------------------------------------------
    private void getDisplayAddressField (
      Evado.Digital.Model.EdRecordField Field,
      StringBuilder sbHtmlContent,
      bool displayTitle )
    {
      this.LogMethod ( "getDisplayAddressField" );
      string [ ] arValue = Field.ItemValue.Split ( ';' );

      if ( arValue.Length == 0 )
      {
        return;
      }

      if ( arValue.Length < 6 )
      {
        return;
      }

      sbHtmlContent.AppendFormat ( EdLabels.Generator_Disp_Field_Title_Address_Format, Field.Title,
        arValue [ 0 ], arValue [ 1 ], arValue [ 2 ], arValue [ 3 ], arValue [ 4 ], arValue [ 5 ] );

      return;

    }//END getDisplayAddressField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the name user markup
    /// 
    /// </summary>
    /// <param name="Field">Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="StringBuilder">Html content string builder.</param>
    /// <param name="displayTitle">Bool: True = display the title..</param>
    //  ---------------------------------------------------------------------------------
    private void getDisplayNameField (
      Evado.Digital.Model.EdRecordField Field,
      StringBuilder sbHtmlContent,
      bool displayTitle )
    {
      this.LogMethod ( "getDisplayNameField" );
      string [ ] arValue = Field.ItemValue.Split ( ';' );

      if ( arValue.Length == 0 )
      {
        return;
      }

      if ( arValue.Length < 4 )
      {
        return;
      }

      sbHtmlContent.AppendFormat ( EdLabels.Generator_Disp_Field_Title_Name_Format, Field.Title,
        arValue [ 0 ], arValue [ 1 ], arValue [ 2 ], arValue [ 3 ] );

      return;

    }//END getDisplayNameField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the selection list user markup
    /// 
    /// </summary>
    /// <param name="Field">Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="StringBuilder">Html content string builder.</param>
    /// <param name="displayTitle">Bool: True = display the title..</param>
    //  ---------------------------------------------------------------------------------
    private void getDisplaySelectionField (
      Evado.Digital.Model.EdRecordField Field,
      StringBuilder sbHtmlContent,
      bool displayTitle )
    {
      this.LogMethod ( "getDisplaySelectionField" );
      this.LogDebug ( "id {0}, OptionList.Count {1}", Field.FieldId, Field.Design.OptionList.Count );
      //
      // initialise the methods variables and objects.
      //
      List<EvOption> optionList = Field.Design.OptionList;
      string [ ] arValue = Field.ItemValue.Split ( ';' );
      String value = String.Empty;

      if ( arValue.Length == 0 )
      {
        return;
      }

      //
      // search selection list if the list is external
      //
      if ( Field.TypeId == EvDataTypes.External_CheckBox_List
        || Field.TypeId == EvDataTypes.External_RadioButton_List
        || Field.TypeId == EvDataTypes.External_Selection_List )
      {
        optionList = this.AdapterObjects.getSelectionOptions (
          Field.Design.ExSelectionListId, String.Empty, false, false );
      }

      if ( arValue.Length > 1 )
      {
        value += "[[ul]]";

        foreach ( string str in arValue )
        {
          foreach ( EvOption option in optionList )
          {
            if ( option.Value == str )
            {
              value += "[[li]]" + option.Description + "[[/li]]";
            }
          }//Iterate through the options 
        }//Iterate through the values.

        value += "[[/ul]]";
      }
      else
      {
        foreach ( string str in arValue )
        {
          foreach ( EvOption option in optionList )
          {
            if ( option.Value == str )
            {
              if ( value != String.Empty )
              {
                value += "[[/br]]";
              }
              value += option.Description;
            }
          }//Iterate through the options 
        }//Iterate through the values.
      }

      sbHtmlContent.AppendFormat ( EdLabels.Generator_Disp_Field_Title_Value_Format + "\r\n", Field.Title, value );

      return;

    }//END getDisplaySelectionField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="StringBuilder">Html content string builder.</param>
    /// <param name="displayTitle">Bool: True = display the title..</param>
    //  ---------------------------------------------------------------------------------
    private void getDisplayFreeTextField (
      Evado.Digital.Model.EdRecordField Field,
      StringBuilder sbHtmlContent,
      bool displayTitle )
    {
      this.LogMethod ( "getDisplayFreeTextField" );

      if ( Field.ItemText == String.Empty )
      {
        return;
      }

      String html =  encodeMarkDown ( Field.ItemText );

      html = html.Replace ( "<", "[[" );
      html = html.Replace ( ">", "]]" );

      if ( displayTitle == true )
      {
        sbHtmlContent.AppendFormat (
          EdLabels.Generator_Disp_Field_Title_FreeText_Format,
          Field.Title,
          html );
        return;
      }
      sbHtmlContent.Append ( html );

      return;

    }//END getDisplayFreeTextField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the image field.
    /// 
    /// </summary>
    /// <param name="Field">Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="StringBuilder">Html content string builder.</param>
    /// <param name="displayTitle">Bool: True = display the title..</param>
    //  ---------------------------------------------------------------------------------
    private void getDisplayImageField (
      Evado.Digital.Model.EdRecordField Field,
      StringBuilder sbHtmlContent,
      bool displayTitle )
    {
      this.LogMethod ( "getDisplayImageField" );

      if ( Field.ItemValue.Length == 0 )
      {
        return;
      }

      string stUrl = this.UniForm_ImageServiceUrl + Field.ItemValue;

      String htmlMarkup = "[[p style='text-align: center;']][[a href='" + stUrl + "' target='_blank' ]] \r\n"
          + "[[img alt='Image " + Field.ItemValue + "' " + "src='" + stUrl + "'/]] [[/a]][[/p]]";

      sbHtmlContent.AppendFormat ( htmlMarkup, Field.Title, Field.ItemText );

      return;

    }//END getDisplayImageField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the image field.
    /// 
    /// </summary>
    /// <param name="Field">Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="StringBuilder">Html content string builder.</param>
    /// <param name="displayTitle">Bool: True = display the title..</param>
    //  ---------------------------------------------------------------------------------
    private void getDisplayStreamedVideoField (
      Evado.Digital.Model.EdRecordField Field,
      StringBuilder sbHtmlContent,
      bool displayTitle )
    {
      this.LogMethod ( "getDisplayStreamedVideoField" );

      if ( Field.ItemValue == String.Empty )
      {
        return;
      }

      string stUrl = this.UniForm_ImageServiceUrl + Field.ItemValue;

      String htmlMarkup = this.getVideoIFrame (Field) ;

      sbHtmlContent.AppendFormat ( htmlMarkup, Field.Title, Field.ItemText );

      this.LogMethodEnd ( "getDisplayStreamedVideoField" );

    }//END getDisplayImageField method.


    // ===================================================================================
    /// <summary>
    /// This method creates a iframe containing a streamed video object
    /// </summary>
    /// <param name="Field">Field object.</param>
    /// <returns>String html</returns>
    // ----------------------------------------------------------------------------------
    private String getVideoIFrame (
     Evado.Digital.Model.EdRecordField Field )
    {
      this.LogMethod ( "getVideoIFrame" );
      //
      // Initialise the methods variables and objects.
      //
      StringBuilder sbHtml = new StringBuilder ( );
      string value = Field.ItemValue.ToLower ( );
      int width = Field.RecordMedia.Width;
      int height = Field.RecordMedia.Height ;
      String stVideoStreamParameters = String.Empty;
      String stVideoSource = String.Empty;

      //
      // data or parameters are missing exit. 
      if ( value == String.Empty
        || this.AdapterObjects.Settings.VimeoEmbeddedUrl == String.Empty
        || this.AdapterObjects.Settings.YouTubeEmbeddedUrl == String.Empty )
      {
        return String.Empty;
      }

      //
      // Set default width
      //
      if ( width == 0
        && height == 0 )
      {
        width = 450;
        height = width * 10 / 17;
      }
      else
      {
        if ( height == 0 )
        {
          height = width * 10 / 17;
        }
        else
        {
          width = height * 17 / 10;
        }
      }


      if ( Field.ItemValue.Contains ( "vimeo.com" ) == true )
      {
        int index = Field.ItemValue.LastIndexOf ( '/' );
        value = Field.ItemValue.Substring ( ( index + 1 ) );

        stVideoSource = this.AdapterObjects.Settings.VimeoEmbeddedUrl + value;

        stVideoStreamParameters = "frameborder=\"0\" webkitAllowFullScreen mozallowfullscreen allowFullScreen ";
      }

      if ( Field.ItemValue.Contains ( "youtube" ) == true
        || Field.ItemValue.Contains ( "youtu.be" ) == true )
      {
        int index = Field.ItemValue.LastIndexOf ( '/' );

        value = Field.ItemValue.Substring ( ( index + 1 ) );

        value = value.Replace ( "watch?v=", "" );

        stVideoSource = this.AdapterObjects.Settings.YouTubeEmbeddedUrl + value;

        stVideoStreamParameters = "frameborder=\"0\" allow=\"accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen";
      }

      this.LogValue ( "Video ID: " + value );
      this.LogValue ( "VideoSource: " + stVideoSource );

      sbHtml.AppendLine ( "[[iframe "
        + "id='" + Field.FieldId + "' "
        + "name='" + Field.FieldId + "' "
        + "src='" + stVideoSource + "' " );

      if ( width > 0 )
      {
        sbHtml.AppendLine ( "width='" + width + "' " );
      }
      if ( height > 0 )
      {
        sbHtml.AppendLine ( "height='" + height + "' " );
      }

      sbHtml.AppendLine ( stVideoStreamParameters
       + " style=' display: block; margin-left: auto; margin-right: auto' ]]"
       + "[[/iframe]]" );

      this.LogMethodEnd ( "getVideoIFrame" );
      return sbHtml.ToString ( );
    }//END getVideoIFrame Method

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates a form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">EvSubject object containing the FirstSubject and form to be generated.</param>
    /// <param name="FieldGroup">Evado.UniForm.Model.Group object.</param>
    /// <param name="FormState">  Evado.Digital.Model.EvFormObjectStates enumerated value.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void createFormField (
       Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Group FieldGroup,
      Evado.Digital.Model.EdRecordObjectStates FormState )
    {
      this.LogMethod ( "getFormField" );
      this.LogDebug ( "FieldId: " + Field.FieldId );
      this.LogDebug ( "TypeId: " + Field.TypeId );
      this.LogDebug ( "FieldLayout {0}. ", this._DefaultFieldLayout );
      this.LogDebug ( "FormAccessRole {0}. ", this._FormAccessRole );
      this.LogDebug ( "HideFieldTitlesWhenReadOnly {0}. ", this._Design.FieldReadonlyDisplayFormat );
      this.LogDebug ( "Field.Design.FieldLayout {0}. ", Field.Design.FieldLayout );
      this.LogDebug ( "this._FieldLayout {0}. ", this._DefaultFieldLayout );

      // 
      // If the not valid sex rules match the FirstSubject's sex then
      // only display the field.
      // 
      if ( Field.Design.HideField == true )
      {
        this.LogDebug ( "Hidden field." );
        return;
      }

      //
      // set the field layout setting.
      //
      Evado.UniForm.Model.FieldLayoutCodes layout = this._DefaultFieldLayout;
      Evado.UniForm.Model.FieldLayoutCodes Fieldlayout =
        EvStatics.parseEnumValue<Evado.UniForm.Model.FieldLayoutCodes> ( Field.Design.FieldLayout );

      //
      // set to the field settings.
      //
      if ( Fieldlayout != layout )
      {
        layout = Fieldlayout;
      }

      //
      // IF the field had static readonly field display them across the entire page.
      //
      if ( Field.TypeId == EvDataTypes.Read_Only_Text
        || Field.TypeId == EvDataTypes.External_Image
        || Field.TypeId == EvDataTypes.Streamed_Video
        || Field.TypeId == EvDataTypes.Html_Content )
      {
        layout = Evado.UniForm.Model.FieldLayoutCodes.Column_Layout;
      }


      // 
      // Initialise the UniFORm field object.
      // 
      Evado.UniForm.Model.Field groupField = FieldGroup.addField (
        Field.FieldId,
        Field.Title,
        Evado.Model.EvDataTypes.Text,
        Field.ItemValue );

      groupField.Layout = layout;

      this.LogDebug ( "groupField.Layout {0}. ", groupField.Layout );
      this.LogDebug ( "groupField.EditAccess {0}. ", groupField.EditAccess );
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
      // If the title is to be hidden then delete the uniform field values.
      //
      if ( groupField.EditAccess != Evado.UniForm.Model.EditAccess.Enabled )
      {
        switch ( this._Design.FieldReadonlyDisplayFormat )
        {
          case EdRecord.FieldReadonlyDisplayFormats.Hide_Field_Titles:
            {
              groupField.Title = String.Empty;
              groupField.Description = String.Empty;
              groupField.Mandatory = false;
              break;
            }

        }//END switch statment
      }//END if statement

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
        case Evado.Model.EvDataTypes.Date_Range:
          {
            this.getDateRangeField ( Field, groupField );
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
        case Evado.Model.EvDataTypes.Telephone_Number:
          {
            this.getTelephoneField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Email_Address:
          {
            this.getEmailAddressField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Address:
          {
            this.getAddressField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Name:
          {
            this.getNameField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Selection_List:
          {
            this.getSelectonField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.External_Selection_List:
          {
            this.getExternalSelectonField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Radio_Button_List:
          {
            this.getRadioButtonListField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.External_RadioButton_List:
          {
            this.getExternalRadioButtonField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Check_Box_List:
          {
            this.getCheckButtonListField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.External_CheckBox_List:
          {
            this.getExternalCheckBoxField ( Field, groupField );
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
        case Evado.Model.EvDataTypes.Image:
          {
            this.getImageField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.External_Image:
          {
            this.getExternalImageField ( Field, groupField );
            return;
          }
        case Evado.Model.EvDataTypes.Http_Link:
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
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    /// <param name="ViewState">  Evado.Digital.Model.EvForm.FormDisplayStates enumerated value.</param>
    //  ---------------------------------------------------------------------------------
    private void createFormFieldHeader (
       Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "createFormFieldHeader" );
      // 
      // Initialise local varibles
      // 
      StringBuilder sbDescription = new StringBuilder ( );

      //
      // set the field to reaonly if the field state is not edit or data cleansing.
      //
      if ( this._FormAccessRole == Evado.Digital.Model.EdRecord.FormAccessRoles.Record_Author )
      {
        GroupField.EditAccess = Evado.UniForm.Model.EditAccess.Enabled;
      }

      // 
      // design header information
      // 
      if ( this._FormAccessRole == Evado.Digital.Model.EdRecord.FormAccessRoles.Record_Author
        && ( this._FormState == EdRecordObjectStates.Draft_Record
          || this._FormState == EdRecordObjectStates.Form_Reviewed ) )
      {
        GroupField.EditAccess = Evado.UniForm.Model.EditAccess.Enabled;
        sbDescription.Append (
           "Form field identifier: " + Field.FieldId );
        if ( Field.Design.IsSummaryField == true )
        {
          sbDescription.Append ( ", SF" );
        }
        if ( Field.Design.Mandatory == true )
        {
          sbDescription.Append ( ", MF" );
        }
        if ( Field.Design.AiDataPoint == true )
        {
          sbDescription.Append ( ", AI-DP" );
        }
        if ( Field.Design.HideField == true )
        {
          sbDescription.Append ( ", HF" );
        }
        if ( Field.TypeId == EvDataTypes.Numeric
          || Field.TypeId == EvDataTypes.Integer_Range
          || Field.TypeId == EvDataTypes.Float_Range
          || Field.TypeId == EvDataTypes.Double_Range )
        {
          sbDescription.AppendLine ( "NV-VR: " + Field.Design.ValidationLowerLimit
             + " - " + Field.Design.ValidationUpperLimit
             + ", NV-AR: " + Field.Design.AlertLowerLimit
             + " - " + Field.Design.AlertUpperLimit );
        }
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
           + "\r\nValidation Error: " + Field.ValidationError );
      }

      if ( Field.Design.Instructions != String.Empty
        && Field.TypeId != Evado.Model.EvDataTypes.Read_Only_Text )
      {
        sbDescription.AppendLine ( Field.Design.Instructions );
      }

      GroupField.Description = sbDescription.ToString ( );

      this.LogMethodEnd ( "createFormFieldHeader" );

    }//END getFormFieldHeader

    //  =================================================================================
    /// <summary>
    ///   This method generates the text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getDisplayField (
        Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getDisplayField" );

      // 
      // Initialise local variables.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Read_Only_Text;
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, 50 );
      GroupField.Description = String.Empty;

      return;

    }//END getReadOnlyField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getTextField (
        Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getTextField method." );

      // 
      // Initialise local variables.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Text;
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width,
        Field.Design.FieldWidth );


      return;

    }//END getTextField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the free text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getFreeTextField (
        Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getFreeTextField" );
      // 
      // Set the field parameters
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Free_Text;
      GroupField.Value = Field.ItemText;
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width,
        Field.Design.FieldWidth );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Height,
        Field.Design.FieldHeight );

      if ( GroupField.EditAccess != Evado.UniForm.Model.EditAccess.Enabled )
      {
        GroupField.Type = Evado.Model.EvDataTypes.Read_Only_Text;
      }

    }//END getFreeTextField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getReadOnlyField (
        Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getReadOnlyField" );

      this.LogDebug ( "Instructions: " + Field.Design.Instructions );
      // 
      // Initialise local variables.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Read_Only_Text;
      GroupField.Value = Field.Design.Instructions;
      GroupField.Layout = Evado.UniForm.Model.FieldLayoutCodes.Column_Layout;
      GroupField.Description = String.Empty;


      //this.LogDebugValue ( "Value: " + GroupField.Value );

      return;

    }//END getReadOnlyField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the streamed video form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvFormField object containing the 
    /// field to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getStreamedVideoField (
        Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getStreamedVideoField" );

      if ( Field.RecordMedia == null )
      {
        Field.RecordMedia = new EdRecordMedia ( );
      }
      this.LogDebug ( "Url: " + Field.RecordMedia.Url );
      this.LogDebug ( "Title: " + Field.RecordMedia.Title );
      this.LogDebug ( "Width: " + Field.RecordMedia.Width );
      this.LogDebug ( "Height: " + Field.RecordMedia.Height );

      // 
      // Initialise local variables.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Streamed_Video;
      GroupField.Value = Field.ItemValue;
      GroupField.Description = Field.Design.Instructions;

      //
      // if the field value is empty load the media URL.
      //
      if ( GroupField.Value == String.Empty )
      {
        GroupField.Value = Field.RecordMedia.Url;
      }

      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, Field.RecordMedia.Width.ToString ( ) );

      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Height, Field.RecordMedia.Height.ToString ( ) );

      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Value_Label, Field.RecordMedia.Title );

      this.LogDebug ( "Value: " + GroupField.Value );

      return;

    }//END getStreamedVideoField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the external image form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">Evado.Digital.Model.EvForm object containing the form 
    /// to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getImageField (
        Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getImageField" );
      this.LogDebug ( "Field.ItemValue {0}.", Field.ItemValue );
      //
      // initialise the methods variables and objects.
      //
      GroupField.Type = Evado.Model.EvDataTypes.Image;
      GroupField.Value = Field.ItemValue;
      GroupField.Description = String.Empty;

      //
      // define the source and target directory paths.
      //
      if ( Field.ItemValue != String.Empty )
      {
        String stTargetPath = this.UniForm_BinaryFilePath + Field.ItemValue;
        String stImagePath = this.UniForm_ImageFilePath + Field.ItemValue;

        this.LogDebug ( "Target path {0}.", stTargetPath );
        this.LogDebug ( "Image path {0}.", stImagePath );

        //
        // copy the file into the image directory.
        //
        System.IO.File.Copy ( stImagePath, stTargetPath, true );
      }

      this.LogDebug ( "Width: " + Field.RecordMedia.Width );
      this.LogDebug ( "Height: " + Field.RecordMedia.Height );

      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width,
      Field.RecordMedia.Width.ToString ( ) );

      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Height,
      Field.RecordMedia.Height.ToString ( ) );

      this.LogDebug ( "Value: " + GroupField.Value );

      return;

    }//END getImageField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the external image form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getExternalImageField (
        Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getExternalImageField" );

      this.LogDebug ( "Instructions: " + Field.Design.Instructions );
      // 
      // Initialise local variables.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.External_Image;
      GroupField.Value = Field.Design.Instructions;
      GroupField.Description = String.Empty;

      this.LogDebug ( "JavaScript: " + Field.Design.JavaScript );

      this.LogDebug ( "Width: " + Field.RecordMedia.Width );
      this.LogDebug ( "Height: " + Field.RecordMedia.Height );

      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width,
      Field.RecordMedia.Width.ToString ( ) );

      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Height,
      Field.RecordMedia.Height.ToString ( ) );

      this.LogDebug ( "Value: " + GroupField.Value );

      return;

    }//END getExternalImageField method.

    //  =================================================================================
    /// <summary>
    ///   This method generates the external image form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getHttpLinkField (
        Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getHttpLinkField" );

      this.LogDebug ( "Instructions: " + Field.Design.Instructions );
      this.LogDebug ( "Value: " + GroupField.Value );
      // 
      // Initialise local variables.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Http_Link;
      GroupField.Description = Field.Design.Instructions;

      return;

    }//END getHttpLinkField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the computed form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getComputedField (
        Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( ".getComputedField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Computed_Field;
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, 20 );

      return;

    }//END getComputedField method.

    // =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the numeric form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getNumericField (
       Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getNumericField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Numeric;

      GroupField.Value = Field.ItemValue;
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, 12 );

      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Min_Value, Field.Design.ValidationLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Max_Value, Field.Design.ValidationUpperLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Min_Alert, Field.Design.AlertLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Max_Alert, Field.Design.AlertUpperLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Min_Normal, Field.Design.NormalRangeLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Max_Normal, Field.Design.NormalRangeUpperLimit.ToString ( ) );
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Validation, Evado.UniForm.Model.Background_Colours.Red );
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Alert, Evado.UniForm.Model.Background_Colours.Orange );
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Normal, Evado.UniForm.Model.Background_Colours.Yellow );

      // 
      // Add the field unit if it exists.
      // 
      if ( Field.Design.UnitHtml != String.Empty )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Unit, Field.Design.UnitHtml );
      }

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }
      if ( Field.TypeId == Evado.Model.EvDataTypes.Integer )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Form.onIntegerValidation" );
      }

    }//END getNumericField method.


    // =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the numeric form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getIntegerRangeField (
       Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getIntegerRangeField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Integer_Range;
      GroupField.Value = Field.ItemValue;
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, 12 );

      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Min_Value, Field.Design.ValidationLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Max_Value, Field.Design.ValidationUpperLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Min_Alert, Field.Design.AlertLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Max_Alert, Field.Design.AlertUpperLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Min_Normal, Field.Design.NormalRangeLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Max_Normal, Field.Design.NormalRangeUpperLimit.ToString ( ) );
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Validation, Evado.UniForm.Model.Background_Colours.Red );
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Alert, Evado.UniForm.Model.Background_Colours.Orange );
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Normal, Evado.UniForm.Model.Background_Colours.Yellow );

      // 
      // Add the field unit if it exists.
      // 
      if ( Field.Design.UnitHtml != String.Empty )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Unit, Field.Design.UnitHtml );
      }

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }
      if ( Field.TypeId == Evado.Model.EvDataTypes.Integer )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Form.onIntegerValidation" );
      }

    }//END getIntegerRangeField method.

    // =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the numeric form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getFloatRangeField (
       Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getFloatRangeField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Float_Range;
      GroupField.Value = Field.ItemValue;
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, 12 );

      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Min_Value, Field.Design.ValidationLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Max_Value, Field.Design.ValidationUpperLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Min_Alert, Field.Design.AlertLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Max_Alert, Field.Design.AlertUpperLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Min_Normal, Field.Design.NormalRangeLowerLimit.ToString ( ) );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Max_Normal, Field.Design.NormalRangeUpperLimit.ToString ( ) );
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Validation, Evado.UniForm.Model.Background_Colours.Red );
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Alert, Evado.UniForm.Model.Background_Colours.Orange );
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Normal, Evado.UniForm.Model.Background_Colours.Yellow );

      // 
      // Add the field unit if it exists.
      // 
      if ( Field.Design.UnitHtml != String.Empty )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Unit, Field.Design.UnitHtml );
      }

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }
      if ( Field.TypeId == Evado.Model.EvDataTypes.Integer )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Form.onIntegerValidation" );
      }

    }//END getFloatRangeField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the date form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getDateField ( Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getDateField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Date;
      GroupField.Value = Field.ItemValue;

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getDateField method.

    // =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the numeric form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getDateRangeField (
       Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getFloatRangeField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Date_Range;
      GroupField.Value = Field.ItemValue;

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }
      if ( Field.TypeId == Evado.Model.EvDataTypes.Integer )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Form.onIntegerValidation" );
      }

    }//END getFloatRangeField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the time form  field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getTimeField ( Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getTimeField" );

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
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getTimeField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the telephone form  field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getTelephoneField ( Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getTelephoneField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Telephone_Number;
      GroupField.Value = Field.ItemValue;

    }//END getTelephoneField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the telephone field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getEmailAddressField ( Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getEmailAddressField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Email_Address;
      GroupField.Value = Field.ItemValue;

    }//END getEmailAddressField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the address field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getAddressField ( Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getAddressField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Address;
      GroupField.Value = Field.ItemValue;

    }//END getEmailAddressField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the Name field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getNameField ( Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getNameField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Name;
      GroupField.Value = Field.ItemValue;

    }//END getEmailAddressField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the Yes No form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getYesNoField (
        Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getYesNoField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Boolean;
      GroupField.Value = Field.ItemValue;
      GroupField.Mandatory = Field.Design.Mandatory;
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Mandatory, Evado.UniForm.Model.Background_Colours.Red );

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

      this.LogMethodEnd ( "getYesNoField" );
    }//END getYesNoField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    /// <param name="FormState">Evado.Digital.Model.EvFormObjectStates enumeration object.</param>
    //  ---------------------------------------------------------------------------------
    private void getSignatureField (
        Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField,
      Evado.Digital.Model.EdRecordObjectStates FormState )
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
        GroupField.Value = EdLabels.Signature_Field_Status_No_Text;
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, "50" );
        GroupField.EditAccess = Evado.UniForm.Model.EditAccess.Disabled;

        if ( Field.ItemText != String.Empty )
        {
          GroupField.Value = EdLabels.Signature_Field_Status_Yes_Text;
        }

        this.LogDebug ( "GroupField.Type: " + GroupField.Type );
        this.LogDebug ( "GroupField.Value: " + GroupField.Value );

        this.LogMethodEnd ( "getSignatureField" );
        return;
      }//END Hide signature field.

      GroupField.Type = Evado.Model.EvDataTypes.Signature;
      GroupField.Value = Field.ItemText;
      GroupField.EditAccess = Evado.UniForm.Model.EditAccess.Enabled;
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Width, "600" );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Height, "200" );
      //
      // the state is not empty or draft and a signature exists it is readonly.
      //
      if ( Field.ItemText != String.Empty
        && FormState != EdRecordObjectStates.Empty_Record
        && FormState != EdRecordObjectStates.Draft_Record
        && FormState != EdRecordObjectStates.Completed_Record )
      {
        GroupField.EditAccess = Evado.UniForm.Model.EditAccess.Disabled;
      }

      this.LogDebug ( "GroupField.Value: " + GroupField.Value );
      this.LogDebug ( "GroupField.EditAccess: " + GroupField.EditAccess );
      this.LogMethodEnd ( "getSignatureField" );

    }//END getTextField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the Yes No form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getQueryYesNoField (
        Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getQueryYesNoField" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Boolean;
      GroupField.Value = Field.ItemValue;
      GroupField.Mandatory = Field.Design.Mandatory;
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Mandatory, Evado.UniForm.Model.Background_Colours.Red );

      GroupField.SetValueColumnWidth ( Evado.UniForm.Model.FieldValueWidths.Twenty_Percent );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Field_Value_Legend, EdLabels.FormField_YesNo_Query_Legend );

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getQueryYesNoField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the radio button list form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getRadioButtonListField (
        Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getRadioButtonoField method. Value: " + Field.ItemValue );
      this.LogDebug ( "Options: " + Field.Design.Options );

      // 
      // Initialise the methods object and variables.
      // 
      List<Evado.Model.EvOption> optionlist = Evado.UniForm.Model.EuStatics.getStringAsOptionList (
        Field.Design.Options );

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
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Mandatory, Evado.UniForm.Model.Background_Colours.Red );

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getRadioButtonListField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the selection list form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField"> Evado.UniForm.Model.Field  object.</param>
    //  ---------------------------------------------------------------------------------
    private void getSelectonField (
      Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getSelectonField" );
      this.LogDebug ( "Options: " + Field.Design.Options );

      // 
      // Initialise the methods object and variables.
      // 
      List<Evado.Model.EvOption> optionlist = Evado.UniForm.Model.EuStatics.getStringAsOptionList (
        Field.Design.Options );

      optionlist.Add ( new Evado.Model.EvOption ( "", "Not Selected" ) );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Selection_List;
      GroupField.Value = Field.ItemValue;
      GroupField.OptionList = optionlist;
      GroupField.Mandatory = Field.Design.Mandatory;
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Mandatory, Evado.UniForm.Model.Background_Colours.Red );

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getSelectonField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the external selection list form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField"> Evado.UniForm.Model.Field  object.</param>
    //  ---------------------------------------------------------------------------------
    private void getExternalSelectonField (
      Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getExternalSelectonField" );

      // 
      // Initialise the methods object and variables.
      // 
      String listId = Field.Design.ExSelectionListId;
      String category = String.Empty;

      //
      // the category contains the category field then set the category value to this field value.
      //
      if ( Field.Design.ExSelectionListCategory != null )
      {
        category = Field.Design.ExSelectionListCategory; ;

        this.LogDebug ( "Auto Category: {0} ", category );

        if ( category.Contains ( EdRecordField.CONST_CATEGORY_AUTO_FIELD_IDENTIFIER ) == true )
        {
          string autoCategory = category.Replace ( EdRecordField.CONST_CATEGORY_AUTO_FIELD_IDENTIFIER, String.Empty );

          category = this.GetFieldObject ( autoCategory ).ItemValue;
          if ( category == null )
          {
            category = String.Empty;
          }
          this.LogDebug ( "Auto Category: {0} ", category );
        }
      }
      this.LogDebug ( "List: {0}, Category: {1} ", listId, category );

      //
      // get the external selection list options.
      //
      List<Evado.Model.EvOption> optionlist = this.AdapterObjects.getSelectionOptions ( listId, category, false, true );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Selection_List;
      GroupField.Value = Field.ItemValue;
      GroupField.OptionList = optionlist;
      GroupField.Mandatory = Field.Design.Mandatory;
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Mandatory, Evado.UniForm.Model.Background_Colours.Red );

    }//END getSelectonField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the external selection list form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField"> Evado.UniForm.Model.Field  object.</param>
    //  ---------------------------------------------------------------------------------
    private void getExternalRadioButtonField (
      Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getExternalRadioButtonField" );

      // 
      // Initialise the methods object and variables.
      // 
      String listId = Field.Design.ExSelectionListId;
      String category = Field.Design.ExSelectionListCategory;
      this.LogDebug ( "List: {0}, Category: {1} ", listId, category );

      //
      // the category contains the category field then set the category value to this field value.
      //
      if ( category.Contains ( EdRecordField.CONST_CATEGORY_AUTO_FIELD_IDENTIFIER ) == true )
      {
        var autoCategory = category.Replace ( EdRecordField.CONST_CATEGORY_AUTO_FIELD_IDENTIFIER, String.Empty );

        category = this.GetFieldObject ( autoCategory ).ItemValue;
        this.LogDebug ( "Auto Category: {0} ", category );
      }

      //
      // get the external selection list options.
      //
      List<Evado.Model.EvOption> optionlist = this.AdapterObjects.getSelectionOptions ( listId, category, false, false );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Radio_Button_List;
      GroupField.Value = Field.ItemValue;
      GroupField.OptionList = optionlist;
      GroupField.Mandatory = Field.Design.Mandatory;
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Mandatory, Evado.UniForm.Model.Background_Colours.Red );

      this.LogMethodEnd ( "getExternalRadioButtonField" );

    }//END getExternalRadioButtonField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the external selection list form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField"> Evado.UniForm.Model.Field  object.</param>
    //  ---------------------------------------------------------------------------------
    private void getExternalCheckBoxField (
      Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getExternalCheckBoxField" );

      // 
      // Initialise the methods object and variables.
      // 
      String listId = Field.Design.ExSelectionListId;
      String category = Field.Design.ExSelectionListCategory;
      this.LogDebug ( "List: {0}, Category: {1} ", listId, category );

      //
      // the category contains the category field then set the category value to this field value.
      //
      if ( category.Contains ( EdRecordField.CONST_CATEGORY_AUTO_FIELD_IDENTIFIER ) == true )
      {
        var autoCategory = category.Replace ( EdRecordField.CONST_CATEGORY_AUTO_FIELD_IDENTIFIER, String.Empty );

        category = this.GetFieldObject ( autoCategory ).ItemValue;
        this.LogDebug ( "Auto Category: {0} ", category );
      }

      //
      // get the external selection list options.
      //
      List<Evado.Model.EvOption> optionlist = this.AdapterObjects.getSelectionOptions ( listId, category, false, false );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Check_Box_List;
      GroupField.Value = Field.ItemValue;
      GroupField.OptionList = optionlist;
      GroupField.Mandatory = Field.Design.Mandatory;
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Mandatory, Evado.UniForm.Model.Background_Colours.Red );

      this.LogMethodEnd ( "getExternalCheckBoxField" );

    }//END getExternalRadioButtonField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the check button list form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField"> Evado.UniForm.Model.Field object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void getCheckButtonListField ( Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getCheckButtonoField" );
      this.LogDebug ( "ItemValue: " + Field.ItemValue );
      this.LogDebug ( "Options: " + Field.Design.Options );

      if ( Field.ItemValue == "Null" )
      {
        Field.ItemValue = String.Empty;
      }

      // 
      // Initialise the methods object and variables.
      // 
      List<Evado.Model.EvOption> optionlist = Evado.UniForm.Model.EuStatics.getStringAsOptionList (
        Field.Design.Options );

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
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getCheckButtonListField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the check button list form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField"> Evado.UniForm.Model.Field object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void getQueryCheckButtonListField ( Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getQueryCheckButtonListField" );
      this.LogDebug ( "ItemValue: " + Field.ItemValue );
      this.LogDebug ( "Options: " + Field.Design.Options );

      if ( Field.ItemValue == "Null" )
      {
        Field.ItemValue = String.Empty;
      }

      // 
      // Initialise the methods object and variables.
      // 
      List<Evado.Model.EvOption> optionlist = Evado.UniForm.Model.EuStatics.getStringAsOptionList (
        Field.Design.Options );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Check_Box_List;
      GroupField.Value = Field.ItemValue;
      GroupField.OptionList = optionlist;
      GroupField.Mandatory = Field.Design.Mandatory;

      GroupField.SetValueColumnWidth ( Evado.UniForm.Model.FieldValueWidths.Twenty_Percent );
      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Field_Value_Legend, EdLabels.FormField_Checkbox_Query_Legend );

      this.LogDebug ( "GroupField.Value: " + GroupField.Value );

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getCheckButtonListField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the horzontal radio buttons form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField"> Evado.UniForm.Model.Field object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void getHorizontalRadioButtonField ( Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getHorizontalRadioButtonField" );

      this.LogDebug ( "Options: " + Field.Design.Options );

      // 
      // Initialise the methods object and variables.
      // 
      List<Evado.Model.EvOption> optionlist = Evado.UniForm.Model.EuStatics.getStringAsOptionList (
        Field.Design.Options );

      optionlist.Add ( new Evado.Model.EvOption ( "", "Not Selected" ) );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Horizontal_Radio_Buttons;
      GroupField.Value = Field.ItemValue;
      GroupField.OptionList = optionlist;
      GroupField.Mandatory = Field.Design.Mandatory;
      GroupField.setBackgroundColor ( Evado.UniForm.Model.FieldParameterList.BG_Mandatory, Evado.UniForm.Model.Background_Colours.Red );

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getHorizontalRadioButtonField method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the analogue scale form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">  Evado.UniForm.Model.Fields object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void getAnalogueScaleField ( Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
    {
      this.LogMethod ( "getAnalogueScale" );

      // 
      // set the field properties and parameters.
      // 
      GroupField.Type = Evado.Model.EvDataTypes.Analogue_Scale;
      GroupField.Value = Field.ItemValue;

      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Min_Label,
       Field.Design.AnalogueLegendStart );

      GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Max_Label,
       Field.Design.AnalogueLegendFinish );

      // 
      // Set the custom validation script if it exists.
      // 
      if ( Field.Design.JavaScript != String.Empty )
      {
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getAnalogueScale method.

    //  =================================================================================
    /// <summary>
    /// Description:
    ///   This method generates the text form field object as html markup.
    /// 
    /// </summary>
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    //  ---------------------------------------------------------------------------------
    private void getUserEndorsementField (
        Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
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
    /// <param name="Field">   Evado.Digital.Model.EvForm object containing the form to be generated.</param>
    /// <param name="ViewState">  Evado.Digital.Model.EvForm.FormDisplayStates enumerated value.</param>
    /// <param name="GroupField">Evado.UniForm.Model.Field object.</param>
    /// <returns>String containing HTML markup for the form.</returns>
    //  ---------------------------------------------------------------------------------
    private void getTableField (
        Evado.Digital.Model.EdRecordField Field,
      Evado.UniForm.Model.Field GroupField )
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
      GroupField.Table = new Evado.UniForm.Model.Table ( );
      GroupField.Type = Evado.Model.EvDataTypes.Table;
      GroupField.EditAccess = Evado.UniForm.Model.EditAccess.Inherited;
      GroupField.Layout = Evado.UniForm.Model.FieldLayoutCodes.Column_Layout;

      // 
      // Initialise the field object.
      // 
      if ( Field.Design.Instructions != String.Empty )
      {
        GroupField.Description = Field.Design.Instructions;
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
        if ( Field.Table.Header [ column ].TypeId == EvDataTypes.Special_Matrix )
        {
          GroupField.Table.Header [ column ].TypeId = EvDataTypes.Read_Only_Text;
        }

        if ( GroupField.Table.Header [ column ].TypeId == EvDataTypes.Numeric )
        {
          GroupField.Table.Header [ column ].OptionsOrUnit = Field.Table.Header [ column ].OptionsOrUnit;
        }

        if ( GroupField.Table.Header [ column ].TypeId == EvDataTypes.Radio_Button_List
          || GroupField.Table.Header [ column ].TypeId == EvDataTypes.Selection_List )
        {
          GroupField.Table.Header [ column ].OptionList = Evado.UniForm.Model.EuStatics.getStringAsOptionList (
            Field.Table.Header [ column ].OptionsOrUnit );
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
        Evado.UniForm.Model.TableRow row = new Evado.UniForm.Model.TableRow ( );
        Evado.Digital.Model.EdRecordTableRow evRow = Field.Table.Rows [ inRow ];

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
        GroupField.AddParameter ( Evado.UniForm.Model.FieldParameterList.Validation_Callback,
          "Evado.Pages.[ '" + this._PageId + "' ]." + Field.FieldId + "_Validation" );
      }

    }//END getTableField method.

    // ***********************************************************************************
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
      List<Evado.UniForm.Model.Parameter> CommandParameters,
         Evado.Digital.Model.EdRecord Form )
    {
      this.LogMethod ( "updateFormObject" );
      this.LogDebug ( "Returned Field count = " + CommandParameters.Count );
      this.LogDebug ( "FormId: " + Form.LayoutId );
      this.LogDebug ( "RecordId: " + Form.RecordId );
      this.LogDebug ( "Form State: " + Form.State );
      //
      // Initialise the methods variables and objects.
      //
      Evado.Digital.Model.EvFormStaticField staticField = new Evado.Digital.Model.EvFormStaticField ( );
      // 
      // If the state is editable then update the TestReport.
      // 
      if ( this._FormAccessRole == EdRecord.FormAccessRoles.Record_Reader )
      {
        return;
      }

      //
      // Set the form role for this user.
      //
      Form.setUserAccess ( this.Session.UserProfile );
      this._FormAccessRole = Form.FormAccessRole;
      this.LogDebug ( "FormAccessRole: " + Form.FormAccessRole );

      this._Sections = Form.Design.FormSections;

      this.LogDebug ( "Updating field values." );

      // 
      // Iterate through the test fields updating the fields that have changed.
      // 
      for ( int count = 0; count < Form.Fields.Count; count++ )
      {
        if ( Form.Fields [ count ].Design.HideField == true )
        {
          this.LogDebug ( "Field: " + Form.Fields [ count ].FieldId + " a hidden value." );
          continue;
        }

        // 
        // Retrieve the form field value and update the field object if the value has changed.
        // 
        this.updateFormField (
          CommandParameters,
          Form.Fields [ count ],
          Form.State );


        //this.LogDebug ( "Field: {0}, value {1}.", Form.Fields [ count ].FieldId , Form.Fields [ count ].ItemValue );

      }//END test field iteration.

      // 
      // Update the test comments
      // 
      this.updateFormComments (
      CommandParameters,
      Form );


    }//END updateFormObject method

    // ==================================================================================
    /// <summary>
    /// This method add a parameter to the groupCommand's parameter list..
    /// </summary>
    /// <param name="ParameterList">List of MethodParameter objects.</param>
    /// <param name="ParameterName">String: name of the parameter to be retrieved.</param>
    //  ---------------------------------------------------------------------------------
    private String GetParameterValue (
      List<Evado.UniForm.Model.Parameter> ParameterList,
      String ParameterName )
    {
      // 
      // Search the parmeters for existing parameters.
      // and exit if update the value.
      // 
      foreach ( Evado.UniForm.Model.Parameter parameter in ParameterList )
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
    ///   This method updates the test annotation field.
    /// 
    /// </summary>
    /// <param name="Form">   Evado.Digital.Model.EvForm object containing test ResultData.</param>
    /// <param name="CommandParameters">The returned test field values.</param>
    /// <param name="User">The users profile.</param>
    /// <returns>Returns a string containing the Java Scripts.</returns>
    //  ----------------------------------------------------------------------------------
    private void updateFormComments (
      List<Evado.UniForm.Model.Parameter> CommandParameters,
         Evado.Digital.Model.EdRecord Form )
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
      Evado.Digital.Model.EdFormRecordComment.AuthorTypeCodes authorType = Evado.Digital.Model.EdFormRecordComment.AuthorTypeCodes.Record_Author;
      // 
      // Get the static test comment field value.
      // 
      if ( this._FormAccessRole == EdRecord.FormAccessRoles.Record_Author )
      {
        stValue = this.GetParameterValue ( CommandParameters, EuRecordGenerator.CONST_FORM_COMMENT_FIELD_ID );

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
            stValue += "\r\n" + item.ItemId + EdLabels.Label_ChangeInitialValue + item.InitialValue + EdLabels.Label_ChangeNewValue + item.NewValue;
          }
        }

        // 
        // Does the returned field value exist
        // 
        if ( stValue != null )
        {
          if ( stValue != String.Empty )
          {
            Evado.Digital.Model.EdFormRecordComment comment = new Evado.Digital.Model.EdFormRecordComment (
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
    /// <param name="FormField">  Evado.Digital.Model.EvFormField object containing test field ResultData.</param>
    /// <param name="CommandParameters">Containing the returned formfield values.</param>
    /// <param name="FormState">Current FormRecord state</param>
    /// <param name="DisplayState">Channel display state</param>
    /// <param name="User">The user's profile.</param>
    /// <returns>Returns a   Evado.Digital.Model.EvFormField object.</returns>
    //  ----------------------------------------------------------------------------------
    private void updateFormField (
      List<Evado.UniForm.Model.Parameter> CommandParameters,
        Evado.Digital.Model.EdRecordField FormField,
         Evado.Digital.Model.EdRecordObjectStates FormState )
    {

      this.LogMethod ( "updateFormField" );
      //this.LogDebug ( "FieldId: {0} type: {1}.", FormField.FieldId, FormField.TypeId );
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
      if ( this._FormAccessRole == EdRecord.FormAccessRoles.Record_Author )
      {

        switch ( FormField.TypeId )
        {
          // 
          // If field type is a free lowerText value update it 
          // 
          case Evado.Model.EvDataTypes.Free_Text:
          case Evado.Model.EvDataTypes.Signature:
            {
              //this.LogDebug ( "Process free text or signature field." );
              //
              // Update the field value if it has changed.
              //
              this.updateTextField (
                CommandParameters,
                FormField,
                true );

              //this.LogDebug ( "Value: " + FormField.ItemText );
              break;
            }// END Update single value fields.

          case Evado.Model.EvDataTypes.Special_Matrix:
          case Evado.Model.EvDataTypes.Table:
            {
              //this.LogDebug ( "Process table field." );

              this.updateFormTableFields ( CommandParameters, FormField );
              break;

            }//END processing table or matrix
          case EvDataTypes.User_Endorsement:
            {
              this.updateUserEndorcementField (
                CommandParameters,
                FormField );
              break;
            }
          default:
            {
              //this.LogDebug ( "Process single value field." );
              //
              // Update the field value if it has changed.
              //
              this.updateTextField (
                CommandParameters,
                FormField,
                false );

              //
              // upload the image if present.
              //
              this.saveImageFile ( FormField );

              break;

            }// END Update single value fields.

        }//END stiwtch statement.

        //this.LogDebugValue ( "Final field state: " + FormField.State );

      }//END updating field

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
    /// <param name="FormField">  Evado.Digital.Model.EvFormField object containing test field ResultData.</param>
    /// <param name="CommandParameters">Containing the returned formfield values.</param>
    /// <param name="IsFreeText">boolean: if the field is free text or not.</param>
    //  ----------------------------------------------------------------------------------
    private void updateTextField (
      List<Evado.UniForm.Model.Parameter> CommandParameters,
        Evado.Digital.Model.EdRecordField FormField,
       bool IsFreeText )
    {
      this.LogMethod ( "updateTextField method " );
      this.LogDebug ( "FormField.FieldId: " + FormField.FieldId );
      this.LogDebug ( "IsFreeText: " + IsFreeText );

      String stValue = this.GetParameterValue ( CommandParameters, FormField.FieldId );
      this.LogDebug ( "stValue: " + stValue );

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
            && stValue.ToLower ( ) == Evado.Digital.Model.EvcStatics.CONST_NUMERIC_NOT_AVAILABLE.ToLower ( ) )
          {
            float errors = Evado.Digital.Model.EvcStatics.CONST_NUMERIC_NULL;
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
            this.LogDebug ( "Field Change: FieldId: '{0}' Old: '{1}' New: {2}'",
              FormField.FieldId, FormField.ItemValue, stValue );

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


    // ==================================================================================
    /// <summary>
    /// THis method copies the upload image file to the image directory.
    /// </summary>
    /// <param name="FormField">  Evado.Digital.Model.EvFormField object containing test field ResultData.</param>
    /// <param name="CommandParameters">Containing the returned formfield values.</param>
    //  ----------------------------------------------------------------------------------
    private void saveImageFile (
        Evado.Digital.Model.EdRecordField FormField )
    {
      this.LogMethod ( "saveImageFile" );

      if ( FormField.TypeId != EvDataTypes.Image )
      {
        this.LogDebug ( "Not an image field" );
        this.LogMethodEnd ( "saveImageFile" );
        return;
      }

      this.LogDebug ( "Field Id {0}, Value '{1}'", FormField.FieldId, FormField.ItemValue );

      if ( FormField.ItemValue == String.Empty )
      {
        this.LogDebug ( "Empty Field" );
        this.LogMethodEnd ( "saveImageFile" );
        return;
      }


      //
      // Initialise the method variables and objects.
      //
      String stSourcePath = this.UniForm_BinaryFilePath + FormField.ItemValue;
      String stImagePath = this.UniForm_ImageFilePath + FormField.ItemValue;

      this.LogDebug ( "Source path '{0}'.", stSourcePath );
      this.LogDebug ( "Image path '{0}'.", stImagePath );

      //
      // Save the file to the directory repository.
      //
      try
      {
        System.IO.File.Copy ( stSourcePath, stImagePath, true );
      }
      catch ( Exception ex )
      {
        this.LogException ( ex );
      }
      this.LogMethodEnd ( "saveImageFile" );
    }//END saveImageFile method

    //  =============================================================================== 
    /// <summary>
    /// This method updates the text field object in the form field with the values from parameter list.
    /// 
    /// Description:
    ///   This method updates the test table field values.
    /// 
    /// </summary>
    /// <param name="FormField">  Evado.Digital.Model.EvFormField object containing test field ResultData.</param>
    /// <param name="CommandParameters">Containing the returned formfield values.</param>
    /// <param name="IsFreeText">boolean: if the field is free text or not.</param>
    /// <returns>String Returns a form field commenet.</returns>
    //  ----------------------------------------------------------------------------------
    private void updateUserEndorcementField (
      List<Evado.UniForm.Model.Parameter> CommandParameters,
        Evado.Digital.Model.EdRecordField FormField )
    {
      //this.LogMethod ( "updateUserEndorcementField method " );
      //this.LogDebugValue ( "FormField.FieldId: " + FormField.FieldId );
      //this.LogDebugValue ( "FormField.Section: " + FormField.Design.Section );

      String stValue = this.GetParameterValue ( CommandParameters, FormField.FieldId );
      //this.LogDebugValue ( "stValue: " + stValue );

      EdRecordSection section = this.getSection ( FormField );
      if ( section != null )
      {
        //this.LogDebugValue ( "Title: " + section.Title );
        if ( section.hasRoleEdit ( this.Session.UserProfile.Roles ) == false )
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
    private EdRecordSection getSection ( EdRecordField FormField )
    {
      //this.LogMethod ( "updateUserEndorcementField method " );
      //this.LogDebugValue ( "Design.Section: " + FormField.Design.Section );
      //
      // Iterate through teh sections and return the matching section object
      //
      foreach ( EdRecordSection section in this._Sections )
      {
        //this.LogDebugValue ( "section.Section: " + section.Section );

        if ( section.No == FormField.Design.SectionNo )
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
    /// <param name="FormField">  Evado.Digital.Model.EvFormField object containing test field ResultData.</param>
    /// <param name="CommandParameters">Containing the returned formfield values.</param>
    /// <returns>String Returns a form field commenet.</returns>
    //  ----------------------------------------------------------------------------------
    private void updateFormTableFields (
      List<Evado.UniForm.Model.Parameter> CommandParameters,
        Evado.Digital.Model.EdRecordField FormField )
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
              if ( FormField.Table.Header [ inCol ].TypeId == EvDataTypes.Numeric )
              {
                if ( stValue.ToLower ( ) == Evado.Digital.Model.EvcStatics.CONST_NUMERIC_NOT_AVAILABLE.ToLower ( ) )
                {
                  stValue = Evado.Digital.Model.EvcStatics.CONST_NUMERIC_NULL.ToString ( );
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

            }//END cell value changed

          }//END value exists.

        }//END column interation loop

      }//END row interation loop

      return;

    }//END updateFormFieldTable method

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region    Evado.Digital.Model.EvForm Java Script generation methods.

    //  =============================================================================== 
    /// <summary>
    ///   This method generates the Java script object variables for the form.
    /// 
    /// </summary>
    /// <param name="FormGuid"> Guid: containing the page Guid.</param>
    /// <param name="FieldList">List<  Evado.Digital.Model.EvFormField> object containing a list of fields.</param>
    /// <param name="ClientDataObject">UniForm.Model.Page objectthe Uniform client page content.</param>
    /// <param name="BinaryFilePath">string binary file path.</param>
    //  ----------------------------------------------------------------------------------
    private void getFormJavaScript (
      Guid PageGuid,
      List<Evado.Digital.Model.EdRecordField> FieldList,
      Evado.UniForm.Model.Page ClientDataObject,
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
      Evado.Digital.Model.EvcStatics.Files.saveFile ( BinaryFilePath, stJavaFileName, stJavaScript.ToString ( ) );

      this.LogMethodEnd ( "getFormJavaScript" );
    }//END getFormJavaScript method

    //  =============================================================================== 
    /// <summary>
    /// Description:
    ///   This method generates the Java script computed fields.
    /// 
    /// </summary>
    /// <param name="FieldList">List<  Evado.Digital.Model.EvFormField> object containing a list of fields.</param>
    /// <returns>String:  Java Script source code</returns>
    //  ----------------------------------------------------------------------------------
    private String getComputedFieldScripts (
      List<Evado.Digital.Model.EdRecordField> FieldList )
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
      foreach ( Evado.Digital.Model.EdRecordField field in FieldList )
      {
        // 
        // Extract the computed fields script and add it to the function.
        // 
        if ( field.TypeId == Evado.Model.EvDataTypes.Computed_Field
          && field.Design.JavaScript != String.Empty )
        {
          this.LogDebug ( "FieldId: " + field.FieldId
          + ", Type: " + field.TypeId
          + ", Section: " + field.Design.SectionNo
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
    /// <param name="FieldList">  List< Evado.Digital.Model.EvFormField> object.</param>
    /// <returns>Returns a string containing the Java Scripts.</returns>
    //  ----------------------------------------------------------------------------------
    public String getCustomValidationScripts (
      List<Evado.Digital.Model.EdRecordField> FieldList )
    {
      this.LogMethod ( "getCustomValidationScripts" );
      // 
      // Initialise the methods objects and variables.
      // 
      System.Text.StringBuilder sbJScript = new StringBuilder ( );

      // 
      // Iterate through the form fields to extract field values.
      // 
      foreach ( Evado.Digital.Model.EdRecordField field in FieldList )
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
