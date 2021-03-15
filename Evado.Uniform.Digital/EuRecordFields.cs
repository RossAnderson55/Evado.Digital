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

using Evado.Bll;
using Evado.Model;
using Evado.Bll.Digital;
using  Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Digital
{
  /// <summary>
  /// This class defines the application base classs that is used to terminate the 
  /// hosted application objects.
  /// 
  /// This class terminates the Customer object.
  /// </summary>
  public class EuRecordFields : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuRecordFields ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuFormFields.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuRecordFields (
      EuGlobalObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuFormFields.";
      this.AdapterObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;


      this.LogInitMethod ( "EuFormFields initialisation" );
      this.LogInit ( "-ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "-SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "-SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "-UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );

      this.LogInit ( "-Settings.LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-Settings.UserProfile.UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-Settings.UserProfile.CommonName: " + Settings.UserProfile.CommonName );

      this._Bll_FormFields = new EdRecordFields ( Settings );

    }//END Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private Evado.Bll.Digital.EdRecordFields _Bll_FormFields = new Evado.Bll.Digital.EdRecordFields ( );

    public const string CONST_REFRESH = "RFSH";
    public const string CONST_TABLE_ROW_FIELD = "ROWS";
    public const string CONST_TABLE_FIELD_ID = "TBL";
    public const string CONST_MATIX_COL_FIELD = "COLS";
    public const string CONST_MATRIX_FIELD_ID = "MAT";
    public const string CONST_FIELD_COUNT = "FFC";
    public const string CONST_FORM_SECTION = "FSTM";

    public const string CONST_VIDEO_IMAGE_WIDTH = "VIW";
    public const string CONST_VIDEO_IMAGE_HEIGHT = "VIH";

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
      this.LogMethod( "getClientDataObject" );
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
        if ( PageCommand.hasParameter ( Model.UniForm.CommandParameters.Create_Object ) == true )
        {
          string stPageType = PageCommand.GetParameter ( Model.UniForm.CommandParameters.Create_Object );

          if ( stPageType == "1" )
          {
            PageCommand.Method = Model.UniForm.ApplicationMethods.Create_Object;
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
      this.LogMethod ( "getFormObject" );
     this.LogValue ( "LoggingLevel: " + this.LoggingLevel );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid formFieldGuid = Guid.Empty;
      //bool refreshPage = false;

      try
      {
        // 
        // Retrieve the record guid from the parameters
        // 
        formFieldGuid = PageCommand.GetGuid ( );
        this.LogValue ( " formGuid: " + formFieldGuid );

        // 
        // if the guid is empty the parameter was not found to exit.
        // 
        if ( formFieldGuid == Guid.Empty )
        {
          this.LogValue ( "recordGuid is EMPTY" );

          return null;
        }

        //
        // If the field has changed then get a new instance of the field.
        //
        if ( this.Session.RecordField.Guid != formFieldGuid )
        {
          // 
          // Retrieve the record object from the database via the DAL and BLL layers.
          // 
          this.Session.RecordField = this._Bll_FormFields.getField ( formFieldGuid );

          this.LogClass ( this._Bll_FormFields.Log );

          this.LogValue ( " SessionObjects.Record.FieldId: " + this.Session.RecordField.FieldId );
        }



        //
        // Extract the ResultData type to validated if it has been changed.
        //
        String stDataType = PageCommand.GetParameter (
          EdRecordField.FieldClassFieldNames.TypeId.ToString ( ) );
        string value = PageCommand.GetParameter ( EuRecordLayouts.CONST_REFRESH );

        //
        // Refresh the page values on the server, used for changing the field type setting
        // to enable the relevant groups.
        //
        if ( ( stDataType != String.Empty
          && stDataType != this.Session.RecordField.TypeId.ToString ( ) )
          || value == "1" )
        {
          this.LogValue ( "Data Type has changed." );

          this.updateObjectValues ( PageCommand );

          this.updateVideoImageValues ( PageCommand );

          this.updateTableValues ( PageCommand );

          this.updateMatrixValues ( PageCommand );

          this.validateFieldId ( PageCommand );
        }
        if ( this.Session.RecordField.TypeId == EvDataTypes.Radio_Button_List )
        {
          this.LogDebug ( "Is Options numeric: " + this.Session.RecordField.Design.hasNumericValues );
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

      return null;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method creates the group commands
    /// </summary>
    //  ------------------------------------------------------------------------------
    private void getGroupCommands (
      Evado.Model.UniForm.Group PageGroup)
    {
      this.LogMethod ( "getGroupCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      Evado.Model.UniForm.Command groupCommand = new Model.UniForm.Command ( );

      //
      // Add the page refresh groupCommand.
      //
      groupCommand = PageGroup.addCommand (
        EdLabels.Form_Field_Refresh_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Record_Layout_Fields.ToString ( ),
        Model.UniForm.ApplicationMethods.Custom_Method );

      groupCommand.setCustomMethod ( Model.UniForm.ApplicationMethods.Get_Object );
      groupCommand.SetGuid ( this.Session.RecordField.Guid );
      groupCommand.AddParameter ( EuRecordLayouts.CONST_REFRESH, "1" );

      //
      // Edit unless the page is in edit mode
      //
      if ( PageGroup.EditAccess != Evado.Model.UniForm.EditAccess.Enabled )
      {
        return;
      }

      //
      // Add the save groupCommand for the page.
      //
      switch ( this.Session.RecordLayout.State )
      {
        case EdRecordObjectStates.Form_Draft:
        case EdRecordObjectStates.Form_Reviewed:
          {
            //
            // Add the same groupCommand.
            //
            groupCommand = PageGroup.addCommand (
              EdLabels.Form_Field_Save_Command_Title,
              EuAdapter.ADAPTER_ID,
              EuAdapterClasses.Record_Layout_Fields.ToString ( ),
              Evado.Model.UniForm.ApplicationMethods.Save_Object );

            // 
            // Define the save groupCommand parameters.
            // 
            groupCommand.SetGuid ( this.Session.RecordField.Guid );

            groupCommand.AddParameter (
              Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
             EdRecordFields.Action_Save );

            //
            // Add the same groupCommand.
            //
            int majorVersion = ( int ) this.Session.RecordLayout.Design.Version;
            if ( this.Session.RecordField.Design.InitialVersion == majorVersion )
            {
              groupCommand = PageGroup.addCommand (
                EdLabels.Form_Field_Delete_Command_Title,
                EuAdapter.ADAPTER_ID,
                EuAdapterClasses.Record_Layout_Fields.ToString ( ),
                Evado.Model.UniForm.ApplicationMethods.Save_Object );
              // 
              // Define the save groupCommand parameters.
              // 
              groupCommand.SetGuid ( this.Session.RecordField.Guid );

              groupCommand.AddParameter (
                Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
               EdRecordFields.Action_Delete );
            }

            return;
          }
        default:
          {
            return;
          }
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

      // 
      // Initialise the client ResultData object.
      // 
      ClientDataObject.Id = this.Session.RecordField.Guid;
      ClientDataObject.Title = EdLabels.Form_Field_Page_Title_Prefix
        + this.Session.RecordField.FieldId
        + EdLabels.Space_Hypen
        + this.Session.RecordField.Title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.PageDataGuid = ClientDataObject.Id;
      ClientDataObject.Page.PageId = this.Session.RecordField.FieldId;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Set the user's edit access if they have configuration edit access.
      //
      this.LogValue ( "HasConfigrationEditAccess: " + this.Session.UserProfile.hasManagementAccess );

      if ( this.Session.UserProfile.hasManagementAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      this.LogValue ( "Field TypeId: " + this.Session.RecordField.TypeId );

      //
      // create the field general group.
      //
      this.createFieldGeneral_Group ( ClientDataObject.Page );

      //
      // create the field properties group.
      //
      this.createFieldProperties_Group ( ClientDataObject.Page );

      //
      // add a pageMenuGroup with fields for custom field validations.
      //
      if ( this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Computed_Field )
      {
        this.Session.RecordField.Design.Mandatory = false;

        this.createComputedScriptGroup ( ClientDataObject.Page );

        return;
      }

      //
      // display the field validation pageMenuGroup
      //
      this.createFieldValidationGroup ( ClientDataObject.Page );

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
      this.LogMethod ( "createFieldGeneral_Group" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      List<EvOption> optionList = new List<EvOption> ( );
      int formVersion = (int) this.Session.RecordLayout.Design.Version;
      bool bEditType = this.Session.RecordField.Design.InitialVersion == formVersion;

      this.LogValue ( "formVersion: " + formVersion );
      this.LogValue ( "Field.InitialVersion: " + this.Session.RecordField.Design.InitialVersion );

      this.LogValue ( "EditType: " + bEditType );
      this.LogValue ( "Form.Design.Section: " + this.Session.RecordField.Design.SectionNo );

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Form_Field_General_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.SetCommandBackBroundColor (
        Model.UniForm.GroupParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      pageGroup.EditAccess = PageObject.EditAccess;
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
        this.Session.RecordLayout.Title );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Form field id
      //
      groupField = pageGroup.createTextField (
        EdRecordField.FieldClassFieldNames.FieldId.ToString ( ),
        EdLabels.Form_Field_Identifier_Field_Label,
        this.Session.RecordField.FieldId,
        20 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      if ( bEditType == false )
      {
        groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }

      //
      // Create the form field type selection.
      //
      optionList = EdRecordField.getDataTypes (  );

      groupField = pageGroup.createSelectionListField (
        EdRecordField.FieldClassFieldNames.TypeId.ToString ( ),
        EdLabels.Form_Field_Type_Field_Label,
        this.Session.RecordField.TypeId.ToString ( ),
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      if ( bEditType == false )
      {
        groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }
      else
      {
        groupField.AddParameter ( Model.UniForm.FieldParameterList.Snd_Cmd_On_Change, 1 );
      }

      //
      // Form title
      //
      groupField = pageGroup.createTextField (
        EdRecordField.FieldClassFieldNames.Subject.ToString ( ),
        EdLabels.Form_Field_Subject_Field_Label,
        this.Session.RecordField.Design.Title,
        150 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Create the Section list list,
      //
      optionList = new List<EvOption> ( );
      optionList.Add ( new EvOption ( ) );

      foreach ( EdRecordSection section in this.Session.RecordLayout.Design.FormSections )
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
          EdRecordField.FieldClassFieldNames.FormSection.ToString ( ),
          EdLabels.Form_Field_Section_Field_Label,
          this.Session.RecordField.Design.SectionNo,
          optionList );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      //
      // Create the form field layout selection.
      //
      optionList = EvStatics.getOptionsFromEnum ( typeof ( Evado.Model.UniForm.FieldLayoutCodes ), true );


      if ( this.Session.RecordField.Design.FieldLayout == null )
      {
        this.Session.RecordField.Design.FieldLayout = this.Session.RecordLayout.Design.DefaultPageLayout;
      }

      groupField = pageGroup.createSelectionListField (
        EdRecordField.FieldClassFieldNames.Field_Layout.ToString ( ),
        EdLabels.LayoutField_Field_Layout_Field_Label,
        this.Session.RecordField.Design.FieldLayout.ToString ( ),
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      this.LogMethodEnd ( "createFieldGeneral_Group" );

    }//END createFieldGeneral_Group Method

    // ==============================================================================
    /// <summary>
    /// This method generates the general pageMenuGroup for the field properties.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void createFieldProperties_Group (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "createFieldProperties_Group" );

      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Parameter parameter = new Evado.Model.UniForm.Parameter ( );
      List<EvOption> optionList = new List<EvOption> ( );
      int formVersion = (int) this.Session.RecordLayout.Design.Version;
      bool bEditType = this.Session.RecordField.Design.InitialVersion == formVersion;

      //
      // Define the general properties pageMenuGroup.
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Form_Field_Properties_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;


      //
      // Form field order
      //
      groupField = pageGroup.createNumericField (
        EdRecordField.FieldClassFieldNames.Order.ToString ( ),
        EdLabels.Form_Field_Order_Field_Label,
        this.Session.RecordField.Order,
        0,
        200 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form field static text Fill the instruction field with text
      //
      if ( this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Read_Only_Text )
      {
        groupField = pageGroup.createFreeTextField (
          EdRecordField.FieldClassFieldNames.Instructions.ToString ( ),
          EdLabels.Form_Field_Read_Only_Text_Field_Label,
          this.Session.RecordField.Design.Instructions,
          150, 20 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        String instructions = EdLabels.Form_Field_Instruction_Field_Description;

        groupField.Description = instructions;
        this.Session.RecordField.Design.AiDataPoint = false;
        this.Session.RecordField.Design.Mandatory = false;
        this.Session.RecordField.Design.Mandatory = false;

        //
        // Form hide hidden field
        //
        groupField = pageGroup.createBooleanField (
          EdRecordField.FieldClassFieldNames.Hide_Field.ToString ( ),
          EdLabels.Form_Field_Hide_Field_Label,
          this.Session.RecordField.Design.HideField );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        return;
      }//END read only field.

      //
      // This block displays the fields needed to define an streamed video or external image
      //
      if ( this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.External_Image
        || this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Streamed_Video )
      {
        groupField = pageGroup.createTextField (
          EdRecordField.FieldClassFieldNames.Instructions.ToString ( ),
          EdLabels.Form_Field_External_URL_Field_Label,
          this.Session.RecordField.Design.Instructions,
          100 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        this.Session.RecordField.Design.AiDataPoint = false;
        this.Session.RecordField.Design.Mandatory = false;

        int iWidth = 0;
        int iHeight = 0;
        if ( this.Session.RecordField.Design.JavaScript != String.Empty )
        {
          String [ ] arParms = this.Session.RecordField.Design.JavaScript.Split ( ';' );
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
        //
        // Form field image or video width
        //
        groupField = pageGroup.createNumericField (
          EuRecordFields.CONST_VIDEO_IMAGE_WIDTH,
          EdLabels.Form_Field_Image_Video_Width_Field_Label,
          iWidth,
          0,
          1000 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
        //
        // Form field image or video height
        //
        groupField = pageGroup.createNumericField (
          EuRecordFields.CONST_VIDEO_IMAGE_HEIGHT,
          EdLabels.Form_Field_Image_Video_Height_Field_Label,
          iHeight,
          0,
          1000 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        //
        // Form hide hidden field
        //
        groupField = pageGroup.createBooleanField (
          EdRecordField.FieldClassFieldNames.Hide_Field.ToString ( ),
          EdLabels.Form_Field_Hide_Field_Label,
          this.Session.RecordField.Design.HideField );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        this.Session.RecordField.Design.AiDataPoint = false;
        this.Session.RecordField.Design.Mandatory = false;

        return;
      }

      //
      // Form Instructions
      //
      groupField = pageGroup.createFreeTextField (
        EdRecordField.FieldClassFieldNames.Instructions.ToString ( ),
        EdLabels.Form_Field_Instructions_Field_Label,
        this.Session.RecordField.Design.Instructions,
        150, 5 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form reference
      //
      groupField = pageGroup.createTextField (
        EdRecordField.FieldClassFieldNames.Reference.ToString ( ),
        EdLabels.Form_Field_Reference_Field_Label,
        this.Session.RecordField.Design.HttpReference,
        50 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form field category
      //
      if ( this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Signature
        && this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Password )
      {
        groupField = pageGroup.createTextField (
          EdRecordField.FieldClassFieldNames.FieldCategory.ToString ( ),
          EdLabels.Form_Field_Category_Field_Title,
          this.Session.RecordField.Design.FieldCategory,
          20 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      //
      // Create the external selection list,
      //
      if ( this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.External_Selection_List )
      {/*
        optionList = EvFormField.getDataTypes ( false );

        groupField = pageGroup.createSelectionListField (
          EvFormField.FormFieldClassFieldNames.ExSelectionListId.ToString ( ),
          EdLabels.Form_Field_External_Selection_Field_Label,
          this.SessionObjects.FormField.Design.ExSelectionListId,
          optionList );
        groupField.Layout = PageGenerator.ApplicationFieldLayout;

        optionList = EvFormField.getDataTypes (false  );

        groupField = pageGroup.createSelectionListField (
          EvFormField.FormFieldClassFieldNames.ExSelectionListCategory.ToString ( ),
          EdLabels.Form_Field_External_Selection_Category_Field_Label,
          this.SessionObjects.FormField.Design.ExSelectionListCategory,
          optionList );
        groupField.Layout = PageGenerator.ApplicationFieldLayout;
      */
      }

      //
      // Form field selection options field.
      //
      if ( this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Analogue_Scale )
      {
        groupField = pageGroup.createTextField (
          EdRecordField.FieldClassFieldNames.AnalogueLegendStart.ToString ( ),
          EdLabels.Form_Field_Analogue_Legend_Start_Field_Label,
          this.Session.RecordField.Design.AnalogueLegendStart,
          50 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        groupField = pageGroup.createTextField (
          EdRecordField.FieldClassFieldNames.AnalogueLegendFinish.ToString ( ),
          EdLabels.Form_Field_Analogue_Legend_Start_Field_Label,
          this.Session.RecordField.Design.AnalogueLegendFinish,
          50 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      //
      // Form field selection options field.
      //
      if ( this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Selection_List
        || this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Radio_Button_List
        || this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Check_Box_List
        || this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Horizontal_Radio_Buttons )
      {
        string stOptions = this.Session.RecordField.Design.Options.Replace ( ";", ";\r\n" );

        groupField = pageGroup.createFreeTextField (
          EdRecordField.FieldClassFieldNames.Selection_Options.ToString ( ),
          EdLabels.Form_Field_Selection_Options_Field_Title,
          EdLabels.Form_Field_Selection_Options_Field_Description,
          stOptions,
          90,
          10 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }


      //
      // Form summary field  field
      //
      if ( this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Read_Only_Text
        && this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Free_Text
        && this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Password
        && this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Signature
        && this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Table
        && this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Special_Matrix )
      {
        groupField = pageGroup.createBooleanField (
          EdRecordField.FieldClassFieldNames.Summary_Field.ToString ( ),
          EdLabels.Form_Field_Summary_Field_Label,
          this.Session.RecordField.Design.IsSummaryField );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      //
      // Form hide mandatory field
      //
      if ( this.Session.RecordField.isReadOnly == false )
      {
        groupField = pageGroup.createBooleanField (
          EdRecordField.FieldClassFieldNames.Mandatory.ToString ( ),
          EdLabels.Form_Field_Mandatory_Field_Label,
          this.Session.RecordField.Design.Mandatory );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        //
        // Form hide ResultData point field
        //
        if ( this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Free_Text
          && this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Password
          && this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Signature )
        {
          groupField = pageGroup.createBooleanField (
            EdRecordField.FieldClassFieldNames.AI_Data_Point.ToString ( ),
            EdLabels.LayoutFields_Enable_AI_Field_Label,
            this.Session.RecordField.Design.AiDataPoint );
          groupField.Layout = EuAdapter.DefaultFieldLayout;
        }
      }
      //
      // Form hide hidden field
      //
      groupField = pageGroup.createBooleanField (
        EdRecordField.FieldClassFieldNames.Hide_Field.ToString ( ),
        EdLabels.Form_Field_Hide_Field_Label,
        this.Session.RecordField.Design.HideField );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      this.LogMethodEnd ( "createFieldProperties_Group" );
    }//END createFieldPropertiesGroup Method

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
      if ( this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Text
        || this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Read_Only_Text
        || this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Free_Text
        || this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Password
        || this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Signature
        || this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Computed_Field )
      {
        return;
      }

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Form_Field_Custom_Validation_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = PageObject.EditAccess;
      //
      // Add the group commands
      //
      this.getGroupCommands ( pageGroup );


      pageGroup.Description = EdLabels.Form_Field_CustomValidation_Field_Description ;

      //
      // Form Instructions
      //
      groupField = pageGroup.createFreeTextField (
        EdRecordField.FieldClassFieldNames.Java_Script.ToString ( ),
        String.Empty,
        this.Session.RecordField.Design.JavaScript,
        80, 10 );
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

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
      if ( this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Computed_Field )
      {
        return;
      }

      //
      // Define the general properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Form_Field_Computed_Field_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;

      pageGroup.EditAccess = PageObject.EditAccess;

      //
      // Add the group commands
      //
      this.getGroupCommands ( pageGroup );

      pageGroup.Description =  "computedFields(){\r\n"
        + "var value = 0;\r\n"
        + "var field;\r\n"
        + "var fieldDisp;\r\n"
        + "\r\n computed script code.\r\n"
        + "\r\n}"
        + "\r\nThe computed script field.value is set to the value of 'value'." ;

      //
      // Form Instructions
      //
      groupField = pageGroup.createFreeTextField (
        EdRecordField.FieldClassFieldNames.Java_Script.ToString ( ),
        String.Empty,
        this.Session.RecordField.Design.JavaScript,
        80, 10 );
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

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
      this.LogValue ( "TypeId: " + this.Session.RecordField.TypeId );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // If a static field field exit the method.
      //
      if ( this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Read_Only_Text
        || this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Password
        || this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Signature
        || this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.Streamed_Video
        || this.Session.RecordField.TypeId == Evado.Model.EvDataTypes.External_Image )
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
      this.LogValue ( "TypeId: " + this.Session.RecordField.TypeId );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // If not a numeric field exit the method.
      //
      if ( this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Numeric
        && this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Integer_Range
        && this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Float_Range )
      {
        return;
      }

      //
      // Define the Form_Field_Sex_Validation_Group_Title properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Form_Field_Numeric_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = PageObject.EditAccess;

      //
      // Add the group commands
      //
      this.getGroupCommands ( pageGroup );

      //
      // form field numeric scaling value.
      //
      optionList =  Evado.Model.Digital.EvcStatics.getStringAsOptionList ( "-12:-12;-9:-9;_6:-6;-3:-3;0;3:3;6:6;9:9;12:12" ); ;

      groupField = pageGroup.createSelectionListField (
        EdRecordField.FieldClassFieldNames.Unit_Scaling.ToString ( ),
        EdLabels.Form_Field_Unit_Scale_Field_Label,
        this.Session.RecordField.Design.UnitScaling,
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // form field numeric unit
      //
      groupField = pageGroup.createTextField (
        EdRecordField.FieldClassFieldNames.Unit.ToString ( ),
        EdLabels.Form_Field_Unit_Scale_Field_Label,
        this.Session.RecordField.Design.Unit,
        10 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form lower validation range value 
      //
      groupField = pageGroup.createNumericField (
        EdRecordField.FieldClassFieldNames.DefaultNumericValue.ToString ( ),
        EdLabels.Form_Field_Default_Value_Field_Label,
        0,
        -1000000,
        1000000 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.Value = this.Session.RecordField.Design.DefaultValue;

      if ( groupField.Value ==  Evado.Model.Digital.EvcStatics.CONST_NUMERIC_NULL.ToString ( ) )
      {
        groupField.Value =  Evado.Model.Digital.EvcStatics.CONST_NUMERIC_NOT_AVAILABLE;
      }

      //
      // Form lower validation range value 
      //
      groupField = pageGroup.createNumericField (
        EdRecordField.FieldClassFieldNames.ValidationLowerLimit.ToString ( ),
        EdLabels.Form_Field_Lower_Validation_Field_Label,
        this.Session.RecordField.Design.ValidationLowerLimit,
        -1000000,
        1000000 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form upper validation range value 
      //
      groupField = pageGroup.createNumericField (
        EdRecordField.FieldClassFieldNames.ValidationUpperLimit.ToString ( ),
        EdLabels.Form_Field_Upper_Validation_Field_Label,
        this.Session.RecordField.Design.ValidationUpperLimit,
        -1000000,
        1000000 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form lower Alert range value 
      //
      groupField = pageGroup.createNumericField (
        EdRecordField.FieldClassFieldNames.AlertLowerLimit.ToString ( ),
        EdLabels.Form_Field_Lower_Alert_Field_Label,
        this.Session.RecordField.Design.AlertLowerLimit,
        -1000000,
        1000000 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form upper Alert range value 
      //
      groupField = pageGroup.createNumericField (
        EdRecordField.FieldClassFieldNames.AlertUpperLimit.ToString ( ),
        EdLabels.Form_Field_Upper_Alert_Field_Label,
        this.Session.RecordField.Design.AlertUpperLimit,
        -1000000,
        1000000 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      //
      // Form lower Alert range value 
      //
        groupField = pageGroup.createNumericField (
          EdRecordField.FieldClassFieldNames.NormalRangeLowerLimit.ToString ( ),
          EdLabels.Form_Field_Lower_Normal_Range_Field_Label,
          this.Session.RecordField.Design.NormalRangeLowerLimit,
          -1000000,
          1000000 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        //
        // Form upper Alert range value 
        //
        groupField = pageGroup.createNumericField (
          EdRecordField.FieldClassFieldNames.NormalRangeUpperLimit.ToString ( ),
          EdLabels.Form_Field_Upper_Normal_Range_Field_Label,
          this.Session.RecordField.Design.NormalRangeUpperLimit,
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
      this.LogValue ( "Field TypeId: " + this.Session.RecordField.TypeId );

      //
      // If the ResultData type is not a table or matrix exit.
      //
      if ( this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Table
        && this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Special_Matrix )
      {
        this.LogValue ( "Not a table or matix so exit" );

        return;
      }
      this.Session.RecordField.Design.IsSummaryField = false;
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );

      //
      // if the table object is null initialise the object.
      //
      if ( this.Session.RecordField.Table == null )
      {
        this.Session.RecordField.Table = new EdRecordTable ( );
      }

      //
      // Define the Form_Field_Sex_Validation_Group_Title properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Form_Field_Table_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
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
        EuRecordFields.CONST_TABLE_ROW_FIELD,
        EdLabels.Form_Field_Table_Row_Field_Label,
        this.Session.RecordField.Table.Rows.Count.ToString ( ),
        optionList );
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Center_Justified;

      //
      // Field disabled form male subjects.
      //
      groupField = pageGroup.createTableField (
        EuRecordFields.CONST_TABLE_FIELD_ID,
        String.Empty, 5 );

      groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

      groupField.Table.Header [ 0 ].Text = "No";
      groupField.Table.Header [ 0 ].TypeId = EvDataTypes.Read_Only_Text;
      groupField.Table.Header [ 0 ].Width = "50px";

      groupField.Table.Header [ 1 ].Text = "Header text";
      groupField.Table.Header [ 1 ].TypeId = EvDataTypes.Text;
      groupField.Table.Header [ 2 ].Width = "250px";

      groupField.Table.Header [ 2 ].Text = "Width";
      groupField.Table.Header [ 2 ].TypeId = EvDataTypes.Selection_List;
      groupField.Table.Header [ 2 ].OptionList =
         Evado.Model.Digital.EvcStatics.getStringAsOptionList ( "5;10;12;15;20;25;30;35;40;45;50" );

      groupField.Table.Header [ 3 ].Text = "Data Type";
      groupField.Table.Header [ 3 ].TypeId = EvDataTypes.Selection_List;
      groupField.Table.Header [ 3 ].OptionList = EdRecordTableHeader.getTypeList (
        this.Session.RecordField.TypeId );

      groupField.Table.Header [ 4 ].Text = "Option or Unit";
      groupField.Table.Header [ 4 ].TypeId = EvDataTypes.Text;

      //
      // Output the table rows.
      //
      for ( int iCol = 0; iCol < 10; iCol++ )
      {
        String indexCol = ( iCol + 1 ).ToString ( );
        EdRecordTableHeader header = this.Session.RecordField.Table.Header [ iCol ];
        Evado.Model.UniForm.TableRow row = new Model.UniForm.TableRow ( );
        //
        // if the prefilled exists convert it to Matrix type  (readonly).
        //
        if ( this.Session.RecordField.Table.PreFilledColumnList.Contains ( indexCol ) == true )
        {
          header.TypeId = EvDataTypes.Read_Only_Text;
        }
        this.LogValue ( "Col: " + iCol + " T: " + header.Text + ", TYP: " + header.TypeId );

        row.Column [ 0 ] = header.No.ToString ( );
        row.Column [ 1 ] = header.Text.ToString ( );
        row.Column [ 2 ] = header.Width;
        row.Column [ 3 ] = header.TypeId.ToString();
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
      if ( this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Special_Matrix )
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
      if ( this.Session.RecordField.Table == null )
      {
        this.Session.RecordField.Table = new EdRecordTable ( );
      }

      //
      // Define the Form_Field_Sex_Validation_Group_Title properties pageMenuGroup..
      //
      pageGroup = PageObject.AddGroup (
        EdLabels.Form_Field_Matrix_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = PageObject.EditAccess;

      //
      // Add the group commands
      //
      this.getGroupCommands ( pageGroup );

      //
      // Field disabled form male subjects.
      //
      groupField = pageGroup.createTableField (
        EuRecordFields.CONST_MATRIX_FIELD_ID,
        String.Empty,
        10 );
      groupField.Layout = Model.UniForm.FieldLayoutCodes.Column_Layout;

      this.LogDebug ( "PreFilledColumnList: " + this.Session.RecordField.Table.PreFilledColumnList );

      for ( int iCol = 0; iCol < 10; iCol++ )
      {
        String indexCol = ( iCol + 1 ).ToString ( );
        EdRecordTableHeader header = this.Session.RecordField.Table.Header [ iCol ];

        if ( header.Text != String.Empty )
        {
          this.LogDebug ( "TypeId {0}." ,header.TypeId );

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


          if ( groupField.Table.Header [ iCol ].TypeId ==  EvDataTypes.Radio_Button_List
            || groupField.Table.Header [ iCol ].TypeId == EvDataTypes.Selection_List )
          {
            groupField.Table.Header [ iCol ].OptionList =
               Evado.Model.Digital.EvcStatics.getStringAsOptionList ( this.Session.RecordField.Table.Header [ iCol ].OptionsOrUnit );
          }

          if ( groupField.Table.Header [ iCol ].TypeId == EvDataTypes.Special_Matrix)
          {
            groupField.Table.Header [ iCol ].TypeId = EvDataTypes.Text;
          }
        }
      }

      //
      // Iterate through the rows in the matrix.
      //
      foreach ( EdRecordTableRow row in this.Session.RecordField.Table.Rows )
      {
        Evado.Model.UniForm.TableRow groupRow = new Model.UniForm.TableRow ( );

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
        int initialVersion = (int) this.Session.RecordLayout.Design.Version;

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "createObject",
          this.Session.UserProfile );

        // 
        // Initialise the methods variables and objects.
        //    
        string FormId = PageCommand.GetParameter ( EdRecord.RecordFieldNames.Layout_Id );
        string stCount = PageCommand.GetParameter ( EuRecordFields.CONST_FIELD_COUNT );
        string stSectionNo = PageCommand.GetParameter ( EuRecordFields.CONST_FORM_SECTION );

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

        this.Session.RecordField = new  Evado.Model.Digital.EdRecordField ( );
        this.Session.RecordField.Guid =  Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.RecordField.LayoutGuid = this.Session.RecordLayout.Guid;
        this.Session.RecordField.LayoutId = this.Session.RecordLayout.LayoutId;
        this.Session.RecordField.Design.InitialVersion = initialVersion;
        this.Session.RecordField.Order = order;
        this.Session.RecordField.Design.SectionNo = EvStatics.getInteger (stSectionNo );
        this.LogValue ( "Field.InitialVersion: " + this.Session.RecordField.Design.InitialVersion );
        this.LogValue ( "Field.LayoutGuid: " + this.Session.RecordField.LayoutGuid );
        this.LogValue ( "Field.LayoutId: " + this.Session.RecordField.LayoutId );

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
        this.LogValue ( "Guid: " + this.Session.RecordField.Guid );
        this.LogValue ( "FormId: " + this.Session.RecordField.LayoutId );
        this.LogValue ( "Title: " + this.Session.RecordField.Title );

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );

        //
        // If is selected is true this is a new field set the guid to empty.
        //
        if ( this.Session.RecordField.Guid ==  Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.RecordField.Guid = Guid.Empty;
          this.Session.RecordLayout.Fields = new List<EdRecordField> ( );
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

        this.updateVideoImageValues ( PageCommand );

        this.updateTableValues ( PageCommand );

        this.updateMatrixValues ( PageCommand );

        //
        // Validate that the field identifier is valid.
        //
        if ( this.validateFieldId ( PageCommand ) == false )
        {
          this.Session.LastPage.Message = this.ErrorMessage;
          return this.Session.LastPage;
        }

        // 
        // Execute the save record groupCommand to save the record values to the 
        // Evado database.
        // 
        EvEventCodes result = this._Bll_FormFields.SaveItem (
          this.Session.RecordField );

        this.LogValue ( this._Bll_FormFields.Log );

        // 
        // If an error state is returned log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          if ( result == EvEventCodes.Data_Duplicate_Field_Error )
          {
            this.ErrorMessage = String.Format ( EdLabels.Form_Field_Duplicate_ID_Error_Message,
              this.Session.RecordField.FieldId );
          }
          string StEvent = this._Bll_FormFields.Log + " returned error message: " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
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

        return new Model.UniForm.AppData();

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
      this.LogValue ( "FormField.Guid: " + this.Session.RecordField.Guid + ", FieldId: " + this.Session.RecordField.FieldId );

      //
      // Replace periods in the field identifier with underscore
      //
      this.Session.RecordField.FieldId = this.Session.RecordField.FieldId.Replace ( " ", "_" );

      //
      // Iterate through the form fields checking to ensure there are not duplicate field identifiers.
      //
      foreach ( EdRecordField field in this.Session.RecordLayout.Fields )
      {
        this.LogValue ( "FormField.Guid: " + field.Guid + ", FieldId: " + field.FieldId );

        if ( field.FieldId == this.Session.RecordField.FieldId
          && this.Session.RecordField.Guid != field.Guid )
        {
          this.ErrorMessage = String.Format ( EdLabels.Form_Field_Duplicate_ID_Error_Message,
            this.Session.RecordField.FieldId );

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
    private void updateObjectValues ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogValue ( "PageCommand.Parameters List.Count: " + PageCommand.Parameters.Count );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
      {
        this.LogTextStart( parameter.Name + " > " + parameter.Value );

        if ( parameter.Name.Contains (  Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == false
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          && parameter.Name !=  Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION
          && parameter.Name != EuRecordFields.CONST_TABLE_ROW_FIELD
          && parameter.Name != EuRecordFields.CONST_MATIX_COL_FIELD
          && parameter.Name != EuRecordFields.CONST_VIDEO_IMAGE_WIDTH
          && parameter.Name != EuRecordFields.CONST_VIDEO_IMAGE_HEIGHT
          && parameter.Name != EuRecordLayouts.CONST_REFRESH )
        {
          this.LogText ( " >> UPDATED" );
          try
          {
            EdRecordField.FieldClassFieldNames fieldName =  Evado.Model.EvStatics.parseEnumValue<EdRecordField.FieldClassFieldNames> ( parameter.Name );

            this.Session.RecordField.setValue ( fieldName, parameter.Value );

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
    /// THis method updates the form field external image or stream video values with the groupCommand parameter values.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command objects.</param>
    //  ----------------------------------------------------------------------------------
    private void updateVideoImageValues (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateVideoImageValues" );

      //
      // If the ResultData type is not a table or matrix exit.
      //
      if ( this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Streamed_Video
        && this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.External_Image )
      {
        this.LogValue ( "Not a streamed video or extenal image so exit" );
        return;
      }

      string stWidth = PageCommand.GetParameter ( EuRecordFields.CONST_VIDEO_IMAGE_WIDTH );
      string stHeight = PageCommand.GetParameter ( EuRecordFields.CONST_VIDEO_IMAGE_HEIGHT );

      this.Session.RecordField.Design.JavaScript = String.Empty;

      if ( stWidth != String.Empty )
      {
        this.Session.RecordField.Design.JavaScript = stWidth;

        if ( stHeight != String.Empty )
        {
          this.Session.RecordField.Design.JavaScript += ";" + stHeight;
        }
      }

    }//END updateVideoImageValues method.

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
      if ( this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Table
        && this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Special_Matrix )
      {
        this.LogValue ( "Not a table or matix so exit" );
        return;
      }

      //
      // if the table object is null initialise it.
      //
      if ( this.Session.RecordField.Table == null )
      {
        this.LogValue ( "Re initialised the table" );
        this.Session.RecordField.Table = new EdRecordTable ( );
      }

      // 
      // Iterate through the parameter values updating the ResultData object
      //
      string stRowCount = PageCommand.GetParameter ( EuRecordFields.CONST_TABLE_ROW_FIELD );

      this.LogValue ( "stRowCount: " + stRowCount );

      int iRowCount =  Evado.Model.Digital.EvcStatics.getInteger ( stRowCount );

      this.Session.RecordField.Table.SetRowCount ( iRowCount );

      this.LogValue ( "Table.Rows.Count: " + this.Session.RecordField.Table.Rows.Count );

      //
      // Iterate through the table headers.
      //
      for ( int iCol = 0; iCol < this.Session.RecordField.Table.Header.Length; iCol++ )
      {
        String stParameterName = EuRecordFields.CONST_TABLE_FIELD_ID + "_" + ( iCol + 1 );

        this.LogValue ( "stParameterName: " + stParameterName );

        //
        // Set the column header .
        //
        this.Session.RecordField.Table.Header [ iCol ].Text =
          PageCommand.GetParameter ( stParameterName + "_2" );

        //
        // Set the column width.
        //
        this.Session.RecordField.Table.Header [ iCol ].Width =
          PageCommand.GetParameter ( stParameterName + "_3" );

        //
        // Set the column date type.
        //
        this.Session.RecordField.Table.Header [ iCol ].TypeId =
          PageCommand.GetParameter<EvDataTypes> ( stParameterName + "_4" ); 

        this.LogValue ( "Text: " + this.Session.RecordField.Table.Header [ iCol ].Text );

        //
        // Set the column date type.
        //
        this.Session.RecordField.Table.Header [ iCol ].OptionsOrUnit =
          PageCommand.GetParameter ( stParameterName + "_5" );

        this.LogValue ( "Text: " + this.Session.RecordField.Table.Header [ iCol ].Text );

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
      if ( this.Session.RecordField.TypeId != Evado.Model.EvDataTypes.Special_Matrix )
      {
        this.LogValue ( "Not a matix so exit" );
        return;
      }

      // 
      // Get the list of prefilled columns
      //
      this.Session.RecordField.Table.PreFilledColumnList =
        PageCommand.GetParameter ( EuRecordFields.CONST_MATIX_COL_FIELD );
      this.LogValue ( "PreFilledColumnList: " + this.Session.RecordField.Table.PreFilledColumnList );

      //
      // Iterate through the table rows.
      //
      for ( int iRow = 0; iRow < this.Session.RecordField.Table.Rows.Count; iRow++ )
      {
        String indexRow = ( iRow + 1 ).ToString ( );

        //
        // Iterate through the table columns.
        //
        for ( int iCol = 0; iCol < this.Session.RecordField.Table.Header.Length; iCol++ )
        {
          String indexCol = ( iCol + 1 ).ToString ( );
          String stParmeterName = EuRecordFields.CONST_MATRIX_FIELD_ID + "_" + indexRow + "_" + indexCol;

          //
          // if the column is included in the prefil list add the value to the table.
          //
          if ( this.Session.RecordField.Table.PreFilledColumnList.Contains ( indexCol ) == true
            || this.Session.RecordField.Table.Header [ iCol ].TypeId == EvDataTypes.Special_Matrix)
          {
            String stValue = PageCommand.GetParameter ( stParmeterName );

            this.LogValue ( stParmeterName + ": " + stValue );

            this.Session.RecordField.Table.Rows [ iRow ].Column [ iCol ] = stValue;
          }
          else
          {
            this.Session.RecordField.Table.Rows [ iRow ].Column [ iCol ] = String.Empty;
          }

        }// End column iteration loop

      }//END row iteration loop

    }//END updateTableValues method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }
}//END namespace