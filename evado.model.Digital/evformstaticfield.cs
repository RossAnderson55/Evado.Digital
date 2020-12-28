/***************************************************************************************
 * <copyright file="EvFormStaticField.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvFormStaticField data object.
 *
 ****************************************************************************************/

using System;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
    /// <summary>
    ///  This Xml data class contains the trial objects Xml content.
    /// </summary>
    [Serializable]
    public class EvFormStaticField
    {
      #region Class property
      List<EvFormRecordComment> _CommentList = new List<EvFormRecordComment>( );
      /// <summary>
      /// THis property contains a comment list of the form static field
      /// </summary>
      public List<EvFormRecordComment> CommentList
      {
        get { return _CommentList; }
        set { _CommentList = value; }
      }

      private bool _Queried = false;
      /// <summary>
      /// This property indicates whether the form static field is queried
      /// </summary>
      public bool Queried
      {
        get
        {
          return this._Queried;
        }
        set
        {
          this._Queried = value;
        }
      }

      String _FieldTitle = String.Empty;
      /// <summary>
      /// This property contains the title of the form static field
      /// </summary>
      public String FieldTitle
      {
        get { return _FieldTitle; }
        set { _FieldTitle = value; }
      }


      String _cDashMetadata = String.Empty;
      /// <summary>
      /// This property contains cDash metadata values. 
      /// </summary>
      public string cDashMetadata
      {
        get
        {
          return this._cDashMetadata;
        }
        set
        {
          this._cDashMetadata = value;
        }
      }
      #endregion

    }//END EvFormStaticField class

} //END namespace Evado.Model.Digital
