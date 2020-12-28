/***************************************************************************************
 * <copyright file="Evado.Model.UniForm\AbstractedPage.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2013 - 2017 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the AbstractedPage data object.
 *
 ****************************************************************************************/
using System;

namespace Evado.Model.UniForm
{
  /// <summary>
  /// This class defines the client page command object structure.
  /// </summary>
  [Serializable]
  public enum CommandParameters
    {
      /// <summary>
      /// This enumeration defines not selected state or null value.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration defines the field id parameter type.
      /// </summary>
      FieldId,

      /// <summary>
      /// This enumeration defines data object GUID identifier parameter value.
      /// </summary>
      Guid,

      /// <summary>
      /// This enumeration defines offline Page GUID identifier parameter value.
      /// </summary>
      Page_Data_Guid,

      /// <summary>
      /// This enumeration defines the method  command parameter value.
      /// </summary>
      Custom_Method,

      /// <summary>
      /// This enumeration defines the enable manadatroy fields command parameter value.
      /// </summary>
      Enable_Mandatory_Fields,

      /// <summary>
      /// This enumeration defines the command page identifier parameter value.
      /// </summary>
      Page_Id,

      /// <summary>
      /// This enumeration defines the short title parameter value.
      /// </summary>
      Short_Title,

      /// <summary>
      /// This enumeration defines if value is 1 then create a new instance of the object.
      /// </summary>
      Create_Object,

      /// <summary>
      /// This enumeration defines the URL for an http command.
      /// </summary>
      Link_Url,

      /// <summary>
      /// This enumeration defines the default background colour.
      /// </summary>
      BG_Default = 10,

      /// <summary>
      /// This enumeration defines the default background colour.
      /// </summary>
      BG_Alternative,

      /// <summary>
      /// This enumeration defines the selected background colour if the application object had been selected.
      /// </summary>
      BG_Highlighted,

      /// <summary>
      /// This enumeration defines the tiled image URL.
      /// </summary>
      Image_Url,

      /// <summary>
      /// This enumeration defines the tiled column the command is to be displayed in. 
      /// Else all text is below the image.
      /// </summary>
      Tiled_Column,

    }//END Command Parameter enumerated list.

}//END namespace