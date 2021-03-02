/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\UserProfiles.cs" company="EVADO HOLDING PTY. LTD.">
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
    private Evado.Model.UniForm.AppData getObject_UserProfile (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "getObject_UserProfile" );
      // 
      // Initialise the methods variables and objects.
      // 
      Guid subjectGuid = Guid.Empty;
      Evado.Model.UniForm.AppData clientDataObject = new Model.UniForm.AppData ( );

      try
      {
        //
        // Initialise the client ResultData object.
        //
        clientDataObject.Id = this.Session.UserProfile.Guid;
        clientDataObject.Page.Id = clientDataObject.Id;
        clientDataObject.Page.PageDataGuid = clientDataObject.Id;
        // 
        // return the client ResultData object for the customer.
        // 
        this.getDataObject_UserPage ( clientDataObject );

        this.LogMethodEnd ( "getObject_UserProfile" );
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

      this.LogMethodEnd ( "getObject_UserProfile" );
      return this.Session.LastPage;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_UserPage (
      Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getDataObject_UserPage" );
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
      ClientDataObject.Page.PageId = EvPageIds.User_Profile_Page.ToString ( );
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      this.LogDebug ( "clientDataObject status: " + ClientDataObject.Page.EditAccess );

      this.getDataObject_UserGeneralGroup ( ClientDataObject.Page );

      this.getDataObject_UserDetailsGroup ( ClientDataObject.Page );

      this.getDataObject_DashboardGroup ( ClientDataObject.Page );

      this.LogMethodEnd ( "getDataObject_UserPage" );

    }//END getclientDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_UserGeneralGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getDataObject_UserGeneralGroup" );
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

      pageGroup.SetCommandBackBroundColor (
        Model.UniForm.GroupParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

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

      //
      // Generate the organisation type radio button list field object.
      //
      groupField = pageGroup.createTextField ( String.Empty,
        EdLabels.User_Profile_Role_Label,
         EvStatics.enumValueToString ( this.Session.UserProfile.Roles ),
        30 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;


      this.LogMethodEnd ( "getDataObject_UserGeneralGroup" );

    }//END getDataObject_FieldGroup Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_UserDetailsGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getDataObject_UserDetailsGroup" );
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

      // 
      // Create the  name object
      // 
      groupField = pageGroup.createTextField (
         Evado.Model.Digital.EdUserProfile.UserProfileFieldNames.Prefix,
        EdLabels.UserProfile_Prefix_Field_Label,
        this.Session.UserProfile.Prefix, 10 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField = pageGroup.createTextField (
         Evado.Model.Digital.EdUserProfile.UserProfileFieldNames.Given_Name,
        EdLabels.UserProfile_GivenName_Field_Label,
        this.Session.UserProfile.GivenName, 50 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.Mandatory = true;
      groupField.setBackgroundColor (
        Model.UniForm.FieldParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      groupField = pageGroup.createTextField (
         Evado.Model.Digital.EdUserProfile.UserProfileFieldNames.Family_Name,
        EdLabels.UserProfile_FamilyName_Field_Label,
        this.Session.UserProfile.FamilyName, 50 );
      groupField.Layout = EuAdapter.DefaultFieldLayout;
      groupField.Mandatory = true;
      groupField.setBackgroundColor (
        Model.UniForm.FieldParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      //
      // define the user address field.
      //
      if ( this.Session.CollectUserAddress == true )
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
      // 
      // Create the customer telephone number object
      // 
      groupField = pageGroup.createTelephoneNumberField (
         Evado.Model.Digital.EdUserProfile.UserProfileFieldNames.Telephone.ToString ( ),
        EdLabels.UserProfile_Telephone_Field_Label,
        this.Session.UserProfile.Telephone );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the customer telephone number object
      // 
      groupField = pageGroup.createTelephoneNumberField (
         Evado.Model.Digital.EdUserProfile.UserProfileFieldNames.Mobile_Phone.ToString ( ),
        EdLabels.UserProfile_Mobilephone_Field_Label,
        this.Session.UserProfile.MobilePhone );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the customer fax number object
      // 
      groupField = pageGroup.createEmailAddressField (
         Evado.Model.Digital.EdUserProfile.UserProfileFieldNames.Email_Address.ToString ( ),
        EdLabels.UserProfile_Email_Field_Label,
        this.Session.UserProfile.EmailAddress );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      this.LogMethodEnd ( "getDataObject_UserDetailsGroup" );

    }//END getDataObject_FieldGroup Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="Page">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_DashboardGroup (
      Evado.Model.UniForm.Page Page )
    {
      this.LogMethod ( "getDataObject_DashboardGroup" );
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
      this.LogMethodEnd ( "getDataObject_DashboardGroup" );
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

      groupCommand.SetPageId ( EvPageIds.User_Profile_Update_Page );

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

        // 
        // Update the object.
        // 
        if ( this.updateUserObjectValue ( PageCommand ) == false )
        {
          this.ErrorMessage = EdLabels.UserProfile_Value_Update_Error_Message;

          return this.Session.LastPage;
        }


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
        // Update the address field.
        //
        this.updateUserAddressValue ( PageCommand );

        // 
        // Get the save action message value.
        // 
        String stSaveAction = PageCommand.GetParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );

        // 
        // update the object.
        // 
        result = this._Bll_UserProfiles.saveItem ( this.Session.UserProfile );

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
        this.LogTextStart ( parameter.Name + " = " + parameter.Value );

        if ( parameter.Name.Contains ( Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == false
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Page_Id.ToString ( )
          && parameter.Name != Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION
          && parameter.Name != EuUserProfiles.CONST_ADDRESS_FIELD_ID
          && parameter.Name != EuUserProfiles.CONST_CURRENT_FIELD_ID
          && parameter.Name != EuUserProfiles.CONST_NEW_PASSWORD_PARAMETER )
        {
          this.LogTextEnd ( " >> UPDATED" );
          try
          {
            Evado.Model.Digital.EdUserProfile.UserProfileFieldNames fieldName =
              Evado.Model.EvStatics.parseEnumValue<Evado.Model.Digital.EdUserProfile.UserProfileFieldNames> (
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
          this.LogTextEnd ( " >> SKIPPED" );
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

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace