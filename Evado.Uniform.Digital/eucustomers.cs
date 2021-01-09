/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\Customers.cs" company="EVADO HOLDING PTY. LTD.">
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
using Evado.Model.Digital;
// using Evado.Web;

namespace Evado.UniForm.Clinical
{
  /// <summary>
  /// This class defines the application base classs that is used to terminate the 
  /// hosted application objects.
  /// 
  /// This class terminates the Customer object.
  /// </summary>
  public class EuCustomers : EuClassAdapterBase
  {
    #region Class Initialisation
    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EuCustomers ( )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuCustomers.";
    }

    /// <summary>
    /// This method initialises the class and passs in the user profile.
    /// </summary>
    public EuCustomers (
      EuApplicationObjects ApplicationObjects,
      EvUserProfileBase ServiceUserProfile,
      EuSession SessionObjects,
      String UniFormBinaryFilePath,
      EvClassParameters Settings )
    {
      this.ClassNameSpace = "Evado.UniForm.Clinical.EuCustomers.";
      this.ApplicationObjects = ApplicationObjects;
      this.ServiceUserProfile = ServiceUserProfile;
      this.Session = SessionObjects;
      this.UniForm_BinaryFilePath = UniFormBinaryFilePath;
      this.ClassParameters = Settings;


      this.LogInitMethod ( "EuCustomers initialisation" );
      this.LogInit ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.Userid: " + this.Session.UserProfile.UserId );
      this.LogInit ( "SessionObjects.UserProfile.CommonName: " + this.Session.UserProfile.CommonName );
      this.LogInit ( "UniFormBinaryFilePath: " + this.UniForm_BinaryFilePath );

      this._Bll_Customers = new Evado.Bll.Clinical.EvCustomers ( this.ClassParameters );

    }//END Method


    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class constants and variables.

    private const String CONST_CURRENT_FIELD_ID = "CURRENT";
    private const String CONST_NEW_FIELD_ID = "NEW";

    private Evado.Bll.Clinical.EvCustomers _Bll_Customers = new Evado.Bll.Clinical.EvCustomers ( );

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
      this.LogMethod ( "getClientDataObject" );

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
        this.LogValue ( Evado.Model.UniForm.EuStatics.CONST_METHOD_START
          + " Evado.UniForm.Clinical.Customers.getListObject" );
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

          this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage; ;
        }

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        clientDataObject.Title = EvCustomerLabels.Customer_List_Page_Title;
        clientDataObject.Page.Title = clientDataObject.Title;
        clientDataObject.Id = Guid.NewGuid ( );

        if ( this.ApplicationObjects.HelpUrl != String.Empty )
        {/**/
          Evado.Model.UniForm.Command helpCommand = clientDataObject.Page.addCommand (
           EvLabels.Label_Help_Command_Title,
           EuAdapter.APPLICATION_ID,
           EuAdapterClasses.Customers.ToString ( ),
           Model.UniForm.ApplicationMethods.Get_Object );

          helpCommand.Type = Evado.Model.UniForm.CommandTypes.Html_Link;

          helpCommand.AddParameter ( Model.UniForm.CommandParameters.Link_Url,
           EvcStatics.createHelpUrl (
            this.ApplicationObjects.HelpUrl,
             Evado.Model.Digital.EvPageIds.Customer_View ) );

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
        this.ErrorMessage = EvCustomerLabels.Customer_List_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage; ;

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
        this.LogValue ( Evado.Model.UniForm.EuStatics.CONST_METHOD_START
          + " Evado.UniForm.Clinical.Customers.getListGroup" );

        // 
        // Create the new pageMenuGroup.
        // 
        Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
          EvCustomerLabels.Customer_List_Group_Title,
          Evado.Model.UniForm.EditAccess.Inherited );
        pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
        pageGroup.CmdLayout = Evado.Model.UniForm.GroupCommandListLayouts.Vertical_Orientation;
        pageGroup.Title = EvCustomerLabels.Customer_List_Group_Title;


        // 
        // Add the save groupCommand
        // 
        Evado.Model.UniForm.Command groupCommand = pageGroup.addCommand (
          EvCustomerLabels.Customer_New_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Customers.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Create_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        groupCommand.AddParameter ( EuCustomers.CONST_NEW_FIELD_ID, "true" );

        groupCommand.SetBackgroundColour (
          Model.UniForm.CommandParameters.BG_Default,
          Model.UniForm.Background_Colours.Purple );

        // 
        // get the list of customers.
        // 
        if ( this.ApplicationObjects.CustomerList.Count == 0 )
        {
          this.ApplicationObjects.CustomerList = this._Bll_Customers.getView ( EvCustomer.CustomerStates.Null  );
          this.LogValue ( this._Bll_Customers.Log );
        }
        this.LogValue ( "list count: " + this.ApplicationObjects.CustomerList.Count );
        // 
        // generate the page links.
        // 
        foreach ( EvCustomer customer in this.ApplicationObjects.CustomerList )
        {
          // 
          // Add the trial organisation to the list of organisations as a groupCommand.
          // 
          Evado.Model.UniForm.Command command = pageGroup.addCommand (
            customer.LinkText,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Customers.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Get_Object );

          command.Id = customer.Guid;
          command.SetGuid ( customer.Guid );

        }//END trial organisation list iteration loop

        this.LogValue ( "command count: " + pageGroup.CommandList.Count );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvCustomerLabels.Customer_List_Error_Message;

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
      this.LogValue ( Evado.Model.UniForm.EuStatics.CONST_METHOD_START
        + this.ClassNameSpace + "getObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.AppData clientDataObject = new Evado.Model.UniForm.AppData ( );
      Guid OrgGuid = Guid.Empty;

      //
      // Determine if the user has access to this page and log and error if they do not.
      //
      if ( this.Session.UserProfile.hasManagementAccess == false
        && this.Session.UserProfile.hasRecordAccess == false )
      {
        this.LogIllegalAccess (
          this.ClassNameSpace + "getListObject",
          this.Session.UserProfile );

        this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

        return this.Session.LastPage; ;
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

        if ( this.Session.Customer.Guid != Guid.Empty )
        {
          // 
          // return the client ResultData object for the customer.
          // 
          this.getDataObject ( clientDataObject );
        }
        else
        {
          this.LogValue ( "ERROR: current organisation guid empty" );
          this.ErrorMessage = EvCustomerLabels.Customer_Guid_Empty_Message;
        }

        return clientDataObject;
      }
      this.LogValue ( "Query site Guid: " + OrgGuid );

      try
      {
        // 
        // Retrieve the customer object from the database via the DAL and BLL layers.
        // 
        this.Session.Customer = this._Bll_Customers.getItem ( OrgGuid );

        this.LogValue ( this._Bll_Customers.Log );

        this.LogValue ( "Customer.CustomerId: " + this.Session.Customer.CustomerId );

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
        this.ErrorMessage = EvCustomerLabels.Customer_Page_Error_Message;

        // 
        // Generate the log the error event.
        // 
        this.LogException ( Ex );
      }

      return this.Session.LastPage; ;

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

      ClientDataObject.Id = this.Session.Customer.Guid;
      ClientDataObject.Title = EvCustomerLabels.Customer_Page_Title;

      ClientDataObject.Page.Id = ClientDataObject.Id;
      ClientDataObject.Page.Title = ClientDataObject.Title;
      ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // Set the user edit access to the objects.
      //
      if ( this.Session.UserProfile.hasAdministrationAccess == true
        || this.Session.UserProfile.hasManagementEditAccess == true )
      {
        ClientDataObject.Page.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      }
      this.LogValue ( "Page.Status: " + ClientDataObject.Page.EditAccess );

      //
      // add the page Command objects.
      //
      this.GetPageCommandObjects ( ClientDataObject.Page );

      //
      // get the Customer detail group.
      //
      this.getCustomerDetails_Group ( ClientDataObject.Page );

      //
      // get the Customer settings group.
      //
      this.getCustomerSettings_Group ( ClientDataObject.Page );

      //
      // Get the Customer setting group.
      //

      this.LogMethodEnd ( "getDataObject" );

    }//END getDataObject Method

    // ==============================================================================
    /// <summary>
    /// This method generated a customer page command objects.
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void GetPageCommandObjects ( Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "setPageCommandObjects" );
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      //
      // Add the help button if the help url is defined.
      //
      /*
      if ( this.ApplicationObjects.HelpUrl != String.Empty )
      {
        pageCommand = PageObject.addCommand (
         EvLabels.Label_Help_Command_Title,
         EuAdapter.APPLICATION_ID,
         EuAdapterClasses.Customers.ToString ( ),
         Model.UniForm.ApplicationMethods.Get_Object );

        pageCommand.Type = Evado.Model.UniForm.CommandTypes.Html_Link;

        pageCommand.AddParameter ( Model.UniForm.CommandParameters.Link_Url,
           EvcStatics.createHelpUrl (
            this.ApplicationObjects.HelpUrl,
             Evado.Model.Digital.EvPageIds.Customer_Page ) );
      }
      */

      // 
      // Add the save groupCommand
      // 
      if ( PageObject.EditAccess == Evado.Model.UniForm.EditAccess.Enabled )
      {
        //
        // save command.
        //
        pageCommand = PageObject.addCommand (
          EvCustomerLabels.Customer_Save_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Customers.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        pageCommand.SetGuid ( PageObject.Id );

        //
        // Delete command
        //
        if ( this.Session.UserProfile.hasEvadoAdministrationAccess == true )
        {
          pageCommand = PageObject.addCommand (
            EvCustomerLabels.Customer_Delete_Command_Title,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Customers.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Save_Object );

          // 
          // Define the save and delete groupCommand parameters
          // 
          pageCommand.SetGuid ( PageObject.Id );
          pageCommand.AddParameter (
             Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
            EvCustomer.ActionCodes.Delete_Object.ToString ( ) );
        }
      }

      this.LogMethodEnd ( "setPageCommandObjects" );
    }//ENd setPageCommandObjects method

    // ==============================================================================
    /// <summary>
    /// This method generated a customer group command objects.
    /// </summary>
    /// <param name="Group">Evado.Model.UniForm.Group object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void GetGroupCommandObjects ( Evado.Model.UniForm.Group Group )
    {
      this.LogMethod ( "setPageCommandObjects" );
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Command pageCommand = new Evado.Model.UniForm.Command ( );

      // 
      // Add the save groupCommand
      // 
      if ( Group.EditAccess == Evado.Model.UniForm.EditAccess.Enabled )
      {
        //
        // save command.
        //
        pageCommand = Group.addCommand (
          EvCustomerLabels.Customer_Save_Command_Title,
          EuAdapter.APPLICATION_ID,
          EuAdapterClasses.Customers.ToString ( ),
          Evado.Model.UniForm.ApplicationMethods.Save_Object );

        // 
        // Define the save and delete groupCommand parameters
        // 
        pageCommand.SetGuid ( Group.Id );

        //
        // Delete command
        //
        if ( this.Session.UserProfile.hasEvadoAdministrationAccess == true )
        {
          pageCommand = Group.addCommand (
            EvCustomerLabels.Customer_Delete_Command_Title,
            EuAdapter.APPLICATION_ID,
            EuAdapterClasses.Customers.ToString ( ),
            Evado.Model.UniForm.ApplicationMethods.Save_Object );

          // 
          // Define the save and delete groupCommand parameters
          // 
          pageCommand.SetGuid ( Group.Id );
          pageCommand.AddParameter (
             Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION,
            EvCustomer.ActionCodes.Delete_Object.ToString ( ) );
        }
      }

      this.LogMethodEnd ( "setPageCommandObjects" );
    }//ENd setPageCommandObjects method

    // ==============================================================================
    /// <summary>
    /// This method generated a Customer Details Group
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    /// <returns>ClientApplicationData object</returns>
    //  ------------------------------------------------------------------------------
    private void getCustomerDetails_Group ( 
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getCustomerDetailsObject" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );
      List<EvOption> optionList = new List<EvOption> ( );

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvCustomerLabels.Customer_Details_Group_Label,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // set the group command objects.
      //
      this.GetGroupCommandObjects ( pageGroup );

      // 
      // Create the customer id object
      // 
      pageField = pageGroup.createReadOnlyTextField (
        EvCustomerLabels.Customer_No_Field_Label,
        this.Session.Customer.CustomerNo.ToString() );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the customer Service Type object
      // 
      pageField = pageGroup.createReadOnlyTextField (
        EvCustomerLabels.Customer_Service_Field_Label,
        this.Session.Customer.ServiceType.ToString ( ) );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the customer Service Type object
      // 
      if ( this.Session.Customer.NoOfStudies == 1 )
      {
        pageField = pageGroup.createBooleanField (
          String.Empty,
          EvCustomerLabels.Customer_IsSingleStudy_Field_Label,
          this.Session.Customer.IsSingleStudy );
        pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;
        pageField.EditAccess = Model.UniForm.EditAccess.Disabled;
      }
      else
      {
        pageField = pageGroup.createReadOnlyTextField (
          EvCustomerLabels.Customer_No_of_Studies_Field_Label,
          this.Session.Customer.NoOfStudies.ToString ( ) );
        pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      }

      // 
      // Create the customer name object
      // 
      pageField = pageGroup.createTextField (
        EvCustomer.CustomerFieldNames.Name.ToString ( ),
        EvCustomerLabels.Customer_Name_Field_Label,
        this.Session.Customer.Name,
        50 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      pageField.Mandatory = true;

      pageField.setBackgroundColor (
        Model.UniForm.FieldParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      // 
      // Create the customer name object
      // 
      pageField = pageGroup.createAddressField (
        EvCustomer.CustomerFieldNames.Address,
        EvCustomerLabels.Customer_Address_Field_Label,
        this.Session.Customer.Address );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the customer telephone number object
      // 
      pageField = pageGroup.createTelephoneNumberField (
        EvCustomer.CustomerFieldNames.Telephone.ToString ( ),
        EvCustomerLabels.Customer_Telephone_Field_Label,
        this.Session.Customer.Telephone );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;


      // 
      // Create the customer email addressr object
      // 
      pageField = pageGroup.createEmailAddressField (
        EvCustomer.CustomerFieldNames.Email_Address.ToString ( ),
        EvCustomerLabels.Customer_Email_Field_Label,
        this.Session.Customer.EmailAddress );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;


      // 
      // Create the customer telephone number object
      // 
      pageField = pageGroup.createTextField (
        EvCustomer.CustomerFieldNames.Administrator.ToString ( ),
        EvCustomerLabels.Customer_Administator_Field_Label,
        this.Session.Customer.Administrator, 50 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;


      // 
      // Create the customer fax number object
      // 
      pageField = pageGroup.createEmailAddressField (
        EvCustomer.CustomerFieldNames.AdminEmailAddress.ToString ( ),
        EvCustomerLabels.Customer_AdministratorEmail_Field_Label,
        this.Session.Customer.AdminEmailAddress );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;


      this.LogMethodEnd ( "getCustomerDetailsObject" );
    }//END getCustomerDetails_Group Method

    // ==============================================================================
    /// <summary>
    /// This method returns a client application ResultData object
    /// </summary>
    /// <param name="PageObject">Evado.Model.UniForm.Page object.</param>
    //  ------------------------------------------------------------------------------
    private void getCustomerSettings_Group ( 
      Evado.Model.UniForm.Page PageObject )
    {
      this.LogMethod ( "getCustomerSettings_Group" );
      // 
      // Initialise the methods variables and objects.
      // 
      Evado.Model.UniForm.Field pageField = new Evado.Model.UniForm.Field ( );

      //
      // If the user does not have Evado administration access exit.
      //
      if ( this.Session.UserProfile.hasEvadoAdministrationAccess == false )
      {
        this.LogDebug ( "User not an Evado Administrator." );
        this.LogMethodEnd ( "getCustomerSettings_Group" );
        return;
      }

      // 
      // create the page pageMenuGroup
      // 
      Evado.Model.UniForm.Group pageGroup = PageObject.AddGroup (
        EvCustomerLabels.Customer_Settings_Group_Label,
        String.Empty,
        Evado.Model.UniForm.EditAccess.Inherited );
      pageGroup.Layout = Evado.Model.UniForm.GroupLayouts.Full_Width;
      pageGroup.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;

      //
      // set the group command objects.
      //
      this.GetGroupCommandObjects ( pageGroup );

      // 
      // Create the customer id object
      // 
      pageField = pageGroup.createReadOnlyTextField (
        EvCustomerLabels.Customer_No_Field_Label,
        this.Session.Customer.CustomerNo.ToString() );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the Customer home page header text object
      // 
      pageField = pageGroup.createTextField (
        EvCustomer.CustomerFieldNames.Ads_Group.ToString ( ),
        EvCustomerLabels.Customer_Ads_Group_Field_Label,
        this.Session.Customer.AdsGroupName, 50 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      // 
      // Create the customer telephone number object
      // 
      pageField = pageGroup.createTextField (
        EvCustomer.CustomerFieldNames.Home_Page_Header.ToString ( ),
        EvCustomerLabels.Customer_Home_Page_Header_Field_Label,
        this.Session.Customer.HomePageHeader, 100 );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Generate the customer type list.
      //
      List<EvOption> typeList = EvCustomer.GetServiceList ( false );

      //
      // Generate the organisation type radio button list field object.
      //
      pageField = pageGroup.createRadioButtonListField (
        EvCustomer.CustomerFieldNames.Service_Type.ToString ( ),
        EvCustomerLabels.Customer_Service_Field_Label,
        Evado.Model.EvStatics.getEnumStringValue ( this.Session.Customer.ServiceType ),
        typeList );

      pageField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      pageField.Mandatory = true;
      pageField.setBackgroundColor (
        Model.UniForm.FieldParameterList.BG_Mandatory,
        Model.UniForm.Background_Colours.Red );

      //
      // Generate the customer type list.
      //
      typeList = EvStatics.Enumerations.getOptionsFromEnum( typeof(EvCustomer.CustomerStates ), true );

      //
      // Generate the organisation type radio button list field object.
      //
      pageField = pageGroup.createRadioButtonListField (
        EvCustomer.CustomerFieldNames.State.ToString ( ),
        EvCustomerLabels.Customer_State_Field_Label,
        Evado.Model.EvStatics.getEnumStringValue ( this.Session.Customer.State ),
        typeList );

      pageField.EditAccess = Evado.Model.UniForm.EditAccess.Enabled;
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;
      // 
      // Create the customer Service Type object
      //
      pageField = pageGroup.createBooleanField (
        String.Empty,
        EvCustomerLabels.Customer_IsSingleStudy_Field_Label,
        this.Session.Customer.IsSingleStudy );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      //
      // Display the number of studies running.
      //
      pageField = pageGroup.createReadOnlyTextField (
        EvCustomerLabels.Customer_No_of_Studies_Field_Label,
        this.Session.Customer.NoOfStudies.ToString ( ) );
      pageField.Layout = EuRecordGenerator.ApplicationFieldLayout;

      this.LogMethodEnd ( "getCustomerSettings_Group" );

    }//END getCustomerSettings_Group Method

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
      try
      {
        this.LogMethod ( "createObject" );

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
        if ( this.Session.UserProfile.hasEvadoAdministrationAccess == false )
        {
          this.LogIllegalAccess (
            this.ClassNameSpace + "getListObject",
            this.Session.UserProfile );

          this.ErrorMessage = EvLabels.Illegal_Page_Access_Attempt;

          return this.Session.LastPage; ;
        }

        //
        // Initialise the dlinical ResultData objects.
        //
        this.Session.Customer = new EvCustomer ( );
        this.Session.Customer.Guid = Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID;

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
        this.ErrorMessage = EvCustomerLabels.Customer_Creation_Error_Message;

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
    private Evado.Model.UniForm.AppData updateObject (
      Evado.Model.UniForm.Command PageCommand )
    {
      this.LogMethod ( "updateObject" );
      this.LogDebug ( "PageCommand: " + PageCommand.getAsString ( false, true ) );
      try
      {

        this.LogDebug ( "Customer"
          + " Guid: " + this.Session.Customer.Guid
          + " CustomerId: " + this.Session.Customer.CustomerId
          + " Title: " + this.Session.Customer.Name );
        EvCustomer.ActionCodes saveAction = EvCustomer.ActionCodes.Save;

        // 
        // Log access to page.
        // 
        this.LogPageAccess (
          this.ClassNameSpace + "updateObject",
          this.Session.UserProfile );

        // 
        // Initialise the update variables.
        // 
        this.Session.Customer.UpdatedByUserId = this.Session.UserProfile.UserId;
        this.Session.Customer.UserCommonName = this.Session.UserProfile.CommonName;
        this.ApplicationObjects.CustomerList = new List<EvCustomer> ( );

        // 
        // IF the guid is new object id  alue then set the save object for adding to the database.
        // 
        if ( this.Session.Customer.Guid == Evado.Model.Digital.EvcStatics.CONST_NEW_OBJECT_ID )
        {
          this.Session.Customer.Guid = Guid.Empty;
        }

        // 
        // Delete the object.
        // 
        if ( PageCommand.Method == Evado.Model.UniForm.ApplicationMethods.Delete_Object )
        {
          return new Model.UniForm.AppData ( );
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

        this.LogDebug ( "AddressStreet_1: " + this.Session.Customer.AddressStreet_1 );
        this.LogDebug ( "AddressStreet_2: " + this.Session.Customer.AddressStreet_2 );
        this.LogDebug ( "AddressCity: " + this.Session.Customer.AddressCity );
        this.LogDebug ( "AddressState: " + this.Session.Customer.AddressState );
        this.LogDebug ( "AddressCountry: " + this.Session.Customer.AddressCountry );
        this.LogDebug ( "AddressPostCode: " + this.Session.Customer.AddressPostCode );

        // 
        // Get the save action message value.
        // 
        String stSaveAction = PageCommand.GetParameter ( Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION );

        // 
        // Resets the save action to the parameter passed in the page groupCommand.
        // 
        if ( stSaveAction != String.Empty )
        {
          saveAction = Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvCustomer.ActionCodes> ( stSaveAction );
        }
        this.Session.Customer.Action = saveAction;


        // 
        // update the object.
        // 
        EvEventCodes result = this._Bll_Customers.saveItem ( this.Session.Customer );

        // 
        // get the debug ResultData.
        // 
        this.LogValue ( this._Bll_Customers.Log );

        // 
        // if an error state is returned create log the event.
        // 
        if ( result != EvEventCodes.Ok )
        {
          string StEvent = this._Bll_Customers.Log 
            + "\r\nreturned error message: " + Evado.Model.Digital.EvcStatics.getEventMessage ( result );
          this.LogError ( EvEventCodes.Database_Record_Update_Error, StEvent );

          this.ErrorMessage = EvCustomerLabels.Customer_Update_Error_Message;

          return this.Session.LastPage;
        }//END save error returned.

        return new Model.UniForm.AppData ( );

      }
      catch ( Exception Ex )
      {
        // 
        // Create the error message to be displayed to the user.
        // 
        this.ErrorMessage = EvCustomerLabels.Customer_Update_Error_Message;

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
      // Org name not defined.
      //
      if ( this.Session.Customer.Name == String.Empty
       && this.Session.Customer.Guid == Guid.Empty )
      {
        if ( this.ErrorMessage != String.Empty )
        {
          this.ErrorMessage += "\r\n ";
        }
        this.ErrorMessage += EvCustomerLabels.Customer_Name_Error_Message;

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
      this.LogMethod ( "updateObjectValue" );
      this.LogDebug ( "Parameters.Count: " + Parameters.Count );
      this.LogDebug ( "Customer.Guid: " + this.Session.Customer.Guid );

      /// 
      /// Iterate through the parameter values updating the ResultData object
      /// 
      foreach ( Evado.Model.UniForm.Parameter parameter in Parameters )
      {
        if ( parameter.Name.Contains ( Evado.Model.Digital.EvcStatics.CONST_GUID_IDENTIFIER ) == false
          && parameter.Name != Evado.Model.UniForm.CommandParameters.Custom_Method.ToString ( )
          && parameter.Name != Evado.Model.Digital.EvcStatics.CONST_SAVE_ACTION
          && parameter.Name != EuCustomers.CONST_CURRENT_FIELD_ID )
        {
          this.LogDebug ( parameter.Name + " > " + parameter.Value + " >> UPDATED" );
          try
          {
            EvCustomer.CustomerFieldNames fieldName =
               Evado.Model.Digital.EvcStatics.Enumerations.parseEnumValue<EvCustomer.CustomerFieldNames> (
              parameter.Name );

            this.Session.Customer.setValue ( fieldName, parameter.Value );

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