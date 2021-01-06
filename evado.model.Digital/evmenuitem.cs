/***************************************************************************************
 * <copyright file="EvMenuItem.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvMenuItem data object.
 *
 ****************************************************************************************/

using System;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// Business entity used to model accounts
  /// </summary>
  [Serializable]
  public class EvMenuItem : EvHasSetValue<EvMenuItem.MenuFieldNames>
  {
    #region  Class enumeration lists.

    /// <summary>
    /// This enumeration list defines the enumerated field names for the class.
    /// </summary>
    public enum MenuFieldNames
    {
      /// <summary>
      /// This enumeration defines the Null Value or no selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the page identifier of menu field names.
      /// </summary>
      Page_Id,

      /// <summary>
      /// This enumeration defines the menu identifier of menu field names.
      /// </summary>
      Order,

      /// <summary>
      /// This enumeration defines the menu title of menu field names.
      /// </summary>
      Title,

      /// <summary>
      /// This enumeration defines the menu group identifier of menu field names.
      /// </summary>
      Group,

      /// <summary>
      /// This enumeration defines the menu  group header of menu field names.
      /// </summary>
      Group_Header,

      /// <summary>
      /// This enumeration defines the menu site of menu field names.
      /// </summary>
      Platform,

      /// <summary>
      /// This enumeration defines the menu's delimited modules of menu field names.
      /// </summary>
      Modules,

      /// <summary>
      /// This enumeration defines the menu's delimited role list of menu field names.
      /// </summary>
      Role_List,
    }

    #endregion

    #region constants
    /// <summary>
    /// This constant defines the admin menu group identifier.
    /// </summary>
    public const string CONST_MENU_ADMIN_GROUP_ID = "ADMIN";
    /// <summary>
    /// this constant defines the analysis menu group identifier.
    /// </summary>
    public const string CONST_MENU_ANALYSIS_GROUP_ID = "ANA";
    /// <summary>
    /// this constant defines the management menu group identifier.
    /// </summary>
    public const string CONST_MENU_PROJECT_MANAGEMENT_GROUP_ID = "TM";
    /// <summary>
    /// this constant defines the global project menu group identifier.
    /// </summary>
    public const string CONST_MENU_PROJECT_RECORD_GROUP_ID = "TR";
    /// <summary>
    /// this constant defines the project record menu group identifier.
    /// </summary>
    public const string CONST_MENU_AUDIT_GROUP_ID = "TAU";
    /// <summary>
    /// this constant defines the data management menu group identifier.
    /// </summary>
    public const string CONST_MENU_DATA_MANAGEMENT_GROUP_ID = "DMG";

    /// <summary>
    /// This constant defines the trail dashboard group key value.
    /// </summary>
    public const string CONST_PROJECT_DASHBOARD_GROUP = "TRDSH";

    /// <summary>
    /// The constant defines site dashboard group key value
    /// </summary>
    public const string CONST_SITE_DASHBOARD_GROUP = "STDSH";

    /// <summary>
    /// The constant defines trail menu group key value
    /// </summary>
    public const string CONST_PROJECT_MENU_GROUP = "TMNU";

    /// <summary>
    /// The constant defines subject menu group key value
    /// </summary>
    public const string CONST_SUBJECT_MENU_GROUP = "SUMNU";

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Properties 

    private Guid _Guid = Guid.Empty;
    /// <summary>
    /// This property contains the unique global identifier of menu item.
    /// </summary>
    public Guid Guid
    {
      get { return _Guid; }
      set { _Guid = value; }
    }

    private EvPageIds _PageId = EvPageIds.Null;
    /// <summary>
    /// This property contains the application page identifier as an enumerated value used to define the page.
    /// </summary>
    public EvPageIds PageId
    {
      get { return this._PageId; }
      set { this._PageId = value; }
    }

    private int _Order = 0;
    /// <summary>
    /// This property contains the order value of menu item.
    /// </summary>
    public int Order
    {
      get { return _Order; }
      set { _Order = value; }
    }

    private string _Title = String.Empty;
    /// <summary>
    /// This property contains the menu item's title.
    /// </summary>
    public string Title
    {
      get { return _Title; }
      set { _Title = value; }
    }

    private string _Group = String.Empty;
    /// <summary>
    /// This property contains the menu item's group identifier.
    /// </summary>
    public string Group
    {
      get { return _Group; }
      set { _Group = value.ToUpper(); }
    }

    private bool _groupHeader = false;
    /// <summary>
    /// This property indicates if the menu item is a group header. 
    /// </summary>
    public bool GroupHeader
    {
      get { return _groupHeader; }
      set { _groupHeader = value; }
    }

    private string _Platform = String.Empty;
    /// <summary>
    /// This property contains the application site this menu item belongs to.
    /// </summary>
    public string Platform
    {
      get { return _Platform; }
      set { _Platform = value; }
    }

    private string _modules = String.Empty;
    /// <summary>
    /// This property contains the delimited list of modules this menu item belongs to.
    /// </summary>
    public string Modules
    {
      get { return _modules; }
      set
      {
        _modules = value;
        if ( _modules == String.Empty )
        {
          this._modules = EvModuleCodes.All_Modules.ToString ( );
        }
        _modules = _modules.Replace ( ",", EvcStatics.CONST_STRING_SEPARATOR );
      }
    }

    /// <summary>
    /// This property returns the modules as an array of string objects.
    /// </summary>
    public string [ ] ModuleList
    {
      get
      {
        return this._modules.Split ( EvcStatics.CONST_CHAR_SEPARATOR );
      }
      set
      {
        this._modules = String.Empty;
        foreach(  string str in value )
        {
          if ( this._modules != String.Empty )
          {
            this._modules +=  EvcStatics.CONST_CHAR_SEPARATOR;
          }
          this._modules += str;
        }
      }
    }

    private string _RoleList = String.Empty;
    /// <summary>
    /// This property contains an encoded list of the roles for this menu item.
    /// </summary>
    public string RoleList
    {
      get { return this._RoleList; }
      set { this._RoleList = value; }
    }

    private string _UserId = String.Empty;
    /// <summary>
    /// This property contains user identifier of this menu item
    /// </summary>
    public string UserId
    {
      get { return _UserId; }
      set { _UserId = value; }
    }
   
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region public methods

    //  =================================================================================
    /// <summary>
    ///   This method validates the items selection
    /// </summary>
    /// <param name="LoadedModuleList">List of EvModuleCodes: list of enabled modules</param>
    /// <param name="Project">EvProject object</param>
    /// <param name="UserRole">EvMenuItem.RoleCodes: enumeration user role</param>
    /// <returns>bool: is the role in the list</returns>
    //  ---------------------------------------------------------------------------------
    public bool SelectMenuHeader (
      List<EvModuleCodes> LoadedModuleList,
      EdApplication Project,
      EvRoleList UserRole )
    {
      if ( this._groupHeader == false )
      {
        return false;
      }

      //
      // only select items that have the correct role. 
      //
      if ( this.hasRole ( UserRole ) == false )
      {
        return false;
      }
      //
      // Skip all global menuitems if 
      //
      if ( this.hasModule ( EvModuleCodes.All_Modules ) == true )
      {
        return true;
      }

      return true;

    }//END addRole method.

    //  =================================================================================
    /// <summary>
    ///   This method validates the items selection
    /// </summary>
    /// <param name="LoadedModuleList">List of EvModuleCodes: list of enabled modules</param>
    /// <param name="Project">EvProject object</param>
    /// <param name="Organisation">EvOrganisation object</param>
    /// <param name="UserRole">EvMenuItem.RoleCodes: enumeration user role</param>
    /// <returns>bool: is the role in the list</returns>
    //  ---------------------------------------------------------------------------------
    public bool SelectMenuItem (
      List<EvModuleCodes> LoadedModuleList,
      EvRoleList UserRole )
    {
      //
      // only select items that have the correct role. 
      //
      if ( this.hasRole ( UserRole ) == false )
      {
        //return false;
      }

      if ( this.hasModule( EvModuleCodes.All_Modules ) == true )
      {
        return true;
      }

      return true;

    }//END addRole method.
    
    //  =================================================================================
    /// <summary>
    ///   This method adds new role to the enumeration list of menu roles
    /// </summary>
    /// <param name="LoadedModuleList">List of EvModuleCodes enumerations</param>
    /// <param name="Module">EvModuleCodes enumeration </param>
    /// <returns>bool: is the role in the list</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Convert a role object into string format
    /// 
    /// 2. Retrun true, if new role is added
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    private bool hasLoadedModule ( 
      List<EvModuleCodes> LoadedModuleList, 
      EvModuleCodes Module )
    {
      foreach( EvModuleCodes module in LoadedModuleList )
      {
        if ( module == Module )
        {
          return true;
        }
      }
      return false;
    }//ENd hasLoadedModule module.

    //  =================================================================================
    /// <summary>
    ///   This method adds new role to the enumeration list of menu roles
    /// </summary>
    /// <param name="Role">EvMenuItem.RoleCodes: enumeration list of menu roles</param>
    /// <returns>bool: is the role in the list</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Convert a role object into string format
    /// 
    /// 2. Retrun true, if new role is added
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public bool addRole ( EvRoleList Role )
    {
      //
      // Get the string value of the selected module
      // 
      string stRole = Role.ToString ( );

      // 
      // Test for administration module
      // 
      if ( this._RoleList.Contains ( stRole ) == false )
      {
        this._RoleList += ";" + stRole;

        return true;
      }

      return false;

    }//END addRole method.

    //  =================================================================================
    /// <summary>
    ///   This method indicates if the role value is in the enumeration list of menu roles
    /// </summary>
    /// <param name="Role">EvMenuItem.RoleCodes: the enumeration list of menu roles</param>
    /// <returns>bool: is the role is in the enumeration list of menu roles</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Convert Role object item into a string
    /// 
    /// 2. Return true, if the Role exists
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public bool hasRole ( EvRoleList Role )
    {
      //
      // Get the string value of the selected module
      // 
      string stRole = Role.ToString ( );

      string [] arRoleList = this._RoleList.Split ( ';' );

      foreach ( String role in arRoleList )
      {
        // 
        // Test for administration module
        // 
        if ( role.Trim() == stRole.Trim() )
        {
          return true;
        }
      }

      return false;

    }//END hasRole method.

    //  =================================================================================
    /// <summary>
    ///   This method indicates if the role value is in the enumeration list of menu roles
    /// </summary>
    /// <param name="Module">EvModuleCodes </param>
    /// <returns>bool: is the role is in the enumeration list of menu roles</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Convert Role object item into a string
    /// 
    /// 2. Return true, if the Role exists
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public bool hasModule ( EvModuleCodes  Module )
    {
      //
      // Get the string value of the selected module
      // 
      string stModule = Module.ToString ( );
      string [ ] arModuleList = this._modules.Split ( ';' );

      //
      // Iterate thought the modules for a match.
      //
      foreach ( String module in arModuleList )
      {
        if ( module.Trim ( ) == stModule.Trim ( ) )
        {
          return true;
        }
      }
      // 
      // Test for administration module
      // 
      if ( this._modules.Contains ( stModule ) == true )
      {
        return true;
      }

      return false;

    }//END hasRole method.

    //  ================================================================================
    /// <summary>
    /// Sets the value of this activity class field name. Validate the format of the
    /// value. 
    /// </summary>
    /// <param name="fieldName">MenuClassFieldNames: Name of the field to be setted.</param>
    /// <param name="value">String: value to be setted</param>
    /// <returns>EvEventCodes: indicating the successful update of the property value.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Switch the fieldName and update value for the property defined by the activity field names
    /// 
    /// 2. Return casting error, if field name type is empty
    /// </remarks>
    //  --------------------------------------------------------------------------------
    public EvEventCodes setValue ( MenuFieldNames fieldName, String value )
    {
      EvPageIds pageId = EvPageIds.Null;
      //
      // Switch the FieldName based on the activity field names
      //
      switch ( fieldName )
      {
        case MenuFieldNames.Page_Id:
          {
            if ( EvcStatics.Enumerations.tryParseEnumValue<EvPageIds> ( value, out pageId ) == false )
            {
              return EvEventCodes.Data_Enumeration_Casting_Error;
            }

            this._PageId = pageId;
            break;
          }
        case MenuFieldNames.Order:
          {
            try
            {
              this.Order = int.Parse ( value );
            }
            catch { }
              break;
          }
        case MenuFieldNames.Title:
          {
            this.Title = value;
            break;
          }
        case MenuFieldNames.Group:
          {
            this.Group = value;
            break;
          }
        case MenuFieldNames.Group_Header:
          {
            this.GroupHeader = EvcStatics.getBool( value );
            break;
          }
        case MenuFieldNames.Platform:
          {
            this.Platform = value;
            break;
          }
        case MenuFieldNames.Modules:
          {
            this.Modules = value;

            if ( this.Modules.Contains ( EvModuleCodes.All_Modules.ToString ( ) ) == true )
            {
              this.Modules = EvModuleCodes.All_Modules.ToString ( ) ;
            }

            break;
          }
        case MenuFieldNames.Role_List:
          {
            this.RoleList = value;
            break;
          }
      }// End switch field name

      return EvEventCodes.Ok;

    }//End setValue method.


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region public static methods

    //  =================================================================================
    /// <summary>
    ///   This method adds new role to the enumeration list of menu roles
    /// </summary>
    /// <param name="SelectionList">boolean to get the module list.</param>
    /// <returns>List of EvOption </returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Convert a role object into string format
    /// 
    /// 2. Retrun true, if new role is added
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption> getModuleList (  bool SelectionList )
    {
      //
      // Get the string value of the selected module
      // 
      List<EvOption> loadedModulesList = new List<EvOption> ( );

      if ( SelectionList == true )
      {
        loadedModulesList.Add ( new EvOption ( ) );
      }

      //
      // Create the Loaded modules list
      //
      //Administration, Clinical, Management, Mobile, Registry, Surveillance
      //
      loadedModulesList.Add ( EvcStatics.Enumerations.getOption ( EvModuleCodes.Administration_Module ) );
      loadedModulesList.Add ( EvcStatics.Enumerations.getOption ( EvModuleCodes.Auxiliary_Subject_Data ) );
      loadedModulesList.Add ( EvcStatics.Enumerations.getOption ( EvModuleCodes.Management_Module ) );
      loadedModulesList.Add ( EvcStatics.Enumerations.getOption ( EvModuleCodes.Imaging_Module ) );
      loadedModulesList.Add ( EvcStatics.Enumerations.getOption ( EvModuleCodes.Integration_Module ) );


      return loadedModulesList;

    }//END addRole method.

    //  =================================================================================
    /// <summary>
    ///   This method adds new role to the enumeration list of menu roles
    /// </summary>
    /// <param name="Modules">String delimited list of modules.</param>
    /// <param name="SelectionList">boolen add option list if true.</param>
    /// <returns>List of EvOption </returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Convert a role object into string format
    /// 
    /// 2. Retrun true, if new role is added
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public static List<EvOption>  getRoleList ( String Modules, bool SelectionList )
    {
      //
      // Get the string value of the selected module
      // 
      List<EvOption> optionlist = new List<EvOption>();

      if ( SelectionList == true )
      {
        optionlist.Add ( new EvOption ( ) );
      }

      optionlist.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Administrator ) );
      optionlist.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Evado_Administrator ) );
      optionlist.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Manager ) );
     
      if ( Modules.Contains ( EvModuleCodes.Management_Module.ToString ( ) ) == true )
      {
        //optionlist.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Project_Finance ) );
        //optionlist.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Project_Budget ) );
      }
      
      //optionlist.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Project_Designer ) );
      optionlist.Add ( EvcStatics.Enumerations.getOption ( EvRoleList.Coordinator ) );


      return optionlist;

    }//END addRole method.

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END class EvMenuItem

}//END Namespace Evado.Model
