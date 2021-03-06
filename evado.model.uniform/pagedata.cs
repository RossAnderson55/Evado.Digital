﻿/***************************************************************************************
 * <copyright file="Evado.UniForm.Model\PageData.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2013 - 2021 EVADO HOLDING PTY. LTD.  All rights reserved.
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
 *  This class contains the AbstractedPage data object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;


namespace Evado.UniForm.Model
{
  // ==================================================================================
  /// <summary>
  /// This class contains offline page data object.
  /// This object is populated with page command and field data values.
  /// </summary>
  // ----------------------------------------------------------------------------------
  [Serializable]
  public class PageData
  {
    #region Class PropertyList

    private Guid _Id = Guid.Empty;

    /// <summary>
    ///  This property contains an identifier for the PageData object.
    /// </summary>
    /// 
    public Guid Id
    {
      get { return this._Id; }
      set { this._Id = value; }
    }

    private String _PageId = String.Empty;

    /// <summary>
    ///  This property contains the page id.
    /// </summary>
    public String PageId
    {
      get { return this._PageId; }
      set { this._PageId = value; }
    }

    private String _AppId = String.Empty;

    /// <summary>
    /// This property contains the applicationa id.
    /// 
    /// </summary>

    public String AppId
    {
      get { return _AppId; }
      set { _AppId = value; }
    }

    private String _Object = String.Empty;

    /// <summary>
    /// This property contains the string object.
    /// 
    /// </summary>

    public String Object
    {
      get { return _Object; }
      set { _Object = value; }
    }

    private bool _New = false;

    /// <summary>
    ///  This property contains new data flag
    ///  True indicates that this is a new page data object.
    /// </summary>

    public bool New
    {
      get { return this._New; }
      set { this._New = value; }
    }

    private EditAccess _Status = EditAccess.Inherited;

    /// <summary>
    /// This property contains the status whether a page's fields are editable by the user
    /// when displayed in the device client.
    /// </summary>

    public EditAccess Status
    {
      get { return _Status; }
      set { _Status = value; }
    }

    private bool _Updated = false;

    /// <summary>
    ///  This property contains update data flag
    ///  True indicates that this page's values have been updated.
    /// </summary>

    public bool Updated
    {
      get { return this._Updated; }
      set { this._Updated = value; }
    }

    private String _SelectionFields = null;

    /// <summary>
    /// This property contains the page field delimited string identifying the page fields
    /// that are to be used to generated the page filter.
    /// </summary>
    public String SelectionFields
    {
      get { return _SelectionFields; }
      set { _SelectionFields = value; }
    }

    private String _SelectionFilter = null;

    /// <summary>
    /// This property contains the page filter string to retrieve this page into an active
    /// list of pages when the client is offline.
    /// 
    /// The filter string is generated by concatenation the values of the fields listed
    /// in the Selection fields.
    /// 
    /// This string must match the group's Offline_Selection_Filter parameter's value to be selected.
    /// 
    /// </summary>

    public String SelectionFilter
    {
      get { return _SelectionFilter; }
      set { _SelectionFilter = value; }
    }

    private List<PageReference> _CommandList = new List<PageReference>( );


    /// <summary>
    /// This property contains the list of page commands.
    /// The Parameter name field contains the Command Identifier
    /// The parameter value field contains the PageData object guid identifer that is to be called by the command. 
    /// </summary>

    public List<PageReference> CmdList
    {
      get { return this._CommandList; }
      set { this._CommandList = value; }
    }

    private List<DataObj> _DataList = new List<DataObj>( );
    /// <summary>
    /// This property contains a list of page field data values.
    /// 
    /// The Parameter Name field contains the FieldId for the field
    /// The Parameter Value field contains the field value.
    /// 
    /// Where the field is a multi-value fields the Field Id will have a siffex to define the Row and column,
    /// e.g. TableData field cell row 0 col 1 data would be identified by 'TableData_0_1'
    /// 
    /// </summary>
    // ----------------------------------------------------------------------------------
    public List<DataObj> DataList
    {
      get { return this._DataList; }
      set { this._DataList = value; }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class methods

    // ==================================================================================
    /// <summary>
    /// This method adds a command page reference object to the command reference list.
    /// </summary>
    /// <param name="PageReference">PageReference: Object referecing the command to be executed.</param>
    /// <remarks>
    /// This method consists of following steps. 
    /// 
    /// 1. Add the page reference object to the command list.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public void addPageReference( PageReference PageReference )
    {
      // 
      // Add the page reference object to the command list
      // 
      this._CommandList.Add ( PageReference );

    }//END addPageReference method

    // ==================================================================================
    /// <summary>
    /// This method adds a command page reference object to the command reference list.
    /// </summary>
    /// <param name="CommandGuid">Guid: Referecing the command to be executed.</param>
    /// <param name="PageDataGuid">Guid: Referenceing the page data object to be retrieved.</param>
    /// <returns>PageReference object  </returns>
    /// <remarks> 
    /// This method consists of following steps. 
    /// 
    /// 1. Initialise the page reference object with the method parameter values.
    /// 
    /// 2. Add the page reference object to the command list
    /// 
    /// 3. Return the page reference object.
    /// 
    /// </remarks>
    // ----------------------------------------------------------------------------------
    public PageReference addPageReference( Guid CommandGuid, Guid PageDataGuid )
    {
      // 
      // Initialise the page reference object with the method parameter values.
      // 
      PageReference pageReference = new PageReference( CommandGuid, PageDataGuid );

      // 
      // Add the page reference object to the command list
      // 
      this._CommandList.Add ( pageReference );

      // 
      // Return the page reference object.
      // 
      return pageReference;

    }//END addPageReference method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }
}//END namespace