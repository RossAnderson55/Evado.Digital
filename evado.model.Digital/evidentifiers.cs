/***************************************************************************************
 * <copyright file="Evado.UniForm.Model.eClinical\EvIdentifiers.cs" company="EVADO HOLDING PTY. LTD.">
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
 ****************************************************************************************/
using System;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This class contains the identifier for Evado indexes.
  /// </summary>
  [Serializable]
  public class EvIdentifiers
  {

    /// <summary>
    ///  This constant defines a display prefix of the identifiers
    /// </summary>
    public const string DISPLAY_PREFIX = "DISP_";

    /// <summary>
    ///  This constant defines a trial identifier
    /// </summary>
    public const string CUSTOMER_GUID = "CUSTOMER_GUID";

    /// <summary>
    ///  This constant defines a trial identifier
    /// </summary>
    public const string CUSTOMER_NO = "CUSTOMER_NO";

    /// <summary>
    ///  This constant defines a trial identifier
    /// </summary>
    public const string APPLICATION_ID = "TrialId";

    /// <summary>
    ///  This constant defines a record identifier
    /// </summary>
    public const string RECORD_ID = "RecordId";

    /// <summary>
    ///  This constant defines a form identifier
    /// </summary>
    public const string LAYOUT_ID = "LayoutId";

    /// <summary>
    ///  This constant defines a field identifier
    /// </summary>
    public const string FIELD_ID = "FieldId";

    /// <summary>
    ///  This constant defines an Alert identifier
    /// </summary>
    public const string ALERT_ID = "AlertId";

    /// <summary>
    ///  This constant defines a Record Type identifier
    /// </summary>
    public const string RECORD_TYPE = "RecordType";

    /// <summary>
    ///  This constant defines a Record date identifier
    /// </summary>
    public const string RECORD_DATE = "RecDate";

    /// <summary>
    ///  This constant defines an object state identifier
    /// </summary>
    public const string OBJECT_STATE = "State";

    /// <summary>
    ///  The constant defines a record is mandatory field identifier
    /// </summary>
    public const string LAYOUT_TITLE = "LayoutTitle";

    /// <summary>
    /// This constant defines a forms approval command paramenter.
    /// </summary>
    public const string LAYOUT_APPROVAL = "LayoutApproval";

    /// <summary>
    ///  The constant defines a submitted by field identifier
    /// </summary>
    public const string SUBMITTED_BY = "Submitted";

    /// <summary>
    ///  The constant defines a category field identifier
    /// </summary>
    public const string CATEGORIES = "Categories";

    /// <summary>
    ///  The constant defines a history field identifier
    /// </summary>
    public const string HISTORY = "History";

    public const string ORGANISATION_ID = "OrgId";


  }//END EvIdentifiers class

}//END namespace Evado.Model.Digital