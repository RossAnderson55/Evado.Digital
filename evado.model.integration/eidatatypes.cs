/***************************************************************************************
 * <copyright file="Evado.Model.Integration\EiDataTypes.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c)  2002 - 2021  EVADO HOLDING PTY. LTD..  All rights reserved.
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
  ///  this enumeration defines the types of query that are to be executed.
  /// </summary>
  [Serializable]
  public enum EiDataTypes
  {
    Null = 0, 
    /// <summary>
    /// This enumeration defines string text data QueryType.
    /// </summary>
    Text = 1,

    /// <summary>
    /// This enumeration defines the boolean data QueryType.
    /// </summary>
    Boolean = 2,

    /// <summary>
    /// This enumeration defines floating point data QueryType.
    /// </summary>
    Floating_Point = 3,

    /// <summary>
    /// This enumeration defines double precision floating point data QueryType.
    /// </summary>
    Double_Floating_Point = 4,

    /// <summary>
    /// This enumeration defines date time data QueryType.
    /// </summary>
    Integer = 5,

    /// <summary>
    /// This enumeration defines date time data QueryType.
    /// </summary>
    Date = 6,

    /// <summary>
    /// This enumeration defines date time data QueryType.
    /// </summary>
    Time = 7,

    /// <summary>
    /// This enumeration defines Telephone number.
    /// </summary>
    Telephone_Number = 8,

    /// <summary>
    /// This enumeration defines Email addres.
    /// </summary>
    Email_Address = 8,

  }//END EiQueryTypes enumeration.

}//END Namespace Evado.Model.Integration
