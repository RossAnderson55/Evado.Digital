/***************************************************************************************
 * <copyright file="EvURL.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvURL data object.
 *
 ****************************************************************************************/

using System;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This data class defines the application page object used to define pages that can be accessed from the
  /// menu and mobile environment.
  /// </summary>
  [Serializable]
  public partial class EvApplicationPage
  {
    #region class initialisation methods

    /// <summary>
    /// Default constructor
    /// </summary>
    public EvApplicationPage ( )
    {
    }

    /// <summary>
    /// Constructor with specified initial values
    /// </summary>
    /// <param name="PageId">EvApplicationPageIdList: Unique global identifier</param>
    /// <param name="Title">String: The title of the page</param>
    /// <param name="URL">String: relative URl of the page for web access</param>
    /// 
    public EvApplicationPage ( EvPageIds PageId, String Title, String URL )
    {
      this._PageId = PageId;
      this._Title = Title;
      this._PageUrl = URL;
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Properties

    private EvPageIds _PageId = EvPageIds.Null;
    /// <summary>
    /// This property contains the application page identifier used to define the page.
    /// </summary>
    public EvPageIds PageId
    {
      get { return this._PageId; }
      set { this._PageId = value; }
    }

    private string _Title = String.Empty;
    /// <summary>
    /// This property contains the application page title used to define the page.
    /// </summary>
    public string Title
    {
      get { return this._Title; }
      set { this._Title = value.Trim ( ); }
    }

    private string _PageUrl = String.Empty;
    /// <summary>
    /// This property contains page url of the Application Page.
    /// </summary>
    public string PageUrl
    {
      get { return this._PageUrl; }
      set { this._PageUrl = value.Trim ( ); }
    }

    //  =================================================================================
    /// <summary>
    /// This method returns the page Url, from the module directory.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public string getModulePageUrl ( )
    {
      return "." + this._PageUrl;
    }

    //  =================================================================================
    /// <summary>
    /// This method generates a list of PageIds 
    /// </summary>
    /// <returns></returns>
    //  ---------------------------------------------------------------------------------
    public static List<Evado.Model.EvOption> getPageIdSList ( bool IsSelectionList)
    {
      return EvStatics.getOptionsFromEnum ( typeof ( EvPageIds ), IsSelectionList ); 
    }


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END EvApplicationPage method

}//END namespace Evado.Model
