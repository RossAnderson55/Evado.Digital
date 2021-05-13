/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Records.cs" company="EVADO HOLDING PTY. LTD.">
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
using System.Collections;
using System.Text;
using System.Web;
using System.Web.SessionState;

 
using Evado.Model;
using Evado.Digital.Bll;
using Evado.Digital.Model;
// using Evado.Web;

namespace Evado.Digital.Adapter
{
  /// <summary>
  /// This class defines the application base classs that is used to terminate the 
  /// hosted application objects.
  /// 
  /// This class terminates the Customer object.
  /// </summary>
  public class EuEntityFields : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuEntityFields ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuEntityFields.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuEntityFields (
      EuGlobalObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuEntityFields.";
      this.AdapterObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;


      this.LogInitMethod ( "EuEntityFields initialisation" );
      this.LogInit ( "-ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "-SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "-SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "-UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );

      this.LogInit ( "-Settings.LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-Settings.UserProfile.UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-Settings.UserProfile.CommonName: " + Settings.UserProfile.CommonName );

      this._Bll_EntityFields = new EdEntityFields ( Settings );

    }//END Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Digital.Bll.EdEntityFields _Bll_EntityFields = new Evado.Digital.Bll.EdEntityFields ( );

    public const string CONST_REFRESH = "RFSH";
    public const string CONST_TABLE_ROW_FIELD = "ROWS";
    public const string CONST_TABLE_FIELD_ID = "TBL";
    public const string CONST_MATIX_COL_FIELD = "COLS";
    public const string CONST_MATRIX_FIELD_ID = "MAT";
    public const string CONST_FIELD_COUNT = "FFC";
    public const string CONST_FORM_SECTION = "FSTM";

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ===============================================================================
    /// <summary>
    /// This method gets the application object from the list.
    /// 
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    /// <returns>ClientApplicationData</returns>
    //  ----------------------------------------------------------------------------------
    override public Evado.Model.UniForm.AppData getDataObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getClientDataObject" );
      this.LogValue ( "PageCommand: " + PageCommand.getAsString ( false, true ) );

      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasManagementAccess == false )
        {
          this.LogIllegalAccess ( this.ClassNameSpace + "getDataObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

          this.Session.LastPage.Message = this.ErrorMessage;

          return this.Session.LastPage;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess ( this.ClassNameSpace + "getDataObject",
          this.Session.UserProfile );

        //
        // Reset the groupCommand method using a page type groupCommand parameter
        //
        if ( PageCommand.hasParameter ( Evado.Model.UniForm.CommandParameters.Create_Object ) == true )
        {
          string stPageType = PageCommand.GetParameter ( Evado.Model.UniForm.CommandParameters.Create_Object );

          if ( stPageType == "1" )
          {
            PageCommand.Method = Evado.Model.UniForm.ApplicationMethods.Create_Object;
          }
        }

        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          // 
          // Select the method to retrieve a record object.
          // 
          case Evado.Model.UniForm.ApplicationMethods.Get_Object:
            {
              clientDataObject = this.getObject ( PageCommand );
              break;

            }//END get object case

          // 
          // Select the groupCommand to create a new record object.
          // 
          case Evado.Model.UniForm.ApplicationMethods.Create_Object:
            {
              clientDataObject = this.createObject ( PageCommand );
              break;
            }//END create case

          // 
          // Select the method to update the record object.
          // 
          case Evado.Model.UniForm.ApplicationMethods.Save_Object:
            {
              this.LogValue ( " Save Object method" );

              // 
              // Update the object values
              // 
              clientDataObject = this.updateObject ( PageCommand );
              break;

            }//END save case
          default:
            {
              this.LogValue ( "A valid method was not selected." );
              clientDataObject = this.Session.LastPage;
              break;
            }

        }//END Switch

        // 
        // Handle returned exceptions.
        // 
        if ( clientDataObject == null )
        {
          this.LogDebug ( " null application data returned." );
          clientDataObject = this.Session.LastPage;
        }

        //
        // If an errot message exist display it.
        //
        if ( this.ErrorMessage != String.Empty )
        {
          clientDataObject.Message = this.ErrorMessage;
        }

        // 
        // return the client ResultData object.
        // 
        this.LogMethodEnd ( "getClientDataObject" );
        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // If an exception is created log the error and return an error.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END getDataObject methods

    // ==================================================================================
    /// <summary>
    /// This method Saves the clinical objects to user session object.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public void SaveSessionObjects ( )
    {
      this.LogMethod ( "SaveSessionObjects" );
      // 
      // Save the session ResultData so it is available for the next user generated groupCommand.
      // 


    }//END SaveSessionObjects method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class private methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getObject" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid entityFieldGuid = Guid.Empty;
      //bool refreshPage = false;

      try
      {
        // 
        // Retrieve the record guid from the parameters
        // 
        entityFieldGuid = PageCommand.GetGuid ( );
        this.LogValue ( " formGuid: " + entityFieldGuid );

        // 
        // if the guid is empty the parameter was not found to exit.
        // 
        if ( entityFieldGuid == Guid.Empty )
        {
          this.LogValue ( "recordGuid is EMPTY" );

          return null;
        }

        //
        // If the field has changed then get a new instance of the field.
        //
        if ( this.Session.EntityField.Guid != entityFieldGuid )
        {
          // 
          // Retrieve the record object from the database via the DAL and BLL layers.
          // 
          this.Session.EntityField = this._Bll_EntityFields.getField ( entityFieldGuid );

          this.LogClass ( this._Bll_EntityFields.Log );

        }

        this.LogDebug ( "Field.FieldId {0}. ", this.Session.EntityField.FieldId );
        this.LogDebug ( "Field Type {0}.", this.Session.EntityField.TypeId );

        //
        // Extract the ResultData type to validated if it has been changed.
        //
        String stDataType = PageCommand.GetParameter (
          EdRecordField.ClassFieldNames.TypeId.ToString ( ) );
        string value = PageCommand.GetParameter ( EuRecordLayouts.CONST_REFRESH );

        //
        // Refresh the page values on the server, used for changing the field type setting
        // to enable the relevant groups.
        //
        if ( ( stDataType != String.Empty
          && stDataType != this.Session.EntityField.TypeId.ToString ( ) )
          || value == "1" )
        {
          this.LogValue ( "Data Type has changed." );

          this.updateObjectValues ( PageCommand );

          this.updateTableValues ( PageCommand );

          this.updateMatrixValues ( PageCommand );

          this.validateFieldId ( PageCommand );
        }
        if ( this.Session.EntityField.TypeId == EvDataTypes.Radio_Button_List )
        {
          this.LogDebug ( "Is Options numeric: " + this.Session.EntityField.Design.hasNumericValues );
        }
        // 
        // Save the session ResultData so it is available for the next user generated groupCommand.
        // 
        this.SaveSessionObjects ( );

        //
        // Generate the client ResultData object.
        //
        this.getDataObject ( clientDataObject );

        if ( this.ErrorMessage != String.Empty )
        {
          clientDataObject.Message = this.ErrorMessage;
        }

        // 
        // Return the client ResultData object to the calling method.
        // 
        this.LogMethodEnd ( "getObject" );
        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // On an exception raised create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Record_Retrieve_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getObject" );
      return this.Session.LastPage;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method creates the group commands
    /// </summary>
    //  ------------------------------------------------------------------------------
    private void getGroupCommands (
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "getGroupCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );

      //
      // Add the page refresh groupCommand.
      //
      groupCommand = PageGroup.addCommand (
        EdLabels.Form_Field_Refresh_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Entity_Layout_Fields.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Evado.Model.UniForm.ApplicationMethods.Get_Object );
      groupCommand.SetGuid ( this.Session.EntityField.Guid );
      groupCommand.AddParameter ( EuRecordLayouts.CONST_REFRESH, "1" );

      //
      // Edit unless the page is in edit mode
      //
      if ( PageGroup.EditAccess != Evado.Model.UniForm.EditAccess.Enabled )
      {
        return;
      }

      //
      // Add the same groupCommand.
      //
      groupCommand = PageGroup.addCommand (
        EdLabels.Form_Field_Save_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Entity_Layout_Fields.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      // 
      // Define the save groupCommand parameters.
      // 
      groupCommand.SetGuid ( this.Session.EntityField.Guid );

      groupCommand.AddParameter (
        Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
       Evado.Digital.Bll.EdEntityFields.Action_Save );

      //
      // Add the same groupCommand.
      //
      int majorVersion = (int) this.Session.EntityLayout.Design.Version;
      if ( this.Session.EntityField.Design.InitialVersion == majorVersion )
      {
        groupCommand = PageGroup.addCommand (
          EdLabels.Form_Field_Delete_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Entity_Layout_Fields.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );
        // 
        // Define the save groupCommand parameters.
        // 
        groupCommand.SetGuid ( this.Session.EntityField.Guid );

        groupCommand.AddParameter (
          Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
         EdEntityFields.Action_Delete );
      }

    }//END getSaveCommand Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      if ( this.Session.EntityField.RecordMedia == null
        && ( this.Session.EntityField.TypeId == EvDataTypes.Streamed_Video
          || this.Session.EntityField.TypeId == EvDataTypes.External_Image ) )
      {
        this.Session.EntityField.RecordMedia = new EdRecordMedia ( );
      }

      // 
      // Initialise the client ResultData object.
      // 
      ClientDataObject.Id = this.Session.EntityField.Guid;
      ClientDataObject.Title = EdLabels.Form_Field_Page_Title_Prefix
        + this.Session.EntityField.FieldId
        + EdLabels.Space_Hypen
        + this.Session.EntityField.Title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
      ClientDataObject.Page.PageId = this.Session.EntityField.FieldId;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;

      //
      // Set the user's edit access if they have configuration edit access.
      //
      this.LogValue ( "HasConfigrationEditAccess: " + this.Session.UserProfile.hasManagementAccess );



      if ( this.Session.UserProfile.hasManagementAccess == true         
        || ( this.Session.UserProfile.hasAdministrationAccess == true
          && this.AdapterObjects.Settings.EnableAdminUpdateOfIssuedObjects == true ))
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      this.LogValue ( "Field TypeId: " + this.Session.EntityField.TypeId );

      //
      // add a field generate group
      //
      this.createFieldGeneral_Group ( ClientDataObject.Page );

      //
      // Add the properties field group.

      this.createFieldProperties_Group ( ClientDataObject.Page );

      //
      // add a pageMenuGroup with fields for custom field validations.
      //
      if ( this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Computed_Field )
      {
        this.Session.EntityField.Design.Mandatory = false;

        this.createComputedScriptGroup ( ClientDataObject.Page );

        return;
      }

      //
      // display the field validation pageMenuGroup
      //
      //this.createFieldValidationGroup ( ClientDataObject.Page );

      //
      // display the numeric field validation pageMenuGroup.
      //
      this.createFieldNumericValidationGroup ( ClientDataObject.Page );

      //
      // add a pageMenuGroup with fields for custom field validations.
      //
      //this.createCustomValidationGroup ( ClientDataObject.Page );

      //
      // Display the table property gorup.
      //
      this.createTableFieldGroup ( ClientDataObject.Page );

      //
      // display the matrix property pageMenuGroup.
      //
      this.creatMatrixFieldGroup ( ClientDataObject.Page );

      this.LogMethodEnd ( "getDataObject" );
    }//END getFormDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method generates the general pageMenuGroup for the field properties.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void createFieldGeneral_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "createGeneralFieldGroup" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      List<EvOption> optionList = new List<EvOption> ( );
      Evado.Model.UniForm.EditAccess initialAccess = Evado.Model.UniForm.EditAccess.Disabled;

      int formVersion = (int) this.Session.EntityLayout.Design.Version;
      if ( this.Session.EntityField.Design.InitialVersion == formVersion )
      {
        initialAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }
      this.LogValue ( "formVersion: " + formVersion );
      this.LogValue ( "Field.InitialVersion: " + this.Session.EntityField.Design.InitialVersion );

      this.LogValue ( "initialAccess: " + initialAccess );
      this.LogValue ( "Form.Design.Section: " + this.Session.EntityField.Design.SectionNo );

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Form_Field_General_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.SetCommandBackBroundColor (
        Evado.Model.UniForm.GroupParameterList.BG_Mandatory,
        Evado.Model.UniForm.Background_Colours.Red );

      this.LogValue ( "pageGroup.EditAccess: " + pageGroup.EditAccess );
      //
      // Add the group commands
      //
      this.getGroupCommands ( pageGroup );

      //
      // Form ID
      //
      groupField = pageGroup.createReadOnlyTextField (
        String.Empty,
        EdLabels.Form_Title_Field_Label,
       this.Session.EntityLayout.Title );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form field id
      //
      groupField = pageGroup.createTextField (
        EdRecordField.ClassFieldNames.FieldId.ToString ( ),
        EdLabels.Form_Field_Identifier_Field_Label,
        this.Session.EntityField.FieldId,
        20 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // this field is only editable when the field is initialised.
      //
      groupField.EditAccess = initialAccess;

      //
      // Create the form field type selection.
      //
      optionList = EdRecordField.getDataTypes ( );

      groupField = pageGroup.createSelectionListField (
        EdRecordField.ClassFieldNames.TypeId.ToString ( ),
        EdLabels.Form_Field_Type_Field_Label,
        this.Session.EntityField.TypeId.ToString ( ),
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // this field is only editable when the field is initialised.
      //
      groupField.EditAccess = initialAccess;
      if ( groupField.EditAccess == Evado.Model.UniForm.EditAccess.Enabled )
      {
        groupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
      }

      //
      // Form title
      //
      groupField = pageGroup.createTextField (
        EdRecordField.ClassFieldNames.Subject.ToString ( ),
        EdLabels.Form_Field_Subject_Field_Label,
        this.Session.EntityField.Design.Title,
        150 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Create the Section list list,
      //
      optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( "-1", "" ) );

      foreach ( EdRecordSection section in this.Session.EntityLayout.Design.FormSections )
      {
        if ( section.Title != String.Empty )
        {
          optionList.Add ( new EvOption ( section.No.ToString ( ), section.Title ) );
        }//END section exists.

      }//END section iteration loop.

      //
      // if sections exist then display the selection list.
      //
      if ( optionList.Count > 1 )
      {
        groupField = pageGroup.createSelectionListField (
          EdRecordField.ClassFieldNames.FormSection.ToString ( ),
          EdLabels.Form_Field_Section_Field_Label,
          this.Session.EntityField.Design.SectionNo,
          optionList );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      //
      // Create the form field layout selection.
      //
      optionList = EvStatics.getOptionsFromEnum ( typeof ( Evado.Model.UniForm.FieldLayoutCodes ), true );

      if ( this.Session.EntityField.Design.FieldLayout == null )
      {
        this.Session.EntityField.Design.FieldLayout = this.Session.EntityLayout.Design.DefaultPageLayout;
      }

      groupField = pageGroup.createSelectionListField (
        EdRecordField.ClassFieldNames.Field_Layout.ToString ( ),
        EdLabels.LayoutField_Field_Layout_Field_Label,
        this.Session.EntityField.Design.FieldLayout.ToString ( ),
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

    }//END createGeneralFieldGroup Method

    // ==============================================================================
    /// <summary>
    /// This method generates the general pageMenuGroup for the field properties.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void createFieldProperties_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "createPropertiesFieldGroup" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      List<EvOption> optionList = new List<EvOption> ( );
      Evado.Model.UniForm.EditAccess initialAccess = Evado.Model.UniForm.EditAccess.Disabled;

      int formVersion = (int) this.Session.EntityLayout.Design.Version;
      if ( this.Session.EntityField.Design.InitialVersion == formVersion )
      {
        initialAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }
      this.LogValue ( "formVersion: " + formVersion );
      this.LogValue ( "Field.InitialVersion: " + this.Session.EntityField.Design.InitialVersion );

      this.LogValue ( "initialAccess: " + initialAccess );
      //
      // Define the general properties pageMenuGroup.
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Form_Field_Properties_Group_Title);
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      //
      // Form field order
      //
      groupField = pageGroup.createNumericField (
        EdRecordField.ClassFieldNames.Order.ToString ( ),
        EdLabels.Form_Field_Order_Field_Label,
        this.Session.EntityField.Order,
        0,
        200 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form field static text Fill the instruction field with text
      //
      if ( this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Read_Only_Text )
      {
        groupField = pageGroup.createFreeTextField (
          EdRecordField.ClassFieldNames.Instructions.ToString ( ),
          EdLabels.Form_Field_Read_Only_Text_Field_Label,
          this.Session.EntityField.Design.Instructions,
          150, 20 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        String instructions = EdLabels.Form_Field_Instruction_Field_Description;

        groupField.Description = instructions;
        this.Session.EntityField.Design.AiDataPoint = false;
        this.Session.EntityField.Design.Mandatory = false;
        this.Session.EntityField.Design.Mandatory = false;

        //
        // Form hide hidden field
        //
        groupField = pageGroup.createBooleanField (
          EdRecordField.ClassFieldNames.Hide_Field.ToString ( ),
          EdLabels.Form_Field_Hide_Field_Label,
          this.Session.EntityField.Design.HideField );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        return;
      }//END read only field.

      //
      // This block displays the fields needed to define an streamed video or external image
      //
      if ( this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.External_Image
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Streamed_Video )
      {

        this.Session.EntityField.Design.AiDataPoint = false;
        this.Session.EntityField.Design.Mandatory = false;
        //
        // Form Instructions
        //
        groupField = pageGroup.createFreeTextField (
          EdRecordField.ClassFieldNames.Instructions.ToString ( ),
          EdLabels.Form_Field_Instructions_Field_Label,
          EdLabels.Form_Field_External_URL_Inst_Field_Label,
          this.Session.EntityField.Design.Instructions,
          150, 5 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        //
        // the form field image url
        //
        groupField = pageGroup.createTextField (
          EdRecordField.ClassFieldNames.Media_Url,
          EdLabels.Form_Field_External_URL_Field_Label,
          this.Session.EntityField.RecordMedia.Url,
          100 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        //
        // the field media title.
        //
        groupField = pageGroup.createTextField (
          EdRecordField.ClassFieldNames.Media_Title,
          EdLabels.Form_Field_External_URL_Title_Field_Label,
          this.Session.EntityField.RecordMedia.Title,
          100 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        //
        // Form field image or video width
        //
        groupField = pageGroup.createNumericField (
          EdRecordField.ClassFieldNames.Media_Width,
          EdLabels.Form_Field_Image_Video_Width_Field_Label,
          this.Session.EntityField.RecordMedia.Width,
          0,
          1000 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        //
        // Form field image or video height
        //
        groupField = pageGroup.createNumericField (
          EdRecordField.ClassFieldNames.Media_Height,
          EdLabels.Form_Field_Image_Video_Height_Field_Label,
          this.Session.EntityField.RecordMedia.Height,
          0,
          1000 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        //
        // Form hide hidden field
        //
        groupField = pageGroup.createBooleanField (
          EdRecordField.ClassFieldNames.Hide_Field.ToString ( ),
          EdLabels.Form_Field_Hide_Field_Label,
          this.Session.EntityField.Design.HideField );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        this.Session.EntityField.Design.AiDataPoint = false;
        this.Session.EntityField.Design.Mandatory = false;

        return;
      }

      //
      // Form Instructions
      //
      groupField = pageGroup.createFreeTextField (
        EdRecordField.ClassFieldNames.Instructions.ToString ( ),
        EdLabels.Form_Field_Instructions_Field_Label,
        this.Session.EntityField.Design.Instructions,
        150, 5 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form reference
      //
      groupField = pageGroup.createTextField (
        EdRecordField.ClassFieldNames.Reference.ToString ( ),
        EdLabels.Form_Field_Reference_Field_Label,
        this.Session.EntityField.Design.HttpReference,
        50 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form field category
      //
      if ( this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Signature
        && this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Password )
      {
        groupField = pageGroup.createTextField (
          EdRecordField.ClassFieldNames.FieldCategory.ToString ( ),
          EdLabels.Form_Field_Category_Field_Title,
          this.Session.EntityField.Design.FieldCategory,
          20 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      //
      // Create the external selection list,
      //
      if ( this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.External_Selection_List
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.External_CheckBox_List
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.External_RadioButton_List )
      {
        optionList = this.AdapterObjects.getSelectionListOptions ( true );

        groupField = pageGroup.createSelectionListField (
          EdRecordField.ClassFieldNames.ExSelectionListId,
          EdLabels.Form_Field_External_Selection_Field_Label,
          this.Session.EntityField.Design.ExSelectionListId,
          optionList );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        //
        // this field is only editable when the field is initialised.
        //
        groupField.EditAccess = initialAccess;
        if ( groupField.EditAccess == Evado.Model.UniForm.EditAccess.Enabled )
        {
          groupField.AddParameter ( Evado.Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
        }

        groupField = pageGroup.createTextField (
          EdRecordField.ClassFieldNames.ExSelectionListCategory,
          EdLabels.Form_Field_External_Selection_Category_Field_Label,
         EdLabels.EntityField_External_Selection_Category_Field_Description,
          this.Session.EntityField.Design.ExSelectionListCategory,
          20 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        //
        // this field is only editable when the field is initialised.
        //
        groupField.EditAccess = initialAccess;
      }

      //
      // Form field selection options field.
      //
      if ( this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Free_Text
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Text )
      {
        groupField = pageGroup.createNumericField (
          EdRecordField.ClassFieldNames.FieldWidth,
          EdLabels.Form_Field_Field_Width_Field_Label,
          this.Session.EntityField.Design.FieldWidth,
          5, 100 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }
      else
      {
        this.Session.EntityField.Design.FieldWidth = 0;
      }

      if ( this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Free_Text )
      {
        groupField = pageGroup.createNumericField (
          EdRecordField.ClassFieldNames.FieldHeight.ToString ( ),
          EdLabels.Form_Field_Field_Height_Field_Label,
          this.Session.EntityField.Design.FieldHeight,
          2, 50 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }
      else
      {
        this.Session.EntityField.Design.FieldHeight = 0;
      }
      //
      // Form field selection options field.
      //
      if ( this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Analogue_Scale )
      {
        groupField = pageGroup.createTextField (
          EdRecordField.ClassFieldNames.AnalogueLegendStart.ToString ( ),
          EdLabels.Form_Field_Analogue_Legend_Start_Field_Label,
          this.Session.EntityField.Design.AnalogueLegendStart,
          50 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        groupField = pageGroup.createTextField (
          EdRecordField.ClassFieldNames.AnalogueLegendFinish.ToString ( ),
          EdLabels.Form_Field_Analogue_Legend_Start_Field_Label,
          this.Session.EntityField.Design.AnalogueLegendFinish,
          50 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      //
      // Form field selection options field.
      //
      if ( this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Selection_List
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Radio_Button_List
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Check_Box_List
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Horizontal_Radio_Buttons )
      {
        string stOptions = this.Session.EntityField.Design.Options.Replace ( ";", ";\r\n" );

        groupField = pageGroup.createFreeTextField (
          EdRecordField.ClassFieldNames.Selection_Options.ToString ( ),
          EdLabels.Form_Field_Selection_Options_Field_Title,
          EdLabels.Form_Field_Selection_Options_Field_Description,
          stOptions,
          90,
          10 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        //
        // this field is only editable when the field is initialised.
        //
        groupField.EditAccess = initialAccess;
      }


      //
      // Form summary field  field
      //
      if ( this.Session.EntityField.isSingleValue == true
        || this.Session.EntityField.TypeId == EvDataTypes.External_CheckBox_List )
      {
        groupField = pageGroup.createBooleanField (
          EdRecordField.ClassFieldNames.Summary_Field.ToString ( ),
          EdLabels.Form_Field_Summary_Field_Label,
          this.Session.EntityField.Design.IsSummaryField );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      //
      // Form hide mandatory field
      //
      if ( this.Session.EntityField.isReadOnly == false )
      {
        groupField = pageGroup.createBooleanField (
          EdRecordField.ClassFieldNames.Mandatory.ToString ( ),
          EdLabels.Form_Field_Mandatory_Field_Label,
          this.Session.EntityField.Design.Mandatory );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        //
        // Form hide ResultData point field
        //
        if ( this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Read_Only_Text
        && this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Free_Text
          && this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Password
          && this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Signature )
        {
          groupField = pageGroup.createBooleanField (
            EdRecordField.ClassFieldNames.AI_Data_Point.ToString ( ),
            EdLabels.LayoutFields_Enable_AI_Field_Label,
            this.Session.EntityField.Design.AiDataPoint );
          groupField.Layout = EuAdapter.DefaultFieldLayout;
        }
      }
      //
      // Form hide hidden field
      //
      groupField = pageGroup.createBooleanField (
        EdRecordField.ClassFieldNames.Hide_Field.ToString ( ),
        EdLabels.Form_Field_Hide_Field_Label,
        this.Session.EntityField.Design.HideField );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

    }//END createGeneralFieldGroup Method

    // ==============================================================================
    /// <summary>
    /// This method generates the customised pageMenuGroup for the field properties.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void createCustomValidationGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogValue ( Evado.Model.UniForm.EuStatics.CONST_METHOD_START
        + "Evado.UniForm.Clinical.FormFields.createCustomValidationGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      //
      // If a static field field exit the method.
      //
      if ( this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Text
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Read_Only_Text
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Free_Text
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Password
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Signature
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Computed_Field )
      {
        return;
      }

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Form_Field_Custom_Validation_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = PageObject.EditAccess;
      //
      // Add the group commands
      //
      this.getGroupCommands ( pageGroup );


      pageGroup.Description = EdLabels.Form_Field_CustomValidation_Field_Description;

      //
      // Form Instructions
      //
      groupField = pageGroup.createFreeTextField (
        EdRecordField.ClassFieldNames.Java_Script.ToString ( ),
        String.Empty,
        this.Session.EntityField.Design.JavaScript,
        80, 10 );
      groupField.Layout = Evado.Model.UniForm.FieldLayoutCodes.Column_Layout;

    }//END createCustomValidationGroup Method

    // ==============================================================================
    /// <summary>
    /// This method generates the computed field the field properties.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void createComputedScriptGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "createComputedScriptGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );

      //
      // If a static field field exit the method.
      //
      if ( this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Computed_Field )
      {
        return;
      }

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Form_Field_Computed_Field_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.EditAccess = PageObject.EditAccess;

      //
      // Add the group commands
      //
      this.getGroupCommands ( pageGroup );

      pageGroup.Description = "computedFields(){\r\n"
        + "var value = 0;\r\n"
        + "var field;\r\n"
        + "var fieldDisp;\r\n"
        + "\r\n computed script code.\r\n"
        + "\r\n}"
        + "\r\nThe computed script field.value is set to the value of 'value'.";

      //
      // Form Instructions
      //
      groupField = pageGroup.createFreeTextField (
        EdRecordField.ClassFieldNames.Java_Script.ToString ( ),
        String.Empty,
        this.Session.EntityField.Design.JavaScript,
        80, 10 );
      groupField.Layout = Evado.Model.UniForm.FieldLayoutCodes.Column_Layout;

    }//END createCustomValidationGroup Method

    // ==============================================================================
    /// <summary>
    /// This method generates the general pageMenuGroup for the field properties.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void createFieldValidationGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogValue ( Evado.Model.UniForm.EuStatics.CONST_METHOD_START
        + "Evado.UniForm.Clinical.FormFields.createFieldValidationGroup" );
      this.LogValue ( "TypeId: " + this.Session.EntityField.TypeId );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // If a static field field exit the method.
      //
      if ( this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Read_Only_Text
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Password
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Signature
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.Streamed_Video
        || this.Session.EntityField.TypeId == Evado.Model.EvDataTypes.External_Image )
      {
        return;
      }

      //
      // Add the group commands
      //
      this.getGroupCommands ( pageGroup );

    }//END createFieldValidationGroup Method

    // ==============================================================================
    /// <summary>
    /// This method generates the general pageMenuGroup for the field properties.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void createFieldNumericValidationGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "createFieldNumericValidationGroup" );
      this.LogValue ( "TypeId: " + this.Session.EntityField.TypeId );
      this.LogValue ( "UnitScaling: " + this.Session.EntityField.Design.UnitScaling );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // If not a numeric field exit the method.
      //
      if ( this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Numeric
        && this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Integer_Range
        && this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Float_Range )
      {
        return;
      }

      //
      // Define the Form_Field_Sex_Validation_Group_Title properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Form_Field_Numeric_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = PageObject.EditAccess;

      //
      // Add the group commands
      //
      this.getGroupCommands ( pageGroup );

      //
      // form field numeric scaling value.
      //
      optionList = Evado.Digital.Model.EvcStatics.getStringAsOptionList ( "-12:-12;-9:-9;-6:-6;-3:-3;0:0;3:3;6:6;9:9;12:12" );

      foreach ( EvOption optn in optionList )
      {
        this.LogValue ( "{0} + {1} ", optn.Value, optn.Description  );
      }

      groupField = pageGroup.createSelectionListField (
        EdRecordField.ClassFieldNames.Unit_Scaling.ToString ( ),
        EdLabels.Form_Field_Unit_Scale_Field_Label,
        this.Session.EntityField.Design.UnitScaling,
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // form field numeric unit
      //
      groupField = pageGroup.createTextField (
        EdRecordField.ClassFieldNames.Unit.ToString ( ),
        EdLabels.Form_Field_Unit_Scale_Field_Label,
        this.Session.EntityField.Design.Unit,
        10 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form lower validation range value 
      //
      groupField = pageGroup.createNumericField (
        EdRecordField.ClassFieldNames.DefaultNumericValue.ToString ( ),
        EdLabels.Form_Field_Default_Value_Field_Label,
        0,
        -1000000,
        1000000 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.Value = this.Session.EntityField.Design.DefaultValue;

      if ( groupField.Value == Evado.Digital.Model.EvcStatics.CONST_NUMERIC_NULL.ToString ( ) )
      {
        groupField.Value = Evado.Digital.Model.EvcStatics.CONST_NUMERIC_NOT_AVAILABLE;
      }

      //
      // Form lower validation range value 
      //
      groupField = pageGroup.createNumericField (
        EdRecordField.ClassFieldNames.ValidationLowerLimit.ToString ( ),
        EdLabels.Form_Field_Lower_Validation_Field_Label,
        this.Session.EntityField.Design.ValidationLowerLimit,
        -1000000,
        1000000 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form upper validation range value 
      //
      groupField = pageGroup.createNumericField (
        EdRecordField.ClassFieldNames.ValidationUpperLimit.ToString ( ),
        EdLabels.Form_Field_Upper_Validation_Field_Label,
        this.Session.EntityField.Design.ValidationUpperLimit,
        -1000000,
        1000000 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form lower Alert range value 
      //
      groupField = pageGroup.createNumericField (
        EdRecordField.ClassFieldNames.AlertLowerLimit.ToString ( ),
        EdLabels.Form_Field_Lower_Alert_Field_Label,
        this.Session.EntityField.Design.AlertLowerLimit,
        -1000000,
        1000000 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form upper Alert range value 
      //
      groupField = pageGroup.createNumericField (
        EdRecordField.ClassFieldNames.AlertUpperLimit.ToString ( ),
        EdLabels.Form_Field_Upper_Alert_Field_Label,
        this.Session.EntityField.Design.AlertUpperLimit,
        -1000000,
        1000000 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form lower Alert range value 
      //
      groupField = pageGroup.createNumericField (
        EdRecordField.ClassFieldNames.NormalRangeLowerLimit.ToString ( ),
        EdLabels.Form_Field_Lower_Normal_Range_Field_Label,
        this.Session.EntityField.Design.NormalRangeLowerLimit,
        -1000000,
        1000000 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form upper Alert range value 
      //
      groupField = pageGroup.createNumericField (
        EdRecordField.ClassFieldNames.NormalRangeUpperLimit.ToString ( ),
        EdLabels.Form_Field_Upper_Normal_Range_Field_Label,
        this.Session.EntityField.Design.NormalRangeUpperLimit,
        -1000000,
        1000000 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;


    }//END createFieldNumericValidationGroup Method

    // ==============================================================================
    /// <summary>
    /// This method generates the general pageMenuGroup for the field properties.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void createTableFieldGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "createTableFieldGroup" );
      this.LogValue ( "Field TypeId: " + this.Session.EntityField.TypeId );

      //
      // If the ResultData type is not a table or matrix exit.
      //
      if ( this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Table
        && this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Special_Matrix )
      {
        this.LogValue ( "Not a table or matix so exit" );

        return;
      }
      this.Session.EntityField.Design.IsSummaryField = false;
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // if the table object is null initialise the object.
      //
      if ( this.Session.EntityField.Table == null )
      {
        this.Session.EntityField.Table = new EdRecordTable ( );
      }

      //
      // Define the Form_Field_Sex_Validation_Group_Title properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Form_Field_Table_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = PageObject.EditAccess;

      //
      // Add the group commands
      //
      this.getGroupCommands ( pageGroup );

      //
      // Add the table row selection.
      //
      optionList = EdRecordTableHeader.getRowLengthList ( );

      groupField = pageGroup.createSelectionListField (
        EuEntityFields.CONST_TABLE_ROW_FIELD,
        EdLabels.Form_Field_Table_Row_Field_Label,
        this.Session.EntityField.Table.Rows.Count.ToString ( ),
        optionList );
      groupField.Layout = Evado.Model.UniForm.FieldLayoutCodes.Center_Justified;

      //
      // Field disabled form male subjects.
      //
      groupField = pageGroup.createTableField (
        EuEntityFields.CONST_TABLE_FIELD_ID,
        String.Empty, 5 );

      groupField.Layout = Evado.Model.UniForm.FieldLayoutCodes.Column_Layout;

      groupField.Table.Header [ 0 ].Text = "No";
      groupField.Table.Header [ 0 ].TypeId = EvDataTypes.Read_Only_Text;
      groupField.Table.Header [ 0 ].Width = "50px";

      groupField.Table.Header [ 1 ].Text = "Header text";
      groupField.Table.Header [ 1 ].TypeId = EvDataTypes.Text;
      groupField.Table.Header [ 2 ].Width = "250px";

      groupField.Table.Header [ 2 ].Text = "Width";
      groupField.Table.Header [ 2 ].TypeId = EvDataTypes.Selection_List;
      groupField.Table.Header [ 2 ].OptionList =
         Evado.Digital.Model.EvcStatics.getStringAsOptionList ( "5;10;12;15;20;25;30;35;40;45;50" );

      groupField.Table.Header [ 3 ].Text = "Data Type";
      groupField.Table.Header [ 3 ].TypeId = EvDataTypes.Selection_List;
      groupField.Table.Header [ 3 ].OptionList = EdRecordTableHeader.getTypeList (
        this.Session.EntityField.TypeId );

      groupField.Table.Header [ 4 ].Text = "Option or Unit";
      groupField.Table.Header [ 4 ].TypeId = EvDataTypes.Text;

      //
      // Output the table rows.
      //
      for ( int iCol = 0; iCol < 10; iCol++ )
      {
        String indexCol = ( iCol + 1 ).ToString ( );
        EdRecordTableHeader header = this.Session.EntityField.Table.Header [ iCol ];
        Evado.Model.UniForm.TableRow row = new Evado.Model.UniForm.TableRow ( );
        //
        // if the prefilled exists convert it to Matrix type  (readonly).
        //
        if ( this.Session.EntityField.Table.PreFilledColumnList.Contains ( indexCol ) == true )
        {
          header.TypeId = EvDataTypes.Read_Only_Text;
        }
        this.LogValue ( "Col: " + iCol + " T: " + header.Text + ", TYP: " + header.TypeId );

        row.Column [ 0 ] = header.No.ToString ( );
        row.Column [ 1 ] = header.Text.ToString ( );
        row.Column [ 2 ] = header.Width;
        row.Column [ 3 ] = header.TypeId.ToString ( );
        row.Column [ 4 ] = header.OptionsOrUnit.ToString ( );

        groupField.Table.Rows.Add ( row );
      }

    }//END createTableFieldGroup Method

    // ==============================================================================
    /// <summary>
    /// This method generates the general pageMenuGroup for the field properties.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void creatMatrixFieldGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "creatMatrixFieldGroup" );

      //
      // If the ResultData type is not a table or matrix exit.
      //
      if ( this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Special_Matrix )
      {
        this.LogValue ( "Not a matix so exit" );
        this.LogMethodEnd ( "creatMatrixFieldGroup" );
        return;
      }

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // if the table object is null initialise the object.
      //
      if ( this.Session.EntityField.Table == null )
      {
        this.Session.EntityField.Table = new EdRecordTable ( );
      }

      //
      // Define the Form_Field_Sex_Validation_Group_Title properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Form_Field_Matrix_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = PageObject.EditAccess;

      //
      // Add the group commands
      //
      this.getGroupCommands ( pageGroup );

      //
      // Field disabled form male subjects.
      //
      groupField = pageGroup.createTableField (
        EuEntityFields.CONST_MATRIX_FIELD_ID,
        String.Empty,
        10 );
      groupField.Layout = Evado.Model.UniForm.FieldLayoutCodes.Column_Layout;

      this.LogDebug ( "PreFilledColumnList: " + this.Session.EntityField.Table.PreFilledColumnList );

      for ( int iCol = 0; iCol < 10; iCol++ )
      {
        String indexCol = ( iCol + 1 ).ToString ( );
        EdRecordTableHeader header = this.Session.EntityField.Table.Header [ iCol ];

        if ( header.Text != String.Empty )
        {
          this.LogDebug ( "TypeId {0}.", header.TypeId );

          var typeId = header.TypeId;

          if ( typeId == EvDataTypes.Read_Only_Text )
          {
            typeId = EvDataTypes.Text;
          }
          else
          {
            typeId = EvDataTypes.Read_Only_Text;
          }
          this.LogDebug ( "typeId {0}." + typeId );

          groupField.Table.Header [ iCol ].Text = header.Text;
          groupField.Table.Header [ iCol ].TypeId = typeId;
          groupField.Table.Header [ iCol ].Width = header.Width;


          if ( groupField.Table.Header [ iCol ].TypeId == EvDataTypes.Radio_Button_List
            || groupField.Table.Header [ iCol ].TypeId == EvDataTypes.Selection_List )
          {
            groupField.Table.Header [ iCol ].OptionList =
               Evado.Digital.Model.EvcStatics.getStringAsOptionList ( this.Session.EntityField.Table.Header [ iCol ].OptionsOrUnit );
          }

          if ( groupField.Table.Header [ iCol ].TypeId == EvDataTypes.Special_Matrix )
          {
            groupField.Table.Header [ iCol ].TypeId = EvDataTypes.Text;
          }
        }
      }

      //
      // Iterate through the rows in the matrix.
      //
      foreach ( EdRecordTableRow row in this.Session.EntityField.Table.Rows )
      {
        Evado.Model.UniForm.TableRow groupRow = new Evado.Model.UniForm.TableRow ( );

        //
        // iterate through the columns in row.
        //
        for ( int i = 0; i < 10; i++ )
        {
          groupRow.Column [ i ] = row.Column [ i ];
        }
        groupField.Table.Rows.Add ( groupRow );

      }//END row interation.

      this.LogMethodEnd ( "creatMatrixFieldGroup" );
    }//END creatMatrixFieldGroup Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region create object methods.

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Evado.Model.UniForm.AppData</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData createObject ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "createObject" );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
        int order = 0;
        int initialVersion = (int) this.Session.EntityLayout.Design.Version;

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "createObject",
          this.Session.UserProfile );

        // 
        // Initialise the methods variables and objects.
        //    
        string FormId = PageCommand.GetParameter ( EdRecord.FieldNames.Layout_Id );
        string stCount = PageCommand.GetParameter ( EuEntityFields.CONST_FIELD_COUNT );
        string stSectionNo = PageCommand.GetParameter ( EuEntityFields.CONST_FORM_SECTION );

        this.LogValue ( "FormId: " + FormId );
        this.LogValue ( "Count: " + stCount );
        this.LogValue ( "stSectionNo: " + stSectionNo );

        if ( stCount != String.Empty )
        {
          if ( int.TryParse ( stCount, out order ) == false )
          {
            order = 0;
          }
        }

        if ( stSectionNo == String.Empty )
        {
          stSectionNo = "-1";
        }

        order = order * 2 + 1;

        this.Session.EntityField = new Evado.Digital.Model.EdRecordField ( );
        this.Session.EntityField.Guid = Evado.Digital.Model.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.EntityField.LayoutGuid = this.Session.EntityLayout.Guid;
        this.Session.EntityField.LayoutId = this.Session.EntityLayout.LayoutId;
        this.Session.EntityField.Design.InitialVersion = initialVersion;
        this.Session.EntityField.Order = order;
        this.Session.EntityField.Design.SectionNo = EvStatics.getInteger ( stSectionNo );
        this.LogValue ( "Field.InitialVersion: " + this.Session.EntityField.Design.InitialVersion );
        this.LogValue ( "Field.LayoutGuid: " + this.Session.EntityField.LayoutGuid );
        this.LogValue ( "Field.LayoutId: " + this.Session.EntityField.LayoutId );
        // 
        // Generate the new page layout 
        // 
        this.getDataObject ( clientDataObject );

        // 
        // Save the customer object to the session
        // 


        this.LogValue ( "Exit createObject method. ID: "
          + clientDataObject.Id
          + ", Title: " + clientDataObject.Title );

        // 
        // Return the ResultData object.
        // 
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Record_Create_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return null;

    }//END method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Update object methods.

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateObject ( Evado.Model.UniForm.Command PageCommand )
    {
      try
      {
        this.LogMethod ( "updateObject" );
        this.LogValue ( "Parameter PageEvado.Model.UniForm.Command: "
          + PageCommand.getAsString ( false, false ) );

        this.LogValue ( "SessionObjects.FormFields" );
        this.LogValue ( "Guid: " + this.Session.EntityField.Guid );
        this.LogValue ( "FormId: " + this.Session.EntityField.LayoutId );
        this.LogValue ( "Title: " + this.Session.EntityField.Title );

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );

        //
        // If is selected is true this is a new field set the guid to empty.
        //
        if ( this.Session.EntityField.Guid == Evado.Digital.Model.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.EntityField.Guid = Guid.Empty;
          this.Session.EntityLayout.Fields = new List<EdRecordField> ( );
        }

        // 
        // As Evado eClinical does not have a delete record function return true if groupCommand sent.
        // 
        if ( PageCommand.Method == Evado.Model.UniForm.ApplicationMethods.Delete_Object )
        {
          return this.Session.LastPage;
        }

        // 
        // Update the object.
        // 
        this.updateObjectValues ( PageCommand );

        this.updateTableValues ( PageCommand );

        this.updateMatrixValues ( PageCommand );

        //
        // Update the layout identifier to match the entity layout.
        //
        this.Session.EntityField.LayoutId = this.Session.EntityLayout.LayoutId;

        //
        // Validate that the field identifier is valid.
        //
        if ( this.validateFieldId ( PageCommand ) == false )
        {
          this.Session.LastPage.Message = this.ErrorMessage;
          return this.Session.LastPage;
        }

        if ( this.Session.EntityField.RecordMedia != null )
        {
          this.LogDebug ( "Media URL {0} T: {1}, W: {2}, H: {3}.",
            this.Session.EntityField.RecordMedia.Url,
            this.Session.EntityField.RecordMedia.Title,
            this.Session.EntityField.RecordMedia.Width,
            this.Session.EntityField.RecordMedia.Height );

          this.LogDebug ( "Media Data {0}.", this.Session.EntityField.RecordMedia.Data );
        }

        //
        // Add the new entity to the field list.
        //
        this.Session.EntityLayout.Fields.Add ( this.Session.EntityField );

        // 
        // Execute the save record groupCommand to save the record values to the 
        // Evado database.
        // 
        EvEventCodes result = this._Bll_EntityFields.SaveItem (
          this.Session.EntityField );

        this.LogValue ( this._Bll_EntityFields.Log );

        // 
        // If an error state is returned log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          if ( result == EvEventCodes.Data_Duplicate_Field_Error )
          {
            this.ErrorMessage = String.Format ( EdLabels.Form_Field_Duplicate_ID_Error_Message,
              this.Session.EntityField.FieldId );
          }
          string StEvent = this._Bll_EntityFields.Log + " returned error message: " + Evado.Digital.Model.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          switch ( result )
          {
            case EvEventCodes.Data_Duplicate_Id_Error:
              {
                this.ErrorMessage = EdLabels.Form_Field_Duplicate_ID_Error_Message;
                break;
              }
            default:
              {
                this.ErrorMessage = EdLabels.Form_Field_Update_Error_Message;
                break;
              }
          }
          return this.Session.LastPage;
        }

        return new Evado.Model.UniForm.AppData ( );

      }
      catch ( Exception Ex )
      {
        // 
        // If an exception is raised create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Form_Field_Update_Error_Message;

        // 
        // Log the error event to the server log and DB event log.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END method

    // ==================================================================================
    /// <summary>
    /// THis method updates the form record values with the groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command objects.</param>
    /// <returns>Bool: True is field ID is validate.</returns>
    //  ----------------------------------------------------------------------------------

    private bool validateFieldId ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "validateFieldId" );
      this.LogValue ( "FormField.Guid: " + this.Session.EntityField.Guid + ", FieldId: " + this.Session.EntityField.FieldId );

      //
      // Replace periods in the field identifier with underscore
      //
      this.Session.EntityField.FieldId = this.Session.EntityField.FieldId.Replace ( " ", "_" );

      //
      // Iterate through the form fields checking to ensure there are not duplicate field identifiers.
      //
      foreach ( EdRecordField field in this.Session.EntityLayout.Fields )
      {
        this.LogValue ( "FormField.Guid: " + field.Guid + ", FieldId: " + field.FieldId );

        if ( field.FieldId == this.Session.EntityField.FieldId
          && this.Session.EntityField.Guid != field.Guid )
        {
          this.ErrorMessage = String.Format ( EdLabels.Form_Field_Duplicate_ID_Error_Message,
            this.Session.EntityField.FieldId );

          this.LogValue ( this.ErrorMessage );
          return false;
        }
      }//END iteration statement

      return true;
    }//END validateFieldId method.

    // ==================================================================================
    /// <summary>
    /// THis method updates the form record values with the groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command objects.</param>
    //  ----------------------------------------------------------------------------------
    private void updateObjectValues (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogValue ( "PageCommand.Parameters List.Count: " + PageCommand.Parameters.Count );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
      {
        this.LogTextStart ( parameter.Name + " > " + parameter.Value );

        if ( parameter.Name.Contains ( Evado.Digital.Model.EvcStatics.CONST_GUID_IDENTIFIER ) == false
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          && parameter.Name != Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION
          && parameter.Name != EuRecordLayouts.CONST_REFRESH )
        {
          this.LogText ( " >> UPDATED" );
          try
          {
            EdRecordField.ClassFieldNames fieldName = Evado.Model.EvStatics.parseEnumValue<EdRecordField.ClassFieldNames> ( parameter.Name );

            this.Session.EntityField.setValue ( fieldName, parameter.Value );

          }
          catch ( Exception Ex )
          {
            this.LogException ( Ex );
          }
        }
        this.LogTextEnd ( "" );

      }// End iteration loop

    }//END updateObjectValue method.

    // ==================================================================================
    /// <summary>
    /// THis method updates the form field table values with the groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command objects.</param>
    //  ----------------------------------------------------------------------------------
    private void updateTableValues ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogValue ( Evado.Model.UniForm.EuStatics.CONST_METHOD_START
        + "Evado.UniForm.Clinical.FormFields.updateTableValues" );

      //
      // If the ResultData type is not a table or matrix exit.
      //
      if ( this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Table
        && this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Special_Matrix )
      {
        this.LogValue ( "Not a table or matix so exit" );
        return;
      }

      //
      // if the table object is null initialise it.
      //
      if ( this.Session.EntityField.Table == null )
      {
        this.LogValue ( "Re initialised the table" );
        this.Session.EntityField.Table = new EdRecordTable ( );
      }

      // 
      // Iterate through the parameter values updating the ResultData object
      //
      string stRowCount = PageCommand.GetParameter ( EuEntityFields.CONST_TABLE_ROW_FIELD );

      this.LogValue ( "stRowCount: " + stRowCount );

      int iRowCount = Evado.Digital.Model.EvcStatics.getInteger ( stRowCount );

      this.Session.EntityField.Table.SetRowCount ( iRowCount );

      this.LogValue ( "Table.Rows.Count: " + this.Session.EntityField.Table.Rows.Count );

      //
      // Iterate through the table headers.
      //
      for ( int iCol = 0; iCol < this.Session.EntityField.Table.Header.Length; iCol++ )
      {
        String stParameterName = EuEntityFields.CONST_TABLE_FIELD_ID + "_" + ( iCol + 1 );

        this.LogValue ( "stParameterName: " + stParameterName );

        //
        // Set the column header .
        //
        this.Session.EntityField.Table.Header [ iCol ].Text =
          PageCommand.GetParameter ( stParameterName + "_2" );

        //
        // Set the column width.
        //
        this.Session.EntityField.Table.Header [ iCol ].Width =
          PageCommand.GetParameter ( stParameterName + "_3" );

        //
        // Set the column date type.
        //
        this.Session.EntityField.Table.Header [ iCol ].TypeId =
          PageCommand.GetParameter<EvDataTypes> ( stParameterName + "_4" );

        this.LogValue ( "Text: " + this.Session.EntityField.Table.Header [ iCol ].Text );

        //
        // Set the column date type.
        //
        this.Session.EntityField.Table.Header [ iCol ].OptionsOrUnit =
          PageCommand.GetParameter ( stParameterName + "_5" );

        this.LogValue ( "Text: " + this.Session.EntityField.Table.Header [ iCol ].Text );

      }// End iteration loop

    }//END updateTableValues method.

    // ==================================================================================
    /// <summary>
    /// THis method updates the form field table values with the groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command objects.</param>
    //  ----------------------------------------------------------------------------------
    private void updateMatrixValues ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogValue ( Evado.Model.UniForm.EuStatics.CONST_METHOD_START
        + "Evado.UniForm.Clinical.FormFields.updateMatrixValues" );

      //
      // If the ResultData type is not a table or matrix exit.
      //
      if ( this.Session.EntityField.TypeId != Evado.Model.EvDataTypes.Special_Matrix )
      {
        this.LogValue ( "Not a matix so exit" );
        return;
      }

      // 
      // Get the list of prefilled columns
      //
      this.Session.EntityField.Table.PreFilledColumnList =
        PageCommand.GetParameter ( EuEntityFields.CONST_MATIX_COL_FIELD );
      this.LogValue ( "PreFilledColumnList: " + this.Session.EntityField.Table.PreFilledColumnList );

      //
      // Iterate through the table rows.
      //
      for ( int iRow = 0; iRow < this.Session.EntityField.Table.Rows.Count; iRow++ )
      {
        String indexRow = ( iRow + 1 ).ToString ( );

        //
        // Iterate through the table columns.
        //
        for ( int iCol = 0; iCol < this.Session.EntityField.Table.Header.Length; iCol++ )
        {
          String indexCol = ( iCol + 1 ).ToString ( );
          String stParmeterName = EuEntityFields.CONST_MATRIX_FIELD_ID + "_" + indexRow + "_" + indexCol;

          //
          // if the column is included in the prefil list add the value to the table.
          //
          if ( this.Session.EntityField.Table.PreFilledColumnList.Contains ( indexCol ) == true
            || this.Session.EntityField.Table.Header [ iCol ].TypeId == EvDataTypes.Special_Matrix )
          {
            String stValue = PageCommand.GetParameter ( stParmeterName );

            this.LogValue ( stParmeterName + ": " + stValue );

            this.Session.EntityField.Table.Rows [ iRow ].Column [ iCol ] = stValue;
          }
          else
          {
            this.Session.EntityField.Table.Rows [ iRow ].Column [ iCol ] = String.Empty;
          }

        }// End column iteration loop

      }//END row iteration loop

    }//END updateTableValues method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }
}//END namespace