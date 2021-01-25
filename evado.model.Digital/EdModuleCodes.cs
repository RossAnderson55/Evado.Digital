/***************************************************************************************
 * <copyright file="EvMenuItem.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvMenuItem data object.
 *
 ****************************************************************************************/

using System;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This enumeration list defines the states of module code.
  /// </summary>
  [Serializable]
  public enum EdModuleCodes
  {
    /// <summary>
    /// This enumeration selects All modules
    /// </summary>
    All_Modules = 0,

    /// <summary>
    /// This enumeration selects administration module.
    /// </summary>
    Administration_Module,

    /// <summary>
    /// This enumeration selects the Management module.
    /// </summary>
    Management_Module,

    /// <summary>
    /// This enumeration selects the trial module.
    /// </summary>
    Design_Module,

    /// <summary>
    /// This enumeration selects the trial module.
    /// </summary>
    Entity_Module,

    /// <summary>
    /// This enumeration selects the trial module.
    /// </summary>
    Record_Module,

    /// <summary>
    /// This enumeration selects the imaging module.
    /// </summary>
    Imaging_Module ,

    /// <summary>
    /// This enumeration selects the integration module.
    /// </summary>
    Integration_Module,

    /// <summary>
    /// This enumeration selects the Auxiliary subject data module.
    /// </summary>
    Auxiliary_Subject_Data,

    /// <summary>
    /// This enumeration defines the null selectoin 
    /// </summary>
    Null = 99,
  }

}//END Namespace Evado.Model
