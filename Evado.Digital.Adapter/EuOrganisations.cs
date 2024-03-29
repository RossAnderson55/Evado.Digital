﻿/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Organisations.cs" company="EVADO HOLDING PTY. LTD.">
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
  /// This class terminates the Organisation object.
  /// </summary>
  public class EuOrganisations : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuOrganisations ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Digital.EuOrganisations.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuOrganisations (
      EuGlobalObjects AdapterObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      String UniForm_BinaryServiceUrl,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = "Evado.UniForm.Digital.EuOrganisations.";
      this.AdapterObjects = AdapterObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.UniForm_BinaryServiceUrl = UniForm_BinaryServiceUrl;
      this.ClassParameters = Settings;


      this.LogInitMethod ( "EuOrganisations initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "Session.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "Session.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );
      this.LogInit ( "UniForm BinaryServiceUrl: " + this.UniForm_BinaryServiceUrl );

      this.LogInit ( "Settings:" );
      this.LogInit ( "-PlatformId: " + this.ClassParameters.PlatformId );
      this.LogInit ( "-ApplicationGuid: " + this.ClassParameters.AdapterGuid );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-CommonName: " + Settings.UserProfile.CommonName );

      this._Bll_Organisations = new Evado.Digital.Bll.EdOrganisations ( this.ClassParameters );

      if ( this.Session.AdminOrganisation == null )
      {
        this.Session.AdminOrganisation = new EdOrganisation ( );
      }
      if ( this.Session.AdminOrganisationList == null )
      {
        this.Session.AdminOrganisationList = new List<EdOrganisation> ( );
      }

      if ( this.Session.SelectedOrganisationType == null )
      {
        this.Session.SelectedOrganisationType = String.Empty;
      }
    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private const String CONST_CURRENT_FIELD_ID = "CURRENT";
    private const String CONST_NEW_FIELD_ID = "NEW";

    private Evado.Digital.Bll.EdOrganisations _Bll_Organisations = new Evado.Digital.Bll.EdOrganisations ( );

    private String _CurrentFileName = String.Empty;

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ==================================================================================
    /// <summary>
    /// This method gets the trial site object
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.UniForm.Model.EuCommand object</param>
    /// <returns>ClientApplicationData</returns>
    //  ----------------------------------------------------------------------------------
    public override Evado.UniForm.Model.EuAppData getDataObject (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getDataObject" );
      this.LogValue ( "PageCommand Content: " + PageCommand.getAsString ( false, false ) );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );

        if ( PageCommand.hasParameter ( EdOrganisation.FieldNames.Org_Type ) == true )
        {
          this.Session.SelectedOrganisationType = PageCommand.GetParameter ( EdOrganisation.FieldNames.Org_Type );
        }
        if ( PageCommand.hasParameter ( Evado.UniForm.Model.EuCommandParameters.Custom_Method ) == true )
        {
          this.Session.AdminOrganisationList = new List<EdOrganisation> ( );
        }
        this.LogValue ( "SelectedOrganisationType: " + this.Session.SelectedOrganisationType );

        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          case Evado.UniForm.Model.EuMethods.List_of_Objects:
            {
              clientDataObject = this.getListObject ( PageCommand );
              break;
            }
          case Evado.UniForm.Model.EuMethods.Get_Object:
            {
              clientDataObject = this.getObject ( PageCommand );
              break;
            }
          case Evado.UniForm.Model.EuMethods.Create_Object:
            {
              clientDataObject = this.createObject ( PageCommand );
              break;
            }
          case Evado.UniForm.Model.EuMethods.Save_Object:
          case Evado.UniForm.Model.EuMethods.Delete_Object:
            {
              // 
              // Update the object values
              // 
              clientDataObject = this.updateObject ( PageCommand );
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
        this.LogMethodEnd ( "getDataObject" );
        return clientDataObject;
      }
      catch ( Exception Ex )
      {
        this.LogException ( Ex );
      }
      this.LogMethodEnd ( "getDataObject" );
      return this.Session.LastPage;

    }//END getDataObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class list methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    public Evado.UniForm.Model.EuAppData getListObject (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getListObject" );
      try
      {
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasAdministrationAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage; ;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        if ( PageCommand.hasParameter ( Evado.UniForm.Model.EuCommandParameters.Custom_Method ) == true )
        {
          this.Session.AdminOrganisationList = new List<EdOrganisation> ( );
        }

          //
        // fill the organisation list.
        //
        this.getOrganisationList ( );

        //
        // Initialise the list of organisations.
        //
        clientDataObject.Title = EdLabels.Organisation_List_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );

        this.getOrganisationSelection ( clientDataObject.Page );
        // 
        // Add the trial organisation list to the page.
        // 
        this.getListGroup ( clientDataObject.Page );


        this.LogMethodEnd ( "getListObject" );
        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Organisation_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      this.LogMethodEnd ( "getListObject" );
      return this.Session.LastPage; ;

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method updates the list of organisaitons.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getOrganisationList ( )
    {
      this.LogMethod ( "getOrganisationList" );
      // 
      // if the list exists exit.
      // 
      if ( this.Session.AdminOrganisationList.Count > 0 )
      {
        return;
      }

      this.Session.AdminOrganisationList = this._Bll_Organisations.getOrganisationList (
        this.Session.SelectedOrganisationType );

      this.LogValue ( this._Bll_Organisations.Log );
      this.LogValue ( "list count: " + this.Session.AdminOrganisationList.Count );

      this.LogMethodEnd ( "getOrganisationList" );

    }//END getOrganisationList method.

    // ==================================================================================
    /// <summary>
    /// This methods returns a pageMenuGroup object contains a selection of organisations.
    /// </summary>
    /// <param name="PageObject">Application</param>
    /// <returns>Evado.UniForm.Model.EuGroup object</returns>
    //  ---------------------------------------------------------------------------------
    public void getOrganisationSelection (
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "getOrganisationSelection" );

      // 
      // initialise the methods variables and objects.
      // 
      List<Evado.Model.EvOption> optionList = new List<Evado.Model.EvOption> ( );
      Evado.UniForm.Model.EuField groupField = new Evado.UniForm.Model.EuField ( );

      Evado.UniForm.Model.EuGroup pageGroup = PageObject.AddGroup (
        EdLabels.Organisation_List_Selection_Group,
        Evado.UniForm.Model.EuEditAccess.Enabled );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;

      //
      // get the list of organisations.
      //
      optionList = this.AdapterObjects.Settings.GetOrgTypeList ( true );

      // 
      // Set the selection to the current site org id.
      // 
      groupField = pageGroup.createSelectionListField (
        EdOrganisation.FieldNames.Org_Type,
        EdLabels.Config_OrgType_List_Field_Label,
        this.Session.SelectedOrganisationType.ToString ( ),
        optionList );
      groupField.Layout = EuAdapter.DefaultFieldLayout;

      groupField.AddParameter ( Evado.UniForm.Model.EuFieldParameters.Snd_Cmd_On_Change, 1 );

      // 
      // Create a custom groupCommand to process the selection.
      // 
      Evado.UniForm.Model.EuCommand customCommand = pageGroup.addCommand (
        EdLabels.Organisation_Selection_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Organisations.ToString ( ),
        Evado.UniForm.Model.EuMethods.Custom_Method );

      // 
      // Set the custom groupCommand parameter.
      // 
      customCommand.setCustomMethod ( Evado.UniForm.Model.EuMethods.List_of_Objects );


      this.LogMethodEnd ( "getOrganisationSelection" );

    }//END getOrganisationSelection method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.EuPage object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    public void getListGroup (
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "getListGroup" );

      // 
      // Create the new pageMenuGroup.
      // 
      Evado.UniForm.Model.EuGroup pageGroup = PageObject.AddGroup (
        EdLabels.Organisation_List_Group_Title );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;
      pageGroup.CmdLayout = Evado.UniForm.Model.EuGroupCommandListLayouts.Vertical_Orientation;

      // 
      // Add the save groupCommand
      // 
      Evado.UniForm.Model.EuCommand groupCommand = pageGroup.addCommand (
        EdLabels.Organisation_New_Command_Title,
        EuAdapter.ADAPTER_ID,
        EuAdapterClasses.Organisations.ToString ( ),
        Evado.UniForm.Model.EuMethods.Create_Object );

      // 
      // Define the save and delete groupCommand parameters
      // 
      groupCommand.AddParameter ( EuOrganisations.CONST_NEW_FIELD_ID, "true" );

      groupCommand.SetBackgroundColour (
        Evado.UniForm.Model.EuCommandParameters.BG_Default,
        Evado.UniForm.Model.EuBackgroundColours.Purple );

      // 
      // generate the page links.
      // 
      foreach ( EdOrganisation organisation in this.Session.AdminOrganisationList )
      {
        // 
        // Add the trial organisation to the list of organisations as a groupCommand.
        // 
        Evado.UniForm.Model.EuCommand command = pageGroup.addCommand (
          organisation.LinkText,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Organisations.ToString ( ),
          Evado.UniForm.Model.EuMethods.Get_Object );

        command.Id = organisation.Guid;
        command.SetGuid ( organisation.Guid );

      }//END organisation list iteration loop

      this.LogValue ( "pageGroup.CommandList.Count {0}. ", pageGroup.CommandList.Count );

      this.LogMethodEnd ( "getListGroup" );

    }//END getListGroup method.


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class get object methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData getObject (
      Evado.UniForm.Model.EuCommand PageCommand )
    {
      this.LogMethod ( "getObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );
      Guid orgGuid = Guid.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getObject",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

        this.LogMethodEnd ( "getObject" );
        return this.Session.LastPage;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + "getObject",
        this.Session.UserProfile );

      //
      // get the organisation guid.
      //
      orgGuid = PageCommand.GetGuid ( );

      //
      // guid and object empty.
      //
      if ( orgGuid == Guid.Empty
        && this.Session.AdminOrganisation.Guid == Guid.Empty )
      {
        this.ErrorMessage = EdLabels.Organisation_Guid_Empty_Message;
        this.LogMethodEnd ( "getObject" );
        return this.Session.LastPage;
      }

      //
      // load the organisation
      //
      if ( this.getOrganisation ( orgGuid ) == false )
      {
        this.Session.LastPage.Message = this.ErrorMessage;
        this.LogMethodEnd ( "getObject" );
        return this.Session.LastPage;
      }

      // 
      // return the client ResultData object for the customer.
      // 
      this.getDataObject ( clientDataObject );

      //
      // return the client data object.
      //
      this.LogMethodEnd ( "getObject" );
      return clientDataObject;

    }//END getObject method

    // ==============================================================================
    /// <summary>
    /// This method loads the admin organisation
    /// </summary>
    /// <param name="OrgGuid">Guid object identifier.</param>
    //  ------------------------------------------------------------------------------
    private bool getOrganisation ( Guid OrgGuid )
    {
      this.LogMethod ( "getOrganisation" );
      this.LogValue ( "OrgGuid: " + OrgGuid );
      // 
      // if the parameter value exists then set the customerId
      // 
      try
      {
        //
        // if the organisation is loaded or not matching the passed guid.
        //
        if ( this.Session.AdminOrganisation.Guid != Guid.Empty
          && this.Session.AdminOrganisation.Guid == OrgGuid )
        {
          this.LogMethodEnd ( "getOrganisation" );
          return true;
        }

        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        this.Session.AdminOrganisation = this._Bll_Organisations.getItem ( OrgGuid );

        this.LogValue ( this._Bll_Organisations.Log );

        if ( this.Session.AdminOrganisation.OrgType == String.Empty )
        {
          this.Session.AdminOrganisation.OrgType = this.AdapterObjects.Settings.DefaultOrgType;
        }
        // 
        // Create the organisation fax number object
        // 
        this.LogDebug ( "AdminOrganisation.OrgType: " + this.Session.AdminOrganisation.OrgType );

        this.LogValue ( "Organisation.OrgId: "
          + this.Session.AdminOrganisation.OrgId );

        this.LogMethodEnd ( "getObject" );
        return true;
      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Organisation_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );

        this.LogMethodEnd ( "getOrganisation" );
        return false;
      }

    }//END getOrganisation method.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.UniForm.Model.EuAppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject ( Evado.UniForm.Model.EuAppData ClientDataObject )
    {
      this.LogMethod ( "getDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );
      Evado.UniForm.Model.EuField pageField = new Evado.UniForm.Model.EuField ( );

      ClientDataObject.Id = this.Session.AdminOrganisation.Guid;
      ClientDataObject.Title = EdLabels.Organisation_Page_Title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Disabled;

      //
      // Set the user edit access to the objects.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;
      }
      this.LogValue ( "Page.EditAccess: " + ClientDataObject.Page.EditAccess );

      //
      // Add the page commands 
      //
      this.getDataObject_PageCommands ( ClientDataObject.Page );

      //
      // Add the detail group to the page.
      //
      this.getDataObject_DetailsGroup ( ClientDataObject.Page );

      this.LogMethodEnd ( "getDataObject" );

    }//END Method

    //================================================================================
    /// <summary>
    /// This method add the group commands to the grop.
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.EuGroup object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_PageCommands (
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "getDataObject_PageCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );

      // 
      // Add the save groupCommand
      // 
      if ( PageObject.EditAccess == Evado.UniForm.Model.EuEditAccess.Enabled )
      {
        //
        // save command.
        //
        pageCommand = PageObject.addCommand (
          EdLabels.Organisation_Save_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Organisations.ToString ( ),
          Evado.UniForm.Model.EuMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.AdminOrganisation.Guid );
        pageCommand.AddParameter (
           Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
          EdOrganisation.ActionCodes.Save.ToString ( ) );

        this.LogDebug ( "Org GUid {0}.", this.Session.AdminOrganisation.Guid );
        this.LogDebug ( "OrgType {0}.", this.Session.AdminOrganisation.OrgType );
        //
        // Delete command
        //
        if ( this.Session.AdminOrganisation.Guid != EvStatics.CONST_NEW_OBJECT_ID
          && this.Session.AdminOrganisation.OrgType != "Evado" )
        {
          pageCommand = PageObject.addCommand (
            EdLabels.Organisation_Delete_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Organisations.ToString ( ),
            Evado.UniForm.Model.EuMethods.Save_Object );

          // 
          // Define the save and delete groupCommand parameters
          // 
          pageCommand.SetGuid ( this.Session.AdminOrganisation.Guid );
          pageCommand.AddParameter (
             Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
            EdOrganisation.ActionCodes.Delete_Object.ToString ( ) );
        }
      }

      this.LogMethodEnd ( "getDataObject_PageCommands" );

    }//END getDataObject_GroupCommands Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.UniForm.Model.EuAppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject_DetailsGroup (
      Evado.UniForm.Model.EuPage PageObject )
    {
      this.LogMethod ( "getDataObject_DetailsGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuField pageField = new Evado.UniForm.Model.EuField ( );
      List<EvOption> optionList = new List<EvOption> ( );
      Evado.UniForm.Model.EuEditAccess adminAccess = Evado.UniForm.Model.EuEditAccess.Disabled;

      if ( this.Session.UserProfile.hasAdministrationAccess == true )
      {
        adminAccess = Evado.UniForm.Model.EuEditAccess.Enabled;
      }

      // 
      // create the page pageMenuGroup
      // 
      Evado.UniForm.Model.EuGroup pageGroup = PageObject.AddGroup (
        String.Empty );
      pageGroup.Layout = Evado.UniForm.Model.EuGroupLayouts.Full_Width;
      pageGroup.EditAccess = Evado.UniForm.Model.EuEditAccess.Enabled;

      //
      // Add the group commands
      //
      this.getDataObject_GroupCommands ( pageGroup );

      // 
      // Create the customer id object
      // 
      pageField = pageGroup.createTextField (
        EdOrganisation.FieldNames.OrgId.ToString ( ),
        EdLabels.Label_Organisation_Id,
        String.Empty,
        this.Session.AdminOrganisation.OrgId, 10 );
      pageField.EditAccess = Evado.UniForm.Model.EuEditAccess.Disabled;
      pageField.Layout = EuAdapter.DefaultFieldLayout;
      pageField.EditAccess = adminAccess;

      //
      // get the org type selection list.
      //
      optionList = this.AdapterObjects.Settings.GetOrgTypeList ( true );

      //
      // Generate the organisation type radio button list field object.
      //
      pageField = pageGroup.createSelectionListField (
        EdOrganisation.FieldNames.Org_Type.ToString ( ),
        EdLabels.Organisation_Type_Field_Label,
        EdLabels.Organisation_Type_Field_Description,
        this.Session.AdminOrganisation.OrgType,
        optionList );
      pageField.Layout = EuAdapter.DefaultFieldLayout;
      pageField.Mandatory = true;
      pageField.EditAccess = adminAccess;

      if ( this.Session.AdminOrganisation.OrgType == String.Empty )
      {
        pageField.setBackgroundColor (
          Evado.UniForm.Model.EuFieldParameters.BG_Mandatory,
          Evado.UniForm.Model.EuBackgroundColours.Red );
      }

      // 
      // Create the customer name object
      // 
      pageField = pageGroup.createTextField (
        EdOrganisation.FieldNames.Name.ToString ( ),
        EdLabels.Organisation_Name_Field_Label,
        this.Session.AdminOrganisation.Name,
        50 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;
      pageField.Mandatory = true;

      pageField.setBackgroundColor (
        Evado.UniForm.Model.EuFieldParameters.BG_Mandatory,
        Evado.UniForm.Model.EuBackgroundColours.Red );

      // 
      // Create the customer name object
      //
      if ( this.AdapterObjects.Settings.hasHiddenOrganisationField ( EdOrganisation.FieldNames.Image_File_Name ) == false )
      {
        pageField = pageGroup.createImageField (
          EdOrganisation.FieldNames.Image_File_Name.ToString ( ),
          EdLabels.Organisation_ImageFileame_Field_Label,
          this.Session.AdminOrganisation.ImageFileName,
          300, 200 );
        pageField.Layout = EuAdapter.DefaultFieldLayout;

        try
        {
          String stTargetPath = this.UniForm_BinaryFilePath + this.Session.AdminOrganisation.ImageFileName;
          String stImagePath = this.UniForm_ImageFilePath + this.Session.AdminOrganisation.ImageFileName;

          this.LogDebug ( "Target path {0}.", stTargetPath );
          this.LogDebug ( "Image path {0}.", stImagePath );

          //
          // copy the file into the image directory.
          //
          System.IO.File.Copy ( stImagePath, stTargetPath, true );
        }
        catch { }
      }
      else
      {
        this.Session.Organisation.ImageFileName = String.Empty;
      }

      // 
      // Create the street address 1
      //
      if ( this.AdapterObjects.Settings.hasHiddenOrganisationField (
        EdOrganisation.FieldNames.Address_1 ) == false )
      {
        pageField = pageGroup.createTextField (
          EdOrganisation.FieldNames.Address_1,
          EdLabels.Organisation_Address_Street_Field_Label,
          this.Session.AdminOrganisation.AddressStreet_1, 50 );
        pageField.Layout = EuAdapter.DefaultFieldLayout;

        // 
        // Create the street address 2
        // 
        pageField = pageGroup.createTextField (
          EdOrganisation.FieldNames.Address_2,
          EdLabels.Organisation_Address_Street_Field_Label,
          this.Session.AdminOrganisation.AddressStreet_2, 50 );
        pageField.Layout = EuAdapter.DefaultFieldLayout;
      }

      // 
      // Create the street address city
      // 
      if ( this.AdapterObjects.Settings.hasHiddenOrganisationField (
        EdOrganisation.FieldNames.Address_City ) == false )
      {
        pageField = pageGroup.createTextField (
          EdOrganisation.FieldNames.Address_City,
          EdLabels.Organisation_Address_City_Field_Label,
          this.Session.AdminOrganisation.AddressCity, 50 );
        pageField.Layout = EuAdapter.DefaultFieldLayout;
      }

      // 
      // Create the street address state
      // 
      if ( this.AdapterObjects.Settings.hasHiddenOrganisationField (
        EdOrganisation.FieldNames.Address_State ) == false )
      {
        pageField = pageGroup.createTextField (
          EdOrganisation.FieldNames.Address_State,
          EdLabels.Organisation_Address_State_Field_Label,
          this.Session.AdminOrganisation.AddressState, 10 );
        pageField.Layout = EuAdapter.DefaultFieldLayout;
      }

      // 
      // Create the street address 1
      // 
      if ( this.AdapterObjects.Settings.hasHiddenOrganisationField (
        EdOrganisation.FieldNames.Address_Post_Code ) == false )
      {
        pageField = pageGroup.createTextField (
          EdOrganisation.FieldNames.Address_Post_Code,
          EdLabels.Organisation_Address_City_Field_Label,
          this.Session.AdminOrganisation.AddressPostCode, 50 );
        pageField.Layout = EuAdapter.DefaultFieldLayout;
      }

      // 
      // Create the street address 1
      // 
      if ( this.AdapterObjects.Settings.hasHiddenOrganisationField (
        EdOrganisation.FieldNames.Address_Country ) == false )
      {
        pageField = pageGroup.createTextField (
          EdOrganisation.FieldNames.Address_Country,
          EdLabels.Organisation_Address_Country_Field_Label,
          this.Session.AdminOrganisation.AddressCountry, 50 );
        pageField.Layout = EuAdapter.DefaultFieldLayout;
      }

      // 
      // Create the organisation telephone number object
      // 
      if ( this.AdapterObjects.Settings.hasHiddenOrganisationField (
        EdOrganisation.FieldNames.Telephone ) == false )
      {
        pageField = pageGroup.createTelephoneNumberField (
          EdOrganisation.FieldNames.Telephone.ToString ( ),
          EdLabels.Organisation_Telephone_Field_Label,
          this.Session.AdminOrganisation.Telephone );
        pageField.Layout = EuAdapter.DefaultFieldLayout;
      }

      // 
      // Create the organisation fax number object
      // 
      if ( this.AdapterObjects.Settings.hasHiddenOrganisationField (
        EdOrganisation.FieldNames.Email_Address ) == false )
      {
        pageField = pageGroup.createEmailAddressField (
          EdOrganisation.FieldNames.Email_Address.ToString ( ),
          EdLabels.Organisation_Email_Field_Label,
          this.Session.AdminOrganisation.EmailAddress );
        pageField.Layout = EuAdapter.DefaultFieldLayout;
      }

      this.LogMethodEnd ( "getDataObject_DetailsGroup" );

    }//END getDataObject_DetailsGroup Method


    //================================================================================
    /// <summary>
    /// This method add the group commands to the grop.
    /// </summary>
    /// <param name="PageGroup">Evado.UniForm.Model.EuGroup object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_GroupCommands (
      Evado.UniForm.Model.EuGroup PageGroup )
    {
      this.LogMethod ( "getDataObject_GroupCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.UniForm.Model.EuCommand pageCommand = new Evado.UniForm.Model.EuCommand ( );

      // 
      // Add the save groupCommand
      // 
      if ( PageGroup.EditAccess == Evado.UniForm.Model.EuEditAccess.Enabled )
      {
        pageCommand = PageGroup.addCommand (
          EdLabels.Organisation_Save_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Organisations.ToString ( ),
          Evado.UniForm.Model.EuMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.AdminOrganisation.Guid );
        pageCommand.AddParameter (
           Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
          EdOrganisation.ActionCodes.Save.ToString ( ) );


        this.LogDebug ( "Org GUid {0}.", this.Session.AdminOrganisation.Guid );
        this.LogDebug ( "OrgType {0}.", this.Session.AdminOrganisation.OrgType );
        //
        // Delete command
        //
        if ( this.Session.AdminOrganisation.Guid != EvStatics.CONST_NEW_OBJECT_ID
          && this.Session.AdminOrganisation.OrgType != EdAdapterSettings.EVADO_ORGANISATION )
        {
          pageCommand = PageGroup.addCommand (
            EdLabels.Organisation_Delete_Command_Title,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Organisations.ToString ( ),
            Evado.UniForm.Model.EuMethods.Save_Object );

          // 
          // Define the save and delete groupCommand parameters
          // 
          pageCommand.SetGuid ( this.Session.AdminOrganisation.Guid );
          pageCommand.AddParameter (
             Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION,
            EdOrganisation.ActionCodes.Delete_Object.ToString ( ) );
        }
      }
      this.LogMethodEnd ( "getDataObject_GroupCommands" );

    }//END getDataObject_GroupCommands Method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class create object methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Command">Evado.UniForm.Model.ClientClientDataObjectEvado.UniForm.Model.EuCommand object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData createObject ( Evado.UniForm.Model.EuCommand Command )
    {
      this.LogMethod ( "createObject" );
      try
      {
        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "createObject",
          this.Session.UserProfile );

        // 
        // Initialise the methods variables and objects.
        //      
        Evado.UniForm.Model.EuAppData clientDataObject = new Evado.UniForm.Model.EuAppData ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasAdministrationAccess == false )
        {
          this.LogIllegalAccess ( "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage; ;
        }

        //
        // Initialise the dlinical ResultData objects.
        //
        this.Session.AdminOrganisation = new EdOrganisation ( );
        this.Session.AdminOrganisation.Guid = Evado.Digital.Model.EvcStatics.CONST_NEW_OBJECT_ID;
        this.Session.AdminOrganisation.OrgId = String.Empty;
        this.Session.AdminOrganisation.ImageFileName = String.Empty;
        this._CurrentFileName = String.Empty;

        this.getDataObject ( clientDataObject );


        this.LogValue ( "Exit createObject method. ID: "
          + clientDataObject.Id + ", Title: " + clientDataObject.Title );

        return clientDataObject;

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = "Error raised when creating a trial site.";

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage;

    }//END method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class update object methods

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectEvado.UniForm.Model.EuCommand object.</param>
    /// <remarks>
    /// This method has following steps:
    /// 
    /// 1. Update the object values from command parameter values.
    /// 
    /// 2. Update the address fields of the customer.
    /// 
    /// 3. Save the updated fields to the respective tables in Evado Database.
    /// </remarks>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.UniForm.Model.EuAppData updateObject ( Evado.UniForm.Model.EuCommand PageCommand )
    {
      try
      {
        this.LogMethod ( "updateObject" );
        this.LogDebug ( "PageCommand: " + PageCommand.getAsString ( false, true ) );

        this.LogDebug ( "AdminOrganisation" );
        this.LogDebug ( "-Guid: " + this.Session.AdminOrganisation.Guid );
        this.LogDebug ( "-OrgId: " + this.Session.AdminOrganisation.OrgId );
        this.LogDebug ( "-Title: " + this.Session.AdminOrganisation.Name );
        EdOrganisation.ActionCodes saveAction = EdOrganisation.ActionCodes.Save;
        this._CurrentFileName = this.Session.AdminOrganisation.ImageFileName;

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );

        // 
        // Initialise the update variables.
        // 
        this.Session.AdminOrganisationList = new List<EdOrganisation> ( );

        // 
        // IF the guid is new object id  alue then set the save object for adding to the database.
        // 
        if ( this.Session.AdminOrganisation.Guid == Evado.Digital.Model.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.AdminOrganisation.Guid = Guid.Empty;
        }

        // 
        // Delete the object.
        // 
        if ( PageCommand.Method == Evado.UniForm.Model.EuMethods.Delete_Object )
        {
          return new Evado.UniForm.Model.EuAppData ( );
        }

        // 
        // Update the object.
        // 
        this.updateObjectValue ( PageCommand.Parameters );


        //
        // check that the mandatory fields have been filed.
        //
        if ( this.updateCheckMandatory ( ) == false )
        {
          return this.Session.LastPage;
        }

        //
        // copy the image file to the image directory.
        //
        this.saveImageFile ( );

        this.LogDebug ( "AddressStreet_1: " + this.Session.AdminOrganisation.AddressStreet_1 );
        this.LogDebug ( "AddressStreet_2: " + this.Session.AdminOrganisation.AddressStreet_2 );
        this.LogDebug ( "AddressCity: " + this.Session.AdminOrganisation.AddressCity );
        this.LogDebug ( "AddressState: " + this.Session.AdminOrganisation.AddressState );
        this.LogDebug ( "AddressCountry: " + this.Session.AdminOrganisation.AddressCountry );
        this.LogDebug ( "AddressPostCode: " + this.Session.AdminOrganisation.AddressPostCode );

        // 
        // Get the save action message value.
        // 
        String stSaveAction = PageCommand.GetParameter ( Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION );

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        if ( stSaveAction != String.Empty )
        {
          saveAction = Evado.Model.EvStatics.parseEnumValue<EdOrganisation.ActionCodes> ( stSaveAction );
        }
        this.Session.AdminOrganisation.Action = saveAction;


        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_Organisations.saveItem ( this.Session.AdminOrganisation );

        // 
        // get the debug ResultData.
        // 
        this.LogValue ( this._Bll_Organisations.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._Bll_Organisations.Log + " returned error message: " + Evado.Digital.Model.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          switch ( result )
          {
            case EvEventCodes.Data_Duplicate_Id_Error:
              {
                this.ErrorMessage =
                  String.Format (
                    EdLabels.Organisation_Duplicate_Error_Message,
                    this.Session.AdminOrganisation.OrgId );
                break;
              }
            case EvEventCodes.Identifier_Org_Id_Error:
              {
                this.ErrorMessage = EdLabels.Organisation_Identifier_Empty_Error_Message;
                break;
              }
            default:
              {
                this.ErrorMessage = EdLabels.Organisation_Update_Error_Message;
                break;
              }
          }
          return this.Session.LastPage;
        }//END save error returned.

        this.Session.AdminOrganisationList = new List<EdOrganisation> ( );
        this.AdapterObjects.OrganisationList = new List<EdOrganisation> ( );

        return new Evado.UniForm.Model.EuAppData ( );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EdLabels.Organisation_Update_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }
      return this.Session.LastPage;

    }//END method

    // ==================================================================================
    /// <summary>
    /// THis method copies the upload image file to the image directory.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    private void saveImageFile ( )
    {
      this.LogMethod ( "saveImageFile" );

      if ( this.Session.AdminOrganisation.ImageFileName == String.Empty )
      {
        this.LogDebug ( "No Image" );
        this.LogMethodEnd ( "saveImageFile" );
        return;
      }
      if ( this._CurrentFileName == this.Session.AdminOrganisation.ImageFileName )
      {
        return;
      }
      //
      // Initialise the method variables and objects.
      //
      String stSourcePath = this.UniForm_BinaryFilePath + this.Session.AdminOrganisation.ImageFileName;
      String stImagePath = this.UniForm_ImageFilePath + this.Session.AdminOrganisation.ImageFileName;

      this.LogDebug ( "Source path {0}.", stSourcePath );
      this.LogDebug ( "Image path {0}.", stImagePath );

      //
      // Save the file to the directory repository.
      //
      System.IO.File.Copy ( stSourcePath, stImagePath, true );

      this.LogMethodEnd ( "saveImageFile" );
    }//END saveImageFile method

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.EuCommand object.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private bool updateCheckMandatory ( )
    {
      this.LogMethod ( "updateCheckMandatory" );
      //
      // Define the methods variables and objects.
      //
      bool bReturn = true;
      this.ErrorMessage = String.Empty;

      //
      // Org type not defined.
      //
      if ( this.Session.AdminOrganisation.OrgType == String.Empty )
      {
        if ( this.ErrorMessage != String.Empty )
        {
          this.ErrorMessage += "\r\n ";
        }
        this.ErrorMessage += EdLabels.Organisation_Org_Type_Error_Message;

        bReturn = false;
      }

      //
      // Org name not defined.
      //
      if ( this.Session.AdminOrganisation.Name == String.Empty )
      {
        if ( this.ErrorMessage != String.Empty )
        {
          this.ErrorMessage += "\r\n ";
        }
        this.ErrorMessage += EdLabels.Organisation_Org_Name_Error_Message;

        bReturn = false;
      }

      return bReturn;
    }

    // ==================================================================================
    /// <summary>
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="Parameters">List of field values to be updated.</param>
    /// <returns></returns>
    //  ----------------------------------------------------------------------------------
    private void updateObjectValue (
      List<Evado.UniForm.Model.EuParameter> Parameters )
    {
      this.LogMethod ( "updateObjectValue" );
      this.LogDebug ( "Parameters.Count: " + Parameters.Count );
      this.LogDebug ( "Customer.Guid: " + this.Session.AdminOrganisation.Guid );

      /// 
      /// Iterate through the parameter values updating the ResultData object
      /// 
      foreach ( Evado.UniForm.Model.EuParameter parameter in Parameters )
      {
        if ( parameter.Name.Contains ( Evado.Digital.Model.EvcStatics.CONST_GUID_IDENTIFIER ) == false
          && parameter.Name != Evado.UniForm.Model.EuCommandParameters.Custom_Method.ToString ( )
          && parameter.Name != Evado.Digital.Model.EvcStatics.CONST_SAVE_ACTION
          && parameter.Name != EuOrganisations.CONST_CURRENT_FIELD_ID )
        {
          this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );
          try
          {
            EdOrganisation.FieldNames fieldName =
               Evado.Model.EvStatics.parseEnumValue<EdOrganisation.FieldNames> (
              parameter.Name );

            this.Session.AdminOrganisation.setValue ( fieldName, parameter.Value );

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

      this.LogMethodEnd ( "updateObjectValue" );
    }//END updateObjectValue method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace