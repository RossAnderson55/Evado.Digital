/***************************************************************************************
 * <copyright file="Evado.UniForm.Model\AdapterCommand.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2013 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the AdapterCommand data object.
 *
 ****************************************************************************************/
using System;

using Evado.Model;

namespace Evado.UniForm.Model
{
  /// <summary>
  /// This class defines the adapter command class to pacce the user profile, 
  /// page command and exit command to a web service application adatper.
  /// </summary>
  public class AdapterCommand
  {
    //===================================================================================
    /// <summary>
    /// This is the base initialisation method for this class.
    /// </summary>
    //-----------------------------------------------------------------------------------
    public AdapterCommand ( )
    { }

    //===================================================================================
    /// <summary>
    /// This is the parameterised initialisation method for this class
    /// </summary>
    //-----------------------------------------------------------------------------------
    public AdapterCommand (
      Evado.Model.EvUserProfileBase Profile,
      Evado.UniForm.Model.Command PageCommand,
      Evado.UniForm.Model.Command ExitCommand)
    {
      this.ServiceUserProfile = Profile;
      this.PageCommand = PageCommand;
      this.ExitCommand = ExitCommand;
    }

    /// <summary>
    /// This property contains a user profile.
    /// </summary>
    public EvUserProfileBase ServiceUserProfile { get; set; }

    /// <summary>
    /// This property contains object of Command class .
    /// </summary>
    public Command PageCommand { get; set; }

    /// <summary>
    /// This property contains object of Command class .
    /// </summary>
    public Command ExitCommand { get; set; }

  }//END AdapterInitialisation class

}//END NAMESPACE
