/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Organisations.cs" company="EVADO HOLDING PTY. LTD.">
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
using Evado.Bll.Clinical;
using  Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Digital
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
      EuAdapterObjects AdapterObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = "Evado.UniForm.Digital.EuOrganisations.";
      this.GlobalObjects = AdapterObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;


      this.LogInitMethod ( "EuOrganisations initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );

      this.LogInit ( "Settings:" );
      this.LogInit ( "-PlatformId: " + this.ClassParameters.PlatformId );
      this.LogInit ( "-ApplicationGuid: " + this.ClassParameters.AdapterGuid );
      this.LogInit ( "-LoggingLevel: " + Settings.LoggingLevel );
      this.LogInit ( "-UserId: " + Settings.UserProfile.UserId );
      this.LogInit ( "-UserCommonName: " + Settings.UserProfile.CommonName );

      this._Bll_Organisations = new Evado.Bll.Clinical.EdOrganisations ( this.ClassParameters );

    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.
    
    private const String CONST_CURRENT_FIELD_ID = "CURRENT";
    private const String CONST_NEW_FIELD_ID = "NEW";

    private Evado.Bll.Clinical.EdOrganisations _Bll_Organisations = new Evado.Bll.Clinical.EdOrganisations ( );

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    // ==================================================================================
    /// <summary>
    /// This method gets the trial site object.
    /// 
    /// </summary>
    /// <param name="PageCommand">ClientPateEvado.Model.UniForm.Command object</param>
    /// <returns>ClientApplicationData</returns>
    //  ----------------------------------------------------------------------------------
    public Evado.Model.UniForm.AppData getClientDataObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ("getClientDataObject" );
      this.LogValue ( "PageCommand Content: " + PageCommand.getAsString ( false, false ) );
      try
      {
        // 
        // Initialise the methods variables and objects.
        // 
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        // 
        // Determine the method to be called
        // 
        switch ( PageCommand.Method )
        {
          case Evado.Model.UniForm.ApplicationMethods.List_of_Objects:
            {
              clientDataObject = this.getListObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Get_Object:
            {
              clientDataObject = this.getObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Create_Object:
            {
              clientDataObject = this.createObject ( PageCommand );
              break;
            }
          case Evado.Model.UniForm.ApplicationMethods.Save_Object:
          case Evado.Model.UniForm.ApplicationMethods.Delete_Object:
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

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    public Evado.Model.UniForm.AppData getListObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      try
      {
        this.LogValue (  Evado.Model.UniForm.EuStatics.CONST_METHOD_START
          + " Evado.UniForm.Clinical.Organisations.getListObject" );
        // 
        // Initialise the methods variables and objects.
        //      
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasAdministrationAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

           return this.Session.LastPage;;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        clientDataObject.Title = EdLabels.Organisation_List_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );

        if ( this.GlobalObjects.HelpUrl != String.Empty )
        {/**/
          Evado.Model.UniForm.Command helpCommand = clientDataObject.Page.addCommand (
           EdLabels.Label_Help_Command_Title,
           EuAdapter.ADAPTER_ID,
           EuAdapterClasses.Organisations.ToString ( ),
           Model.UniForm.ApplicationMethods.Get_Object );

          helpCommand.Type = Evado.Model.UniForm.CommandTypes.Html_Link;

          helpCommand.AddParameter ( Model.UniForm.CommandParameters.Link_Url,
           EvcStatics.createHelpUrl( 
            this.GlobalObjects.HelpUrl, 
             Evado.Model.Digital.EvPageIds.Organisation_View ) );
        
        }


        // 
        // Add the trial organisation list to the page.
        // 
        this.getListGroup ( clientDataObject.Page );

        this.LogValue ( "data.Title: " + clientDataObject.Title );
        this.LogValue ( "data.Page.Title: " + clientDataObject.Page.Title );


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

       return this.Session.LastPage;;

    }//END getListObject method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class list methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    public void getListGroup ( 
      Evado.Model.UniForm.Page PageObject )
    {
      try
      {
        this.LogValue (  Evado.Model.UniForm.EuStatics.CONST_METHOD_START
          + " Evado.UniForm.Clinical.Organisations.getListGroup" );

        // 
        // Create the new pageMenuGroup.
        // 
        Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
          EdLabels.Organisation_List_Group_Title );
        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
        pageGroup.Title = EdLabels.Organisation_List_Group_Title;


        // 
        // Add the save groupCommand
        // 
        Evado.Model.UniForm.Command groupCommand = pageGroup.addCommand (
          EdLabels.Organisation_New_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Organisations.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Create_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        groupCommand.AddParameter ( EuOrganisations.CONST_NEW_FIELD_ID, "true" );

        groupCommand.SetBackgroundColour (
          Model.UniForm.CommandParameters.BG_Default,
          Model.UniForm.Background_Colours.Purple );

        // 
        // get the list of customers.
        // 
        if ( this.Session.OrganisationList.Count == 0 )
        {
          this.Session.OrganisationList = this._Bll_Organisations.getView ( );
          this.LogValue ( this._Bll_Organisations.Log );
        }
        this.LogValue ( "list count: " + this.Session.OrganisationList.Count );
        // 
        // generate the page links.
        // 
        foreach ( EvOrganisation organisation in this.Session.OrganisationList )
        {
          // 
          // Add the trial organisation to the list of organisations as a groupCommand.
          // 
          Evado.Model.UniForm.Command command = pageGroup.addCommand (
            organisation.LinkText,
            EuAdapter.ADAPTER_ID,
            EuAdapterClasses.Organisations.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Get_Object );
          
          command.Id = organisation.Guid;
          command.SetGuid ( organisation.Guid );

        }//END trial organisation list iteration loop

        this.LogValue ( "command count: " + pageGroup.CommandList.Count );

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

    }//END getListObject method.

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData getObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogValue (  Evado.Model.UniForm.EuStatics.CONST_METHOD_START
        + this.ClassNameSpace + "getObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid OrgGuid = Guid.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess== false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

         return this.Session.LastPage;;
      }

      // 
      // Log access to page.
      // 
      this.LogPageAccess (
        this.ClassNameSpace + "getObject",
        this.Session.UserProfile );

      // 
      // if the parameter value exists then set the customerId
      // 
      OrgGuid = PageCommand.GetGuid ( );
      this.LogValue ( "OrgGuid: " + OrgGuid );

      // 
      // return if not trial id
      // 
      if ( OrgGuid == Guid.Empty )
      {
        this.LogValue ( "Guid Empty get current object" );

        if ( this.Session.AdminOrganisation.Guid != Guid.Empty )
        {
          // 
          // return the client ResultData object for the customer.
          // 
          this.getDataObject ( clientDataObject );
        }
        else
        {
          this.LogValue ( "ERROR: current organisation guid empty" );
          this.ErrorMessage = EdLabels.Organisation_Guid_Empty_Message;
        }

        return clientDataObject;
      }
      this.LogValue ( "Query site Guid: " + OrgGuid );

      try
      {
        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        this.Session.AdminOrganisation = this._Bll_Organisations.getItem ( OrgGuid );

        this.LogValue ( this._Bll_Organisations.Log );

        this.LogValue ( "SessionObjects.Organisation.OrgId: "
          + this.Session.AdminOrganisation.OrgId );

        // 
        // Save the customer object to the session
        // 
         

        // 
        // return the client ResultData object for the customer.
        // 
        this.getDataObject ( clientDataObject );

        this.LogValue ( "Page.Title: " + clientDataObject.Page.Title );

        return clientDataObject;
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
      }

       return this.Session.LastPage;;

    }//END getObject method

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class get object methods

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="ClientDataObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject ( Evado.Model.UniForm.AppData ClientDataObject )
    {
      this.LogMethod ( "getDataObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );

      ClientDataObject.Id = this.Session.AdminOrganisation.Guid;
      ClientDataObject.Title = EdLabels.Organisation_Page_Title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Set the user edit access to the objects.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
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
    /// <param name="PageObject">Evado.Model.UniForm.Group object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_PageCommands (
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_PageCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      //
      // Add the help button if the help url is defined.
      //
      if ( this.GlobalObjects.HelpUrl != String.Empty )
      {
        pageCommand = PageObject.addCommand (
         EdLabels.Label_Help_Command_Title,
         EuAdapter.ADAPTER_ID,
         EuAdapterClasses.Organisations.ToString ( ),
         Model.UniForm.ApplicationMethods.Get_Object );

        pageCommand.Type = Evado.Model.UniForm.CommandTypes.Html_Link;

        pageCommand.AddParameter ( Model.UniForm.CommandParameters.Link_Url,
           EvcStatics.createHelpUrl (
            this.GlobalObjects.HelpUrl,
             Evado.Model.Digital.EvPageIds.Organisation_Page ) );
      }

      // 
      // Add the save groupCommand
      // 
      if ( PageObject.EditAccess == Evado.Model.UniForm.EditAccess.Enabled )
      {
        //
        // save command.
        //
        pageCommand = PageObject.addCommand (
          EdLabels.Organisation_Save_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Organisations.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.Organisation.Guid );
        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EvOrganisation.ActionCodes.Save.ToString ( ) );

        //
        // Delete command
        //
        pageCommand = PageObject.addCommand (
          EdLabels.Organisation_Delete_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Organisations.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.Organisation.Guid );
        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EvOrganisation.ActionCodes.Delete_Object.ToString ( ) );
      }

      this.LogMethodEnd ( "getDataObject_PageCommands" );

    }//END getDataObject_GroupCommands Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.AppData object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getDataObject_DetailsGroup ( 
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getDataObject_DetailsGroup" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        String.Empty );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Add the group commands
      //
      this.getDataObject_GroupCommands ( pageGroup );

      // 
      // Create the customer id object
      // 
      pageField = pageGroup.createTextField (
        EvOrganisation.OrganisationFieldNames.OrgId.ToString ( ),
        EdLabels.Label_Organisation_Id,
        String.Empty,
        this.Session.AdminOrganisation.OrgId, 10 );
      pageField.EditAccess = Evado.Model.UniForm.EditAccess.Disabled;
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the organisation fax number object
      // 
      this.LogDebug ( "AdminOrganisation.OrgType: "
        + this.Session.AdminOrganisation.OrgType );

      //
      // Generate the organisation type list.
      //
      List<EvOption> typeList = EvOrganisation.getOrganisationTypeList (
        this.Session.AdminOrganisation.OrgType );

      //
      // Generate the organisation type radio button list field object.
      //
      pageField = pageGroup.createRadioButtonListField (
        EvOrganisation.OrganisationFieldNames.Org_Type.ToString ( ),
        EdLabels.Organisation_Type_Field_Label,
        EdLabels.Organisation_Type_Field_Description,
        Evado.Model.EvStatics.getEnumStringValue ( this.Session.AdminOrganisation.OrgType ),
        typeList );
      pageField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      pageField.Layout = EuAdapter.DefaultFieldLayout;
      pageField.Mandatory = true;

      if ( this.Session.UserProfile.hasAdministrationAccess == true )
      {
        pageField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

        pageField.setBackgroundColor (
          Model.UniForm.FieldParameterList.BG_Mandatory,
          Model.UniForm.Background_Colours.Red );
      }

      // 
      // Create the customer name object
      // 
      pageField = pageGroup.createTextField (
        EvOrganisation.OrganisationFieldNames.Name.ToString ( ),
        EdLabels.Organisation_Name_Field_Label,
        this.Session.AdminOrganisation.Name,
        50 );
      pageField.Layout = EuAdapter.DefaultFieldLayout;
      pageField.Mandatory = true;

      pageField.setBackgroundColor (
        Model.UniForm.FieldParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      // 
      // Create the customer name object
      // 
      pageField = pageGroup.createAddressField (
        EvOrganisation.OrganisationFieldNames.Address,
        EdLabels.Organisation_Address_Field_Label,
        this.Session.AdminOrganisation.Address );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the organisation telephone number object
      // 
      pageField = pageGroup.createTelephoneNumberField (
        EvOrganisation.OrganisationFieldNames.Telephone.ToString ( ),
        EdLabels.Organisation_Telephone_Field_Label,
        this.Session.AdminOrganisation.Telephone );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the organisation fax number object
      // 
      pageField = pageGroup.createTelephoneNumberField (
        EvOrganisation.OrganisationFieldNames.Fax_Phone.ToString ( ),
        EdLabels.Organisation_Fax_Number_Field_Label,
        this.Session.AdminOrganisation.FaxPhone );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the organisation fax number object
      // 
      pageField = pageGroup.createEmailAddressField (
        EvOrganisation.OrganisationFieldNames.Email_Address.ToString ( ),
        EdLabels.Organisation_Email_Field_Label,
        this.Session.AdminOrganisation.EmailAddress );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      // 
      // Create the organisation current object
      // 
      pageField = pageGroup.createBooleanField (
        EuOrganisations.CONST_CURRENT_FIELD_ID,
        EdLabels.Organisation_Current_Field_Label,
        this.Session.AdminOrganisation.Current );
      pageField.Layout = EuAdapter.DefaultFieldLayout;

      this.LogMethodEnd ( "getDataObject_DetailsGroup" );

    }//END getDataObject_DetailsGroup Method


    //================================================================================
    /// <summary>
    /// This method add the group commands to the grop.
    /// </summary>
    /// <param name="PageGroup">Evado.Model.UniForm.Group object.</param>
    //  ------------------------------------------------------------------------------
    private void getDataObject_GroupCommands (
      Evado.Model.UniForm.Group PageGroup )
    {
      this.LogMethod ( "getDataObject_GroupCommands" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      // 
      // Add the save groupCommand
      // 
      if ( PageGroup.EditAccess == Evado.Model.UniForm.EditAccess.Enabled )
      {
        pageCommand = PageGroup.addCommand (
          EdLabels.Organisation_Save_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Organisations.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.Organisation.Guid );
        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EvOrganisation.ActionCodes.Save.ToString ( ) );

        //
        // Delete command
        //
        pageCommand = PageGroup.addCommand (
          EdLabels.Organisation_Delete_Command_Title,
          EuAdapter.ADAPTER_ID,
          EuAdapterClasses.Organisations.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        pageCommand.SetGuid ( this.Session.Organisation.Guid );
        pageCommand.AddParameter (
           Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
          EvOrganisation.ActionCodes.Delete_Object.ToString ( ) );
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
    /// <param name="Command">Evado.UniForm.Model.ClientClientDataObjectEvado.Model.UniForm.Command object.</param>
    /// <returns>Application Data object</returns>
    //  ----------------------------------------------------------------------------------
    private Evado.Model.UniForm.AppData createObject ( Evado.Model.UniForm.Command Command )
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
        Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );

        //
        // Determine if the user has access to this page and log and error if they do not.
        //
        if ( this.Session.UserProfile.hasAdministrationAccess == false )
        {
          this.LogIllegalAccess ( "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EdLabels.Illegal_Page_Access_Attempt;

           return this.Session.LastPage;;
        }

        //
        // Initialise the dlinical ResultData objects.
        //
        this.Session.AdminOrganisation = new EvOrganisation ( );
        this.Session.AdminOrganisation.Guid =   Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;

        this.LogDebug ( "Organisation CustomerGuid {0}. ", this.Session.AdminOrganisation.CustomerGuid );

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
    /// <param name="PageCommand">Evado.UniForm.Model.ClientClientDataObjectEvado.Model.UniForm.Command object.</param>
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
    private Evado.Model.UniForm.AppData updateObject ( Evado.Model.UniForm.Command PageCommand )
    {
      try
      {
        this.LogMethod ( "updateObject" );
        this.LogDebug( "PageCommand: " + PageCommand.getAsString ( false, true ) );

        this.LogDebug ( "AdminOrganisation"
          + " Guid: " + this.Session.AdminOrganisation.Guid
          + " OrgId: " + this.Session.AdminOrganisation.OrgId
          + " Title: " + this.Session.AdminOrganisation.Name );
        EvOrganisation.ActionCodes saveAction = EvOrganisation.ActionCodes.Save;

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );

        // 
        // Initialise the update variables.
        // 
        this.Session.AdminOrganisation.UpdatedByUserId = this.Session.UserProfile.UserId;
        this.Session.AdminOrganisation.UserCommonName = this.Session.UserProfile.CommonName;
        this.Session.OrganisationList = new List<EvOrganisation> ( );

        // 
        // IF the guid is new object id  alue then set the save object for adding to the database.
        // 
        if ( this.Session.AdminOrganisation.Guid ==  Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.AdminOrganisation.Guid = Guid.Empty;
        }

        // 
        // Delete the object.
        // 
        if ( PageCommand.Method == Evado.Model.UniForm.ApplicationMethods.Delete_Object )
        {
          return new Model.UniForm.AppData();
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


        this.LogDebug ( "AddressStreet_1: " + this.Session.AdminOrganisation.AddressStreet_1 );
        this.LogDebug ( "AddressStreet_2: " + this.Session.AdminOrganisation.AddressStreet_2 );
        this.LogDebug ( "AddressCity: " + this.Session.AdminOrganisation.AddressCity );
        this.LogDebug ( "AddressState: " + this.Session.AdminOrganisation.AddressState );
        this.LogDebug ( "AddressCountry: " + this.Session.AdminOrganisation.AddressCountry );
        this.LogDebug ( "AddressPostCode: " + this.Session.AdminOrganisation.AddressPostCode );

        // 
        // Get the save action message value.
        // 
        String stSaveAction = PageCommand.GetParameter (  Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        if ( stSaveAction != String.Empty )
        {
          saveAction =  Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvOrganisation.ActionCodes> ( stSaveAction );
        }
        this.Session.AdminOrganisation.Action = saveAction;


        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_Organisations.saveItem ( this.Session.AdminOrganisation );

        // 
        // get the debug ResultData.
        // 
        this.LogValue (  this._Bll_Organisations.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._Bll_Organisations.Log + " returned error message: " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          switch ( result )
          {
            case EvEventCodes.Data_Duplicate_Id_Error:
              {
                this.ErrorMessage =
                  String.Format (
                    EdLabels.Organisation_Duplicate_Error_Message,
                    this.Session.Organisation.OrgId );
                break;
              }
            case EvEventCodes.Identifier_Project_Id_Error:
              {
                this.ErrorMessage = EdLabels.Project_Identifier_Empty_Error_Message;
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

        return new Model.UniForm.AppData();

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
    /// THis method saves the ResultData object updating the field values contained in the 
    /// parameter list.
    /// </summary>
    /// <param name="PageCommand">Evado.Model.UniForm.Command object.</param>
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
      if ( this.Session.AdminOrganisation.OrgType  == EvOrganisation.OrganisationTypes.Null)
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
      List<Evado.Model.UniForm.Parameter> Parameters )
    {
      this.LogMethod (  "updateObjectValue" );
      this.LogDebug ( "Parameters.Count: " + Parameters.Count );
      this.LogDebug ( "Customer.Guid: " + this.Session.AdminOrganisation.Guid );

      /// 
      /// Iterate through the parameter values updating the ResultData object
      /// 
      foreach ( Evado.Model.UniForm.Parameter parameter in Parameters )
      {
        if ( parameter.Name.Contains ( Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == false
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          && parameter.Name != Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION
          && parameter.Name != EuOrganisations.CONST_CURRENT_FIELD_ID )
        {
          this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );
          try
          {
            EvOrganisation.OrganisationFieldNames fieldName =
               Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvOrganisation.OrganisationFieldNames> (
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

    }//END updateObjectValue method.

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  }
}//END namespace