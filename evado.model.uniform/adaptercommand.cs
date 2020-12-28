using System;

using Evado.Model;

namespace Evado.Model.UniForm
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
      Evado.Model.UniForm.Command PageCommand,
      Evado.Model.UniForm.Command ExitCommand)
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
