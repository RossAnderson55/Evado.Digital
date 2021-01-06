/***************************************************************************************
 * <copyright file="EvTrialRole.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvTrialRole data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
    /// <summary>
    /// This enumeartion list defines role options
    /// </summary>
    public enum EvRoleList
    {
      /// <summary>
      /// This enumeration defines null value or not selection state
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines an Evado administrator role
      /// </summary>
      Evado_Administrator,

      /// <summary>
      /// This enumeration defines an Evado Manager role
      /// </summary>
      Evado_Manager,

      /// <summary>
      /// This enumeration defines an Evado staff role
      /// </summary>
      Evado_Staff,

      /// <summary>
      /// This enumeration defines an administrator role
      /// </summary>
      Administrator,

      /// <summary>
      /// This enumeration defines the manager role
      /// </summary>
      Manager,

      /// <summary>
      /// This enumeration defines the  coordinator role
      /// </summary>
      Coordinator,

      /// <summary>
      /// This enumeration defines the project coordinator role
      /// </summary>
      Application_User,


    }

}//END namespace Evado.Model.Digital