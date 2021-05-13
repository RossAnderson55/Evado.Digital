/***************************************************************************************
 * <copyright file="EvFormSection.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the EvFormSection data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

using Evado.Model;

namespace Evado.Digital.Model
{

  /// <summary>
  /// Business entity used to model accounts
  /// </summary>
  [Serializable]
  public class EdRecordMedia
  {
    #region Class initialisation

    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EdRecordMedia ( )
    {
      Url = String.Empty;
      Title = String.Empty;
      Width = 0;
      Height = 0;
    }

    #endregion

    #region Class property

    /// <summary>
    /// This property contains the number of a form section
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// This property contains the Order of a form section
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// This property contains a section of a form
    /// </summary>
    public string Title { get; set; }

    String _url = String.Empty;
    /// <summary>
    /// This property contains the media url
    /// </summary>
    public string Url
    {
      get
      {
        return this._url;
      }
      set
      {
        this._url = value;

        if ( this._url.Contains ( "?" ) == true )
        {
          int parmIndex = this._url.IndexOf ( '?' );
          this._url = this._url.Substring ( 0, parmIndex );
        }

      }
    }

    #endregion

    private char delimiter = '^';

    public string Data
    {
      get
      {
        return this.Url + delimiter + this.Title + delimiter + this.Width + delimiter + this.Height;
      }

      set
      {
        if ( value == String.Empty )
        {
          return;
        }

        string [ ] arrValue = value.Split ( delimiter );
        if ( arrValue.Length < 4 )
        {
          return;
        }

        this.Url = arrValue [ 0 ];
        this.Title = arrValue [ 1 ];
        this.Width =  Evado.Model.EvStatics.getInteger( arrValue [ 2 ] );
        this.Height =  Evado.Model.EvStatics.getInteger ( arrValue [ 3 ] );
      }
    }

  }//END EvFormSection class

} // Close namespace Evado.Digital.Model
