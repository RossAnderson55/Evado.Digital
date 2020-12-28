/***************************************************************************************
 * <copyright file="EvFormContent.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvFormContent data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace Evado.Model.Digital
{
  /// <summary>
  ///  This Xml data class contains the trial objects Xml content.
  /// </summary>
  [Serializable]
  public class EvFormContent
  {
    #region internal variables.
    private List<EvUserSignoff> _Signoffs = new List<EvUserSignoff>();
    private List<EvFormRecordComment> _AnnotationList = new List<EvFormRecordComment>();
    private List<EvFormRecordComment> _CommentsList = new List<EvFormRecordComment>();

    private EvFormStaticField _RecordSubject = new EvFormStaticField();
    private EvFormStaticField _StartDate = new EvFormStaticField();
    private EvFormStaticField _FinishDate = new EvFormStaticField();
    private EvFormStaticField _ReferenceId = new EvFormStaticField();

    private string _Country = String.Empty;
    private bool _PriorToTrial = false;

    #endregion

    #region class properties
    /// <summary>
    /// This property contains a sign off list of a form content.
    /// </summary>
    public List<EvUserSignoff> Signoffs
    {
      get
      {
        return this._Signoffs;
      }
      set
      {
        this._Signoffs = value;
      }
    }

    /// <summary>
    /// This property contains a comment list of a form content.
    /// </summary>
    public List<EvFormRecordComment> CommentsList
    {
      get
      {
        return this._CommentsList;
      }
      set
      {
        this._CommentsList = value;
      }
    }

    /// <summary>
    /// This property contains a annotation list of a form content.
    /// </summary>
    public List<EvFormRecordComment> AnnotationList
    {
      get
      {
        return this._AnnotationList;
      }
      set
      {
        this._AnnotationList = value;
      }
    }

    /// <summary>
    /// This property contains a record subject of a form content.
    /// </summary>
    public EvFormStaticField RecordSubject
    {
      get
      {
        return this._RecordSubject;
      }
      set
      {
        this._RecordSubject = value;
      }
    }

    /// <summary>
    /// This property contains a start date object of a form content.
    /// </summary>
    public EvFormStaticField StartDate
    {
      get
      {
        return this._StartDate;
      }
      set
      {
        this._StartDate = value;
      }
    }

    /// <summary>
    /// This property contains a finish date object of a form content.
    /// </summary>
    public EvFormStaticField FinishDate
    {
      get
      {
        return this._FinishDate;
      }
      set
      {
        this._FinishDate = value;
      }
    }

    /// <summary>
    /// This property contains a reference identifier object of a form content.
    /// </summary>
    public EvFormStaticField ReferenceId
    {
      get
      {
        return this._ReferenceId;
      }
      set
      {
        this._ReferenceId = value;
      }
    }

    /// <summary>
    /// This property contains a country of a form content.
    /// </summary>
    public string Country
    {
      get
      {
        return this._Country;
      }
      set
      {
        this._Country = value;
      }
    }

    /// <summary>
    /// This property indicate whether a form content is a prior to trial.
    /// </summary>
    public Boolean PriorToTrial
    {
      get
      {
        return this._PriorToTrial;
      }
      set
      {
        this._PriorToTrial = value;
      }
    }


    #endregion

  }//END EvFormContent class

}//END namespace Evado.Model.Digital
