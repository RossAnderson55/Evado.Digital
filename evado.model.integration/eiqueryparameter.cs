/***************************************************************************************
 * <copyright file="Evado.Model.Integration\EiQueryParameter.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvCaseReportForms business object.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evado.Model.Integration
{
  /// <summary>
  /// This model class defines the Web Service Query structure.
  /// </summary>
  [Serializable]
  public class EiQueryParameter
  {
    //==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    public EiQueryParameter ( )
    {
    }

    //==================================================================================
    /// <summary>
    /// This initialisation method initialises the class 
    /// and initialises the values.
    /// </summary>
    /// <param name="Name">EiQueryParameterNames enumeration</param>
    /// <param name="Value">String Value</param>
    //-----------------------------------------------------------------------------------
    public EiQueryParameter (
      EiQueryParameterNames Name,
      String Value )
    {
      this.Name = Name;
      this.Value = Value;
    }


    /// <summary>
    /// This property defines the parameter QueryType
    /// </summary>
    public EiQueryParameterNames Name {get; set; }

    /// <summary>
    /// This property contains the user account authorised to execute this query.
    /// </summary>
    public String Value {get; set; }


  }//END EvWebServiceQuery

}//END Namespace Evado.Model.Integration
