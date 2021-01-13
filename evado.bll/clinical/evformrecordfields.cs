/***************************************************************************************
 * <copyright file="bll\EvFormRecordFields.cs" company="EVADO HOLDING PTY. LTD.">
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
 ****************************************************************************************/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

//Evado. namespace references.
using Evado.Model;
using Evado.Dal;
using Evado.Model.Digital;

namespace Evado.Bll.Clinical
{
  /// <summary>
  /// A business to manage EvFormFields. This class uses EvFormField ResultData object for its content.
  /// </summary>
  public class EvFormRecordFields : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvFormRecordFields ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvFormRecordFields.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvFormRecordFields ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvFormRecordFields.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

      this._Dal_FormRecordFields = new Evado.Dal.Clinical.EdRecordValues ( Settings );
    }
    #endregion

    #region Class constants
    /// <summary>
    /// This constant defines the save item action for form record fields
    /// </summary>
    public const string ActionSaveItem = "SaveItem";

    /// <summary>
    /// This constant defines the confirm item action for form record fields
    /// </summary>
    public const string ActionConfirmItem = "ConfirmItem";

    /// <summary>
    /// This constant defines the query item action for form record fields
    /// </summary>
    public const string ActionQueryItem = "QueryItem";

    /// <summary>
    /// This constant defines the ResultData cleansing action for form record fields
    /// </summary>
    public const string ActionDataCleansing = "DataCleansing";

    /// <summary>
    /// This constant defines the new recorded action for form record fields
    /// </summary>
    public const string RECORDID_NEW = "NEW";
    #endregion

    #region Object Initialisation
    // 
    // Create instantiate the DAL class 
    // 
    private Evado.Dal.Clinical.EdRecordValues _Dal_FormRecordFields = new Evado.Dal.Clinical.EdRecordValues();

    /// <summary>
    /// Define the class property and state variable.
    /// </summary>
    private System.Text.StringBuilder _DebugLog = new System.Text.StringBuilder ( );
    /// <summary>
    /// This property contains the debug log string
    /// </summary>
    public string DebugLog
    {
      get
      {
        return _DebugLog.ToString();
      }
    }

    /// <summary>
    /// This property contains the html debug log string.
    /// </summary>
    public string DebugLog_Html
    {
      get
      {
        return DebugLog.Replace("\r\n", "<br/>");
      }
    }

    #endregion


  }//END EvFormRecordFields Class.

}//END namespace Evado.Bll.Clinical
