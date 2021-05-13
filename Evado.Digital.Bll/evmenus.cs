/***************************************************************************************
 * <copyright file="BLL\EvMenus.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 * 
 * Description: 
 *  This class contains the EvCommonFormFields business object.
 *
 ****************************************************************************************/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

//Evado. namespace references.
using Evado.Digital.Model;
using Evado.Model;


namespace Evado.Digital.Bll
{
  /// <summary>
  /// A business to manage Menus. This class uses MenuItem ResultData object for its content.
  /// </summary>
  public class EvMenus : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvMenus ( )
    {
      this.ClassNameSpace = "Evado.Digital.Bll.EvMenus.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvMenus ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Digital.Bll.EvMenus.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Digital.Dal.EvStaticSetting.LoggingLevel;
      }

      this._dalMenus = new  Evado.Digital.Dal.EdMenus ( Settings );
    }
    #endregion

    #region Class initialization and Property
    // 
    // Create instantiate the DAL class 
    // 
    private  Evado.Digital.Dal.EdMenus _dalMenus = new  Evado.Digital.Dal.EdMenus ( );

    #endregion

    #region Class methods
    // =====================================================================================
    /// <summary>
    /// This class returns a list of menu item objects
    /// </summary>
    /// <param name="PlatformId">string: (Mandatory) The Site string.</param>
    /// <param name="MenuGroup">string: (Optional) The Menu Group string.</param>
    /// <param name="Modules">string: (Optional) The Module string.</param>
    /// <param name="UserRoleList">string: (Optional) The string list of roles.</param>
    /// <returns>List of EvMenuItem: A list of menu item ResultData objects</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Get a list of menu item objects. 
    /// 
    /// 2. Loop through the menu items list
    /// 
    /// 3. Withdraw the menu items that do not have associated role or module. 
    /// 
    /// 4. Return a list of selection menu items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvMenuItem> getView ( 
      string PlatformId, string MenuGroup, string Modules, string UserRoleList )
    {
      this.LogMethod( "getView method " );
      this.LogDebug ( "PlatformId: " + PlatformId );
      this.LogDebug ( "MenuGroup: " + MenuGroup );
      this.LogDebug ( "Modules: " + Modules );
      this.LogDebug ( "UserRoleList: " + UserRoleList );

      string [ ] arrUserRoleList = UserRoleList.Split ( ';' );
      this.LogDebug ( "Role list length: " + arrUserRoleList.Length );

      //
      // Get a Menu items list. 
      //
      List<EvMenuItem> view = this._dalMenus.getView ( PlatformId, MenuGroup );
      this.LogClass ( this._dalMenus.Log );
      this.LogDebug ( "currentSchedule count: " + view.Count );

      for ( int count = 0; count < view.Count; count++ )
      {
        EvMenuItem item = (EvMenuItem) view [ count ];
        bool delete = true;

        //
        // Withdraw the menu items that do not have associated role. 
        //
        if ( arrUserRoleList.Length > 0 )
        {
          foreach ( string role in arrUserRoleList )
          {
            if ( item.RoleList.ToLower ( ).IndexOf ( role.Trim ( ).ToLower ( ) ) >= 0 )
            {
              delete = false;
            }
          }// Check the role currentSchedule for matches

        }// Null role currentSchedule

        // 
        // Withdraw menu newField if role missing.
        // 
        if ( delete == true )
        {
          view.RemoveAt ( count );
          count--;
        }

      }//END menu list iteration

      return view;

    }//End getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class gets a list of MenuItem objects.
    /// </summary>
    /// <param name="PlatformId">string: (Mandatory) The Site string.</param>
    /// <param name="MenuGroup">string: (Optional) The menu group string.</param>
    /// <returns>List of EvMenuItem:  A list of MenuItem ResultData objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Get a selection list of Menu item objects. 
    /// 
    /// 2. Return the selection list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvMenuItem> getView ( 
      string PlatformId, string MenuGroup )
    {
      this.LogMethod ( "getView method." );
      this.LogDebug ( "PlatformId: " + PlatformId );
      this.LogDebug ( "MenuGroup: " + MenuGroup );

      List<EvMenuItem> view = this._dalMenus.getView ( PlatformId, MenuGroup );

      this.LogClass ( this._dalMenus.Log );

      return view;

    }//End getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of option objects for selection Groups based on Site value. 
    /// </summary>
    /// <param name="PlatformId">string: (Mandatory) The Site string.</param>
    /// <returns>List of EvOption: A list of option ResultData objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Retrieve a selection list of Group ResultData objects. 
    /// 
    /// 2. Return a selection list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getGroupList ( string PlatformId )
    {
      this.LogMethod ( "getGroupList method." );
      this.LogDebug ( "Site: " + PlatformId );

      List<EvOption> List = this._dalMenus.getGroupList ( PlatformId );
      this.LogClass ( this._dalMenus.Log );

      return List;

    }//End getGroupList method.

    // =====================================================================================
    /// <summary>
    /// This class retrieves the MenuItem datatable based on MenuName
    /// </summary>
    /// <param name="MenuName">string: (Mandatory) The Menu Name string.</param>
    /// <returns>EvMenuItem: A Menu Field ResultData object</returns>
    /// <remarks>
    /// This class consists of the following steps:
    /// 
    /// 1. Get the Menu ResultData object matching the MenuName
    /// 
    /// 2. Return the Menu ResultData object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvMenuItem getItem ( string MenuName )
    {
      this.LogMethod ( "getItem method." );
      this.LogDebug ( "MenuName: " + MenuName );

      EvMenuItem _Menu = this._dalMenus.getItem ( MenuName );
      this.LogClass ( this._dalMenus.Log );

      return _Menu;

    }//END getItem class

    // =====================================================================================
    /// <summary>
    /// This class retrieves the MenuItem datatable based on MenuGuid
    /// </summary>
    /// <param name="MenuGuid">Guid: (Mandatory) The Menu global unique identifier</param>
    /// <returns>EvMenuItem: A Menu Field ResultData object</returns>
    /// <remarks>
    /// This class consists of the following steps:
    /// 
    /// 1. Get the Menu ResultData object matching the MenuName
    /// 
    /// 2. Return the Menu ResultData object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvMenuItem getItem ( Guid MenuGuid )
    {
      this.LogMethod ( "getItem method" );
      this.LogDebug ( "MenuGuid: " + MenuGuid );

      EvMenuItem _Menu = this._dalMenus.getItem ( MenuGuid );
      this.LogClass ( this._dalMenus.Log );

      return _Menu;

    }//END getItem class

    // =====================================================================================
    /// <summary>
    /// This class saves items to MenuItem table. 
    /// </summary>
    /// <param name="MenuItem">EvMenuItem: MenuItem ResultData object</param>
    /// <returns>EvEventCodes: an event code for saving items</returns>
    /// <remarks>
    /// This class consists of the following steps: 
    /// 
    /// 1. If MenuItem's Title is empty, delete the MenuItem records. 
    /// 
    /// 2. If MenuItem's Guid is empty, add new the MenuItem records. 
    /// 
    /// 3. Else, update MenuItem records. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes saveItem ( EvMenuItem MenuItem )
    {
      this.LogMethod ( "saveItem method " );
      this.LogDebug ( "Guid: " + MenuItem.Guid );
      this.LogDebug ( "Title: " + MenuItem.Title );
      this.LogDebug ( "RoleList: " + MenuItem.RoleList );

      // 
      // Define the local variables.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      //
      // If MenuItem's title is empty, delete the Menu Items.
      // 
      if ( MenuItem.Title == String.Empty )
      {
        this.LogDebug ( "DELETING ITEM" );

        iReturn = this._dalMenus.deleteItem ( MenuItem );
        this.LogClass ( this._dalMenus.Log );
        return iReturn;
      }

      //
      // If the MenuItem Guid is empty, create a new MenuItem record.
      // 
      if ( MenuItem.Guid == Guid.Empty )
      {
        this.LogDebug ( "ADDING ITEM" );
        iReturn = this._dalMenus.addItem ( MenuItem );
        this.LogClass ( this._dalMenus.Log );

        return iReturn;
      }

      this.LogDebug ( "UPDATING ITEM" );

      //
      // Update the MenuItem record.
      // 
      iReturn = this._dalMenus.updateItem ( MenuItem );
      this.LogClass ( this._dalMenus.Log );
      return iReturn;

    }//END saveItem class
    #endregion

  }//END Menus Class.

}//END namespace Evado.Evado.BLL 
