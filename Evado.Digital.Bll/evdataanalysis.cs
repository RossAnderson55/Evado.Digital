/***************************************************************************************
 * <copyright file="bll\EvDataAnalysis.cs" company="EVADO HOLDING PTY. LTD.">
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
 *
 ****************************************************************************************/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

//Evado. namespace references.
using Evado.Model;
using Evado.Digital.Model;


namespace Evado.Digital.Bll
{
  /// <summary>
  /// A business to manage aObjects. This class uses aObject ResultData object for its content.
  /// </summary>
  public class EvDataAnalysis : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvDataAnalysis ( )
    {
      this.ClassNameSpace = "Evado.Digital.Bll.Clinical.EvDataAnalysis.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvDataAnalysis ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Digital.Bll.Clinical.EvDataAnalysis.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Digital.Dal.EvStaticSetting.LoggingLevel;
      }

      this._dalRecordItems = new  Evado.Digital.Dal.EdRecordValues ( Settings );
    }
    #endregion

    #region Class variables and property
    // 
    // Create instantiate the DAL class 
    // 
    private  Evado.Digital.Dal.EdRecordValues _dalRecordItems = new  Evado.Digital.Dal.EdRecordValues( );

    #endregion

    #region RunQuery  methods
    // =====================================================================================
    /// <summary>
    /// This method returns the category index for the entered category.
    /// </summary>
    /// <param name="Categories">List of String: A list of Category strings</param>
    /// <param name="Category">string: a Category string to be indexed.</param>
    /// <returns>Integer: an index number of category</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Loop through the Categories list. 
    /// 
    /// 2. Return 999, if the Category is empty. 
    /// 
    /// 3. Else, return the category's index
    /// </remarks>
    //  ----------------------------------------------------------------------------------
    private int getCateogryIndex ( List<String> Categories, string Category )
    {
      // 
      // Iterate through the currentSchedule looking for the matching category.
      // 
      for ( int index = 0; index < Categories.Count; index++ )
      {
        if ( Categories [ index ] == String.Empty )
        {
          return 999;
        }
        if ( Categories [ index ] == Category )
        {
          return index;
        }
      }
      return 999;

    }//END getCateogryIndex method

    #endregion

  }//END EvDataAnalysis Class.

}//END namespace Evado.Evado.BLL 
