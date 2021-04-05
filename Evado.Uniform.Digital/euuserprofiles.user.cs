/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\UserProfiles.cs" company="EVADO HOLDING PTY. LTD.">
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
using Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Digital
{
  /// <summary>
  /// This partial class contains the user updatable profile page layout.
  /// </summary>
  public partial class EuUserProfiles : EuClassAdapterBase
  {
    #region Class get user profile methods
    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getObject_MyUserProfile (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getObject_MyUserProfile" );
      // 
      // Initialise the methods variables and objects.
      // 
      Guid subjectGuid = Guid.Empty;
      Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );

      try
      {
        // 
        // return the client ResultData object for the customer.
        // 
        this.getDataObject_MUP_Page ( clientDataObject );

        this.LogMethodEnd ( "getObject_MyUserProfile" );
        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.User_Profile_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getObject_MyUserProfile" );
      return this.Session.LastPage;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_MUP_Page (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getDataObject_MUP_Page" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      //
      // set the client ResultData object properties
      //
      ClientDataObject.Id = this.Session.UserProfile.Guid;
      ClientDataObject.Page.Id = this.Session.UserProfile.Guid;
      ClientDataObject.Title = EdLabels.User_Profile_Page_Title
        + this.Session.UserProfile.CommonName;

      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.PageId = EdStaticPageIds.User_Profile_Page.ToString ( );
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      this.LogDebug ( "clientDataObject status: " + ClientDataObject.Page.EditAccess );

      this.getDataObject_MUP_GeneralGroup ( ClientDataObject.Page );

      this.getDataObject_MUP_DetailsGroup ( ClientDataObject.Page );

      this.getDataObject_MUP_OrganisationGroup ( ClientDataObject.Page );

      this.getDataObject_MUP_PersonaliseGroup ( ClientDataObject.Page );

      this.LogMethodEnd ( "getDataObject_MUP_Page" );

    }//END getclientDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method adds the user generation group.
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_MUP_GeneralGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getDataObject_MUP_GeneralGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      List<EvOption> optionList = new List<EvOption> ( );

      // 
      // create the page pageMenuGroup
      // 
      pageGroup = Page.AddGroup (
        EdLabels.UserProfile_General_Field_Group_Title );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = Model.UniForm.EditAccess.Disabled;

      // 
      // Create the user id object
      // 
      groupField = pageGroup.createTextField (
         String.Empty,
        EdLabels.User_Profile_Identifier_Field_Label,
        this.Session.UserProfile.UserId,
        80 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;


      this.LogMethodEnd ( "getDataObject_MUP_GeneralGroup" );

    }//END getDataObject_MUP_GeneralGroup Method

    // ==============================================================================
    /// <summary>
    /// This method add the user personal details update group.
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_MUP_DetailsGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getDataObject_MUP_DetailsGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );

      // 
      // create the page pageMenuGroup
      // 
      pageGroup = Page.AddGroup (
        EdLabels.UserProfile_General_Details_Group_Title,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      pageGroup.SetCommandBackBroundColor (
        Model.UniForm.GroupParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      //
      // Add the groups commands.
      //
      this.getDataObject_UserGroupCommands ( pageGroup );


      this.LogDebug ( "ImageFileName {0}.", this.Session.UserProfile.ImageFileName );
      // 
      // Create the customer name object
      // 
      this.Session.UserProfile.CurrentImageFileName = this.Session.UserProfile.ImageFileName;

      groupField = pageGroup.createImageField (
        Evado.Model.Digital.EdUserProfile.FieldNames.Image_File_Name,
        EdLabels.UserProfile_ImageFileame_Field_Label,
        this.UniForm_ImageServiceUrl + this.Session.UserProfile.ImageFileName,
        300,
        300 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      try
      {
        String stTargetPath = this.UniForm_BinaryFilePath + this.Session.UserProfile.ImageFileName;
        String stImagePath = this.UniForm_ImageFilePath + this.Session.UserProfile.ImageFileName;

        this.LogDebug ( "Target path {0}.", stTargetPath );
        this.LogDebug ( "Image path {0}.", stImagePath );

        //
        // copy the file into the image directory.
        //
        System.IO.File.Copy ( stImagePath, stTargetPath, true );
      }
      catch { }


      if ( this.AdapterObjects.Settings.hasHiddenUserProfileField ( EdUserProfile.FieldNames.Title ) == false )
      {
        groupField = pageGroup.createTextField (
          Evado.Model.Digital.EdUserProfile.FieldNames.Title,
          EdLabels.UserProfile_Title_Field_Label,
          this.Session.UserProfile.Title, 50 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      //
      // create the name field.
      //
      this.LogDebug ( "Delimited Name: " + this.Session.UserProfile.DelimitedName );

      groupField = pageGroup.createNameField (
        Evado.Model.Digital.EdUserProfile.FieldNames.Delimted_Name,
        EdLabels.UserProfile_Name_Field_Label,
        this.Session.UserProfile.DelimitedName,
        true,
        false );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      #region address block
      //
      // define the user address field.
      //
      if ( this.AdapterObjects.Settings.EnableUserAddressUpdate == true
        && this.AdapterObjects.Settings.EnableUserOrganisationUpdate == false )
      {
        this.LogDebug ( "Address_1:" + this.Session.UserProfile.Address_1 );
        this.LogDebug ( "Address_2:" + this.Session.UserProfile.Address_2 );
        this.LogDebug ( "AddressCity:" + this.Session.UserProfile.AddressCity );
        this.LogDebug ( "AddressState:" + this.Session.UserProfile.AddressState );
        this.LogDebug ( "AddressPostCode:" + this.Session.UserProfile.AddressPostCode );
        this.LogDebug ( "AddressCountry:" + this.Session.UserProfile.AddressCountry );
        // 
        // Create the customer name object
        //
        groupField = pageGroup.createAddressField (
          EuUserProfiles.CONST_ADDRESS_FIELD_ID,
          EdLabels.UserProfile_Address_Field_Label,
          this.Session.UserProfile.Address_1,
          this.Session.UserProfile.Address_2,
          this.Session.UserProfile.AddressCity,
          this.Session.UserProfile.AddressState,
          this.Session.UserProfile.AddressPostCode,
          this.Session.UserProfile.AddressCountry );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        this.LogDebug ( "AddresS:" + groupField.Value );
      }
      #endregion
      #region contact details
      // 
      // Create the customer telephone number object
      // 
      groupField = pageGroup.createTelephoneNumberField (
         Evado.Model.Digital.EdUserProfile.FieldNames.Telephone.ToString ( ),
        EdLabels.UserProfile_Telephone_Field_Label,
        this.Session.UserProfile.Telephone );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the customer telephone number object
      // 
      groupField = pageGroup.createTelephoneNumberField (
         Evado.Model.Digital.EdUserProfile.FieldNames.Mobile_Phone.ToString ( ),
        EdLabels.UserProfile_Mobilephone_Field_Label,
        this.Session.UserProfile.MobilePhone );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the customer fax number object
      // 
      groupField = pageGroup.createEmailAddressField (
         Evado.Model.Digital.EdUserProfile.FieldNames.Email_Address.ToString ( ),
        EdLabels.UserProfile_Email_Field_Label,
        this.Session.UserProfile.EmailAddress );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      #endregion

      this.LogMethodEnd ( "getDataObject_MUP_DetailsGroup" );

    }//END getDataObject_MUP_DetailsGroup Method

    private const string CONST_ORG_PREFIX = "ORG_";
    // ==============================================================================
    /// <summary>
    /// This method add the user personaliseation group
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_MUP_OrganisationGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_MUP_OrganisationGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      List<EvOption> optionList = new List<EvOption> ( );

      if ( this.AdapterObjects.Settings.EnableUserOrganisationUpdate == false )
      {
        this.LogDebug ( "Update update of organisations is disabled." );
        this.LogMethodEnd ( "getDataObject_MUP_OrganisationGroup" );
        return;
      }

      // 
      // create the page pageMenuGroup
      // 
      pageGroup = PageObject.AddGroup (
        String.Empty );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Add the groups commands.
      //
      this.getDataObject_UserGroupCommands ( pageGroup );

      // 
      // Create the customer name object
      // 
      groupField = pageGroup.createTextField (
        EuUserProfiles.CONST_ORG_PREFIX + EdOrganisation.FieldNames.Name.ToString ( ),
        EdLabels.Organisation_Name_Field_Label,
        this.Session.Organisation.Name,
        50 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.Mandatory = true;

      groupField.setBackgroundColor (
        Model.UniForm.FieldParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      // 
      // Create the customer name object
      // 
      groupField = pageGroup.createImageField (
        EuUserProfiles.CONST_ORG_PREFIX + EdOrganisation.FieldNames.Image_File_Name.ToString ( ),
        EdLabels.Organisation_ImageFileame_Field_Label,
        this.Session.Organisation.ImageFileName,
        300, 200 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      try
      {
        String stTargetPath = this.UniForm_BinaryFilePath + this.Session.Organisation.ImageFileName;
        String stImagePath = this.UniForm_ImageFilePath + this.Session.Organisation.ImageFileName;

        this.LogDebug ( "Target path {0}.", stTargetPath );
        this.LogDebug ( "Image path {0}.", stImagePath );

        //
        // copy the file into the image directory.
        //
        System.IO.File.Copy ( stImagePath, stTargetPath, true );
      }
      catch { }

      // 
      // Create the street address 1
      //
      if ( this.AdapterObjects.Settings.hasHiddenOrganisationField (
        EdOrganisation.FieldNames.Address_1 ) == false )
      {
        groupField = pageGroup.createTextField (
          EuUserProfiles.CONST_ORG_PREFIX + EdOrganisation.FieldNames.Address_1,
          EdLabels.Organisation_Address_Street_Field_Label,
          this.Session.Organisation.AddressStreet_1, 50 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;

        // 
        // Create the street address 2
        // 
        groupField = pageGroup.createTextField (
          EuUserProfiles.CONST_ORG_PREFIX + EdOrganisation.FieldNames.Address_2,
          EdLabels.Organisation_Address_Street_Field_Label,
          this.Session.Organisation.AddressStreet_2, 50 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      // 
      // Create the street address city
      // 
      if ( this.AdapterObjects.Settings.hasHiddenOrganisationField (
        EdOrganisation.FieldNames.Address_City ) == false )
      {
        groupField = pageGroup.createTextField (
          EuUserProfiles.CONST_ORG_PREFIX + EdOrganisation.FieldNames.Address_City,
          EdLabels.Organisation_Address_City_Field_Label,
          this.Session.Organisation.AddressCity, 50 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      // 
      // Create the street address state
      // 
      if ( this.AdapterObjects.Settings.hasHiddenOrganisationField (
        EdOrganisation.FieldNames.Address_State ) == false )
      {
        groupField = pageGroup.createTextField (
          EuUserProfiles.CONST_ORG_PREFIX + EdOrganisation.FieldNames.Address_State,
          EdLabels.Organisation_Address_State_Field_Label,
          this.Session.Organisation.AddressState, 10 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      // 
      // Create the street address 1
      // 
      if ( this.AdapterObjects.Settings.hasHiddenOrganisationField (
        EdOrganisation.FieldNames.Address_Post_Code ) == false )
      {
        groupField = pageGroup.createTextField (
          EuUserProfiles.CONST_ORG_PREFIX + EdOrganisation.FieldNames.Address_Post_Code,
          EdLabels.Organisation_Address_City_Field_Label,
          this.Session.Organisation.AddressPostCode, 50 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      // 
      // Create the street address 1
      // 
      if ( this.AdapterObjects.Settings.hasHiddenOrganisationField (
        EdOrganisation.FieldNames.Address_Country ) == false )
      {
        groupField = pageGroup.createTextField (
          EuUserProfiles.CONST_ORG_PREFIX + EdOrganisation.FieldNames.Address_Country,
          EdLabels.Organisation_Address_Country_Field_Label,
          this.Session.Organisation.AddressCountry, 50 );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      // 
      // Create the organisation telephone number object
      // 
      if ( this.AdapterObjects.Settings.hasHiddenOrganisationField (
        EdOrganisation.FieldNames.Telephone ) == false )
      {
        groupField = pageGroup.createTelephoneNumberField (
          EuUserProfiles.CONST_ORG_PREFIX + EdOrganisation.FieldNames.Telephone.ToString ( ),
          EdLabels.Organisation_Telephone_Field_Label,
          this.Session.Organisation.Telephone );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }

      // 
      // Create the organisation fax number object
      // 
      if ( this.AdapterObjects.Settings.hasHiddenOrganisationField (
        EdOrganisation.FieldNames.Email_Address ) == false )
      {
        groupField = pageGroup.createEmailAddressField (
          EuUserProfiles.CONST_ORG_PREFIX + EdOrganisation.FieldNames.Email_Address.ToString ( ),
          EdLabels.Organisation_Email_Field_Label,
          this.Session.Organisation.EmailAddress );
        groupField.Layout = EuAdapter.DefaultFieldLayout;
      }


      this.LogMethodEnd ( "getDataObject_MUP_OrganisationGroup" );
    }

    // ==============================================================================
    /// <summary>
    /// This method add the user personaliseation group
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_MUP_PersonaliseGroup (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_MUP_PersonaliseGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field groupField = new Evado.Model.UniForm.Field ( );
      Evado.Model.UniForm.Group pageGroup = new Evado.Model.UniForm.Group ( );
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );
      List<EvOption> optionList = new List<EvOption> ( );

      // 
      // create the page pageMenuGroup
      // 
      pageGroup = new Model.UniForm.Group (
         EdLabels.UserProfile_Dashboard_Field_Group_Title,
         Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Add the groups commands.
      //
      this.getDataObject_UserGroupCommands ( pageGroup );

      /*
    if ( this.Session.UserProfile.hasManagementAccess == true )
    {
      //
      // create the project dashboard option list.
      //
      optionList = Model.Digital.EdApplication.getDashBoardList ( false );

      //
      // Add the user's project dashboard component selection
      // 
      groupField = pageGroup.createCheckBoxListField (
         EvUserProfile.UserProfileFieldNames.Project_Dashboard_Components,
         EdLabels.UserProfile_Project_Dashboard_Option_Title,
         this.Session.UserProfile.ProjectDashboardComponents,
         optionList );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
    }

    //
    // These can see site dashboard.
    //
    if ( ( this.ApplicationObjects.ApplicationSettings.DisplaySiteDashboard == true )
     &&  ( this.Session.UserProfile.hasEndUserRole( this.Session.Record.Design.ReadAccessRoles ) == true ) )
    {
      //
      // create the site dashboard option list.
      // 
      optionList = Model.Digital.EdApplication.getDashBoardList ( true );

      //
      // Add the user's site dashboard component selection
      // 
      groupField = pageGroup.createCheckBoxListField (
         EvUserProfile.UserProfileFieldNames.Site_Dashboard_Components,
         EdLabels.UserProfile_Site_Dashboard_Option_Title,
         this.Session.UserProfile.SiteDashboardComponents, optionList );
      groupField.Layout = EuFormGenerator.ApplicationFieldLayout;
    }
      */

      if ( pageGroup.FieldList.Count > 0 )
      {
        PageObject.AddGroup ( pageGroup );
      }

      this.LogMethodEnd ( "getDataObject_MUP_PersonaliseGroup" );
    }

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_UserGroupCommands (
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "getDataObject_UserGroupCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command groupCommand = new Evado.Model.UniForm.Command ( );

      // 
      // Add the save groupCommand
      // 
      groupCommand = PageGroup.addCommand (
        EdLabels.User_Profile_Save_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Users.ToString ( ),
        Evado.Model.UniForm.ApplicationMethods.Save_Object );

      groupCommand.SetPageId ( EdStaticPageIds.My_User_Profile_Update_Page );

      // 
      // Define the save and delete groupCommand parameters
      // 
      groupCommand.SetGuid ( this.Session.UserProfile.Guid );

      this.LogMethodEnd ( "getDataObject_UserGroupCommands" );

    }//END getDataObject_GroupCommands method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Update User methods
    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectEvado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData updateUserObject ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateUserObject" );
      this.LogDebug ( "Parameter: " + PageCommand.getAsString ( false, false ) );

      this.LogDebug ( "eClinical.AdminUserProfile:" );
      this.LogDebug ( "Guid: " + this.Session.UserProfile.Guid );
      this.LogDebug ( "UserId: " + this.Session.UserProfile.UserId );
      this.LogDebug ( "CommonName: " + this.Session.UserProfile.CommonName );

      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );
        EvEventCodes result;
        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );


        if ( this.Session.UserProfile.Parameters != null )
        {
          this.LogDebug ( "Parameters.Count: " + this.Session.UserProfile.Parameters.Count );

          foreach ( EvObjectParameter parm in this.Session.UserProfile.Parameters )
          {
            if ( parm != null )
            {
              this.LogDebug ( "Name: " + parm.Name + " value: " + parm.Value );
            }
          }
        }

        // 
        // Update the object.
        // 
        if ( this.updateUserObjectValue ( PageCommand ) == false )
        {
          this.ErrorMessage = EdLabels.UserProfile_Value_Update_Error_Message;

          return this.Session.LastPage;
        }


        //
        // Update the address field.
        //
        this.updateUserAddressValue ( PageCommand );

        this.updateOrganisationValues ( PageCommand );

        //
        // save the image file if it exists.
        //
        this.saveUserImageFile ( );


        this.LogDebug ( "DelimitedName: " + this.Session.UserProfile.DelimitedName );
        this.LogDebug ( "GivenName: " + this.Session.UserProfile.GivenName );
        this.LogDebug ( "MiddleName: " + this.Session.UserProfile.MiddleName );
        this.LogDebug ( "FamilyName: " + this.Session.UserProfile.FamilyName );

        // 
        // Get the save action message value.
        // 
        String stSaveAction = PageCommand.GetParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );

        // 
        // update the object.
        // 
        result = saveUserProfile ( PageCommand );

        // 
        // if an error state is returned create log the event.
        //
        if ( result != EvEventCodes.Ok )
        {
          return this.Session.LastPage;
        }

        //
        // Update the organisation.
        //
        result = this.saveOrganisationValue ( );

        // 
        // if an error state is returned create log the event.
        //
        if ( result != EvEventCodes.Ok )
        {
          return this.Session.LastPage;
        }

        this.LogMethodEnd ( "updateUserObject" );

        this.Session.UserProfile = new EdUserProfile ( );

        return new Model.UniForm.AppData ( );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.User_Profile_Save_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }
      this.LogMethodEnd ( "updateUserObject" );
      return this.Session.LastPage; ;

    }//END method


    // ==================================================================================
    /// <summary>
    /// THis method updates the organisation object.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    private EvEventCodes saveUserProfile ( Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "saveUserProfile" );
      //
      // Initialise the methods variables and objecs.
      //

      // 
      // Get the save action message value.
      // 
      String stSaveAction = PageCommand.GetParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );

      // 
      // update the object.
      // 
      var result = this._Bll_UserProfiles.saveItem ( this.Session.UserProfile );

      // 
      // get the debug ResultData.
      // 
      this.LogDebugClass ( this._Bll_UserProfiles.Log );

      // 
      // if an error state is returned create log the event.
      //
      if ( result != EvEventCodes.Ok )
      {
        string StEvent = this._Bll_UserProfiles.Log + " returned error message: " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
        this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

        this.ErrorMessage = EdLabels.User_Profile_Save_Error_Message;

        return result;
      }

      this.LogMethodEnd ( "saveUserProfile" );
      return EvEventCodes.Ok;
    }//ENd saveUserProfile method

    // ==================================================================================
    /// <summary>
    /// THis method updates the organisation object.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    private EvEventCodes saveOrganisationValue ( )
    {
      this.LogMethod ( "saveOrganisationValue" );
      //
      // Initialise the methods variables and objecs.
      //
      EdOrganisations bll_organisations = new EdOrganisations ( this.ClassParameters );

      // 
      // update the object.
      // 
      EvEventCodes result = bll_organisations.saveItem ( this.Session.Organisation );

      // 
      // get the debug ResultData.
      // 
      this.LogDebugClass ( bll_organisations.Log );

      // 
      // if an error state is returned create log the event.
      // 
      if ( result != EvEventCodes.Ok )
      {
        string StEvent = bll_organisations.Log + " returned error message: " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
        this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

        switch ( result )
        {
          default:
            {
              this.ErrorMessage = EdLabels.Organisation_Update_Error_Message;
              break;
            }
        }
        return result;
      }//END save error returned.


      this.LogMethodEnd ( "saveOrganisationValue" );
      return EvEventCodes.Ok;
    }//ENd saveOrganisationValue method

    // ==================================================================================
    /// <summary>
    /// THis method copies the upload image file to the image directory.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    private void saveUserImageFile ( )
    {
      this.LogMethod ( "saveUserImageFile" );

      if ( this.Session.UserProfile.ImageFileName == null )
      {
        this.LogMethodEnd ( "saveUserImageFile" );
        return;
      }

      if ( this.Session.UserProfile.ImageFileName == String.Empty )
      {
        this.LogDebug ( "Image filename is empty" );
        this.LogMethodEnd ( "saveUserImageFile" );
        return;
      }

      if ( this.Session.UserProfile.CurrentImageFileName == this.Session.UserProfile.ImageFileName )
      {
        this.LogMethodEnd ( "saveUserImageFile" );
        return;
      }


      //
      // Initialise the method variables and objects.
      //
      String stSourcePath = this.UniForm_BinaryFilePath + this.Session.UserProfile.ImageFileName;
      String stImagePath = this.UniForm_ImageFilePath + this.Session.UserProfile.ImageFileName;

      this.LogDebug ( "Source path {0}.", stSourcePath );
      this.LogDebug ( "Image path {0}.", stImagePath );

      //
      // Save the file to the directory repository.
      //
      try
      {
        System.IO.File.Copy ( stSourcePath, stImagePath, true );
      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }
      this.LogMethodEnd ( "saveUserImageFile" );
    }//END saveImageFile method

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameters">List of field values to be updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private bool updateUserObjectValue (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateUserObjectValue" );
      this.LogDebug ( "Parameters.Count: " + PageCommand.Parameters.Count );
      this.LogDebug ( "UserProfile.Guid: " + this.Session.UserProfile.Guid );

      // 
      // Iterate through the parameter values updating the ResultData object
      // 
      foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
      {
        if ( parameter.Name.Contains ( Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == false
          && parameter.Name.Contains ( EuUserProfiles.CONST_ORG_PREFIX ) == false
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Page_Id.ToString ( )
          && parameter.Name != Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION
          && parameter.Name != EuUserProfiles.CONST_ADDRESS_FIELD_ID
          && parameter.Name != EuUserProfiles.CONST_CURRENT_FIELD_ID
          && parameter.Name != EuUserProfiles.CONST_NEW_PASSWORD_PARAMETER )
        {
          this.LogDebug ( "{0} + '{1}' >> UPDATED", parameter.Name, parameter.Value );
          try
          {
            Evado.Model.Digital.EdUserProfile.FieldNames fieldName =
              Evado.Model.EvStatics.parseEnumValue<Evado.Model.Digital.EdUserProfile.FieldNames> (
             parameter.Name );

            this.Session.UserProfile.setValue ( fieldName, parameter.Value );

            //if ( this.Session.UserProfile.debug != String.Empty )
            //{
            //   this.LogDebugValue ( this.Session.UserProfile.debug );
            // }
          }
          catch ( Exception Ex )
          {
            this.LogException ( Ex );

            this.LogMethodEnd ( "updateUserObjectValue" );
            return false;
          }
        }
        else
        {
          this.LogDebug ( "{0} + '{1}' >> SKIPPED", parameter.Name, parameter.Value );
        }

      }// End iteration loop

      //
      // IF the AD user id is empty set it to save value as the UserID.
      //
      if ( this.Session.UserProfile.ActiveDirectoryUserId == String.Empty )
      {
        this.Session.UserProfile.ActiveDirectoryUserId = this.Session.UserProfile.UserId;
      }

      this.LogMethodEnd ( "updateUserObjectValue" );
      return true;

    }//END updateObjectValue method.

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private void updateUserAddressValue (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateAddressValue" );
      this.LogDebug ( "Parameters.Count: " + PageCommand.Parameters.Count );

      //
      // Get the organisation's address 
      //
      String stAddress = PageCommand.GetParameter ( EuUserProfiles.CONST_ADDRESS_FIELD_ID );

      //
      // If there is no address object exit.
      //
      if ( stAddress == String.Empty )
      {
        this.LogDebug ( "Address string empty" );
        this.LogMethodEnd ( "updateAddressValue" );
        return;
      }

      if ( stAddress.Contains ( ";" ) == false )
      {
        this.LogDebug ( "Address missing delimiters." );
        this.LogMethodEnd ( "updateAddressValue" );
        return;
      }

      String [ ] arAddress = stAddress.Split ( ';' );

      this.LogDebug ( "Address array length is {0}.", arAddress.Length );
      if ( arAddress.Length > 5 )
      {
        this.Session.UserProfile.Address_1 = arAddress [ 0 ];
        this.Session.UserProfile.Address_2 = arAddress [ 1 ];
        this.Session.UserProfile.AddressCity = arAddress [ 2 ];
        this.Session.UserProfile.AddressState = arAddress [ 3 ];
        this.Session.UserProfile.AddressPostCode = arAddress [ 4 ];
        this.Session.UserProfile.AddressCountry = arAddress [ 5 ];
      }
      this.LogMethodEnd ( "updateAddressValue" );

    }//END updateAddressValue Method

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private void updateOrganisationValues (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateOrganisationValues" );
      this.LogDebug ( "Parameters.Count: " + PageCommand.Parameters.Count );

      /// 
      /// Iterate through the parameter values updating the ResultData object
      /// 
      foreach ( Evado.Model.UniForm.Parameter parameter in PageCommand.Parameters )
      {
        if ( parameter.Name.Contains ( EuUserProfiles.CONST_ORG_PREFIX ) == true )
        {
          string orgFieldName = parameter.Name.Replace ( EuUserProfiles.CONST_ORG_PREFIX, String.Empty );

          this.LogDebug ( orgFieldName + " = " + parameter.Value + " >> UPDATED" );
          try
          {
            EdOrganisation.FieldNames fieldName =
               Evado.Model.EvStatics.parseEnumValue<EdOrganisation.FieldNames> (
              orgFieldName );

            this.Session.Organisation.setValue ( fieldName, parameter.Value );

          }
          catch ( Exception Ex )
          {
            this.LogException ( Ex );
          }
        }
        else
        {
          this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> SKIPPED" );
        }

      }// End iteration loop
      this.LogMethodEnd ( "updateOrganisationValues" );

    }//END updateOrganisationValues Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace