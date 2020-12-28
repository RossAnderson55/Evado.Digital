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
      /// This enumeration defines the project manager role
      /// </summary>
      Trial_Manager,

      /// <summary>
      /// This enumeration defines the project coordinator role
      /// </summary>
      Trial_Coordinator,

      /// <summary>
      /// This enumeration defines the monitor role
      /// </summary>
      Monitor,

      /// <summary>
      /// This enumeration defines the spoinsor role
      /// </summary>
      Sponsor,

      /// <summary>
      /// This enumeration defines the data manager role
      /// </summary>
      Data_Manager,

      /// <summary>
      /// This enumeration defines the data collector role
      /// </summary>
      Site_User,

      /// <summary>
      /// This enumeration defines the person who can register patients and subject role
      /// </summary>
      Patient_Registration,

      /// <summary>
      /// This enumeration defines the principal Investigator role
      /// </summary>
      Principal_Investigator,

      /// <summary>
      /// This enumeration defines the Investigator role
      /// </summary>
     Investigator,

      /// <summary>
      /// This enumeration defines the Patient role
      /// </summary>
      Patient,

      /// <summary>
      /// This enumeration defines the General Practioner role
      /// </summary>
      Patient_Doctor,

      //
      // Depreciated values.
      //
      /*
      /// <summary>
      /// This enumeration defines the data collector role
      /// </summary>
      Record_Author,

      /// <summary>
      /// This enumeration defines the project manager role
      /// </summary>
      Project_Manager,

      /// <summary>
      /// This enumeration defines the project coordinator role
      /// </summary>
      //Project_Coordinator,
       * 
      /// <summary>
      /// This enumeration defines the project budget role
      /// </summary>
      Project_Budget,

      /// <summary>
      /// This enumeration defines the project finance role
      /// </summary>
      Project_Finance,

      /// <summary>
      /// This enumeration defines the project designer role
      /// </summary>
      Project_Designer,

      /// <summary>
      /// This enumeration defines the data read rolee
      /// </summary>
      Record_Reader,

      /// <summary>
      /// This enumeration defines the not patient access role
      /// </summary>
      Not_Patient,

      /// <summary>
      /// This enumeration defines the no access role
      /// </summary>
      No_Access,
       * */
    }

}//END namespace Evado.Model.Digital