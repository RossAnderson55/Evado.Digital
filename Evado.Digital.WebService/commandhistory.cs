/***************************************************************************************
 * <copyright file="Evado.Digital.WebService\CommandHistory.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2013 - 2018 EVADO HOLDING PTY. LTD.  All rights reserved.
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
 *  This class contains the user naviation hisstory functinality.
 *
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Web.SessionState;

using Evado.Model.UniForm;
using Evado.Model;

namespace Evado.UniForm
{
  /// <summary>
  /// This class manages the user navigation history to enable a user to navigate up the groupCommand hierarchy they have created.
  /// </summary>
  public class CommandHistory
  {
    #region class initialisation methods

    // ==================================================================================
    /// <summary>
    /// The class initialisation method.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public CommandHistory ( )
    {
      this.LogInitialMethod ( "CommandHistory method" );

    }

    // ==================================================================================
    /// <summary>
    /// The class initialisation method.
    /// </summary>
    /// <param name="SessionState">HttpSessionState object from the web service or layer.</param>
    /// <param name="GlobalObjects">Hasthtable collection of all the global objects.</param>
    /// <param name="ServiceUserProfile">The user Id of the current user.</param>
    // ----------------------------------------------------------------------------------
    public CommandHistory (
      Hashtable GlobalObjects,
      EvUserProfileBase ServiceUserProfile )
    {
      this.LogInitialMethod ( "CommandHistory class initialisation method" );
      this.LogInitialDebug ( "ServiceUserProfile.UserId: " + ServiceUserProfile.UserId );

      //
      // Initialise the sesion and Global object stores.
      //
      this._GlobalObjects = GlobalObjects;
      this._GlobalKey = ServiceUserProfile.UserId + Evado.Model.UniForm.EuStatics.GLOBAL_COMMAND_HISTORY;
      this._GlobalKey = this._GlobalKey.ToUpper ( );
      this.LogInitialDebug ( "GlobalKey: " + this._GlobalKey );

      this.LogInitialDebug ( "Attempting to load from Global object store." );

      if ( this._GlobalObjects [ this._GlobalKey ] != null )
      {
        this._CommandHistoryList = (List<Command>) this._GlobalObjects [ this._GlobalKey ];

        this.LogInitialDebug ( "History loaded from Global Object store." );
      }

      this.LogInitialDebug ( "Session CommandHistory count:" + this._CommandHistoryList.Count );

      if ( this._CommandHistoryList.Count == 0 )
      {
        this._CommandHistoryList.Add ( Command.getLogoutCommand ( ) );
      }

      this.LogInitialDebug ( "CommandHistory count:" + this._CommandHistoryList.Count );

    }

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Global Objects

    const string CONST_NAME_SPACE = "Evado.UniForm.CommandHistory.";

    //private HttpSessionState _SessionState;

    private Hashtable _GlobalObjects;

    private String _GlobalKey;

    /// <summary>
    /// This list contains a list of the server commands sent to the client.
    /// </summary>
    private List<Command> _CommandHistoryList = new List<Command> ( );

    private bool _DebugOn = false;
    /// <summary>
    /// This property sets the debug state for the class.
    /// </summary>
    public bool DebugOn
    {
      get { return _DebugOn; }
      set { _DebugOn = value; }
    }

    /// 
    /// Status stores the debug status information.
    /// 
    private StringBuilder _log = new StringBuilder ( );

    public String Log
    {
      get { return this._log.ToString ( ); }
    }

    public int Count
    {
      get { return this._CommandHistoryList.Count; }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Public Methods

    //  ==================================================================================
    /// <summary>
    /// this method returns the initialises the groupCommand history list
    /// </summary>
    /// <param name="HomePageCommand">ClientPageCommand object</param>
    //  ----------------------------------------------------------------------------------
    public void initialiseHistory ( Command HomePageCommand )
    {
      this.LogMethod ( "initialiseHistory method. " );
      this.LogDebug ( "GlobalKey: " + this._GlobalKey );

      //
      // Initialise the home page groupCommand.
      //
      this._CommandHistoryList = new List<Command> ( );

      if ( HomePageCommand.Id == Guid.Empty )
      {
        this.LogDebug ( "New Command Guid created." );
        HomePageCommand.Id = Guid.NewGuid ( );
      }

      //
      // Initialise the list with an empty element
      //
      this._CommandHistoryList.Add ( Command.getLogoutCommand ( ) );

      //
      // Add the home page groupCommand to th list.
      //
      this._CommandHistoryList.Add ( HomePageCommand );

      this.LogDebug ( listCommandHistory ( true ) );

      //
      // Update the session variable.
      //
      //this._SessionState [ CommandHistory.SESSION_COMMAND_HISTORY ] = this._CommandHistoryList;

      this._GlobalObjects [ this._GlobalKey ] = this._CommandHistoryList;

    }//END initialiseCommandHistoryList method.

    //  ==================================================================================
    /// <summary>
    /// This method deletes all of the passes groupCommand and all others in the last.
    /// </summary>
    /// <param name="includeGuid">Bool: true = diplsay the command Guid ID</param>
    //  ----------------------------------------------------------------------------------
    public String listCommandHistory ( bool includeGuid )
    {
      this.LogMethod ( "listCommandHistory method" );
      //
      // Initialise the methods variables and objects.
      //
      StringBuilder sbDisplayText = new StringBuilder ( );

      sbDisplayText.AppendLine ( "Command History list  count: " + this._CommandHistoryList.Count );

      //
      // Iterate through the list of groupCommand history.
      //
      for ( int count = 0; count < this._CommandHistoryList.Count; count++ )
      {
        Command command = this._CommandHistoryList [ count ];

        if ( includeGuid == true )
        {
          //
          // output the groupCommand in the history list.
          //
          sbDisplayText.AppendLine ( "No: " + count
            + " > Id: " + command.Id
            + ", Title: '" + command.Title
            + "', : '" + command.Type
            + "', A: '" + command.ApplicationId
            + "', O: '" + command.Object
            + "', M: '" + command.Method + "'" );
        }
        else
        {
          //
          // output the groupCommand in the history list.
          //
          sbDisplayText.AppendLine ( "No: " + count
            + " > Title: '" + command.Title
            + "', T: '" + command.Type
            + "', A: '" + command.ApplicationId
            + "', O: '" + command.Object
            + "', M: '" + command.Method + "'" );
        }
      }

      //
      // Return the display list.
      //
      return sbDisplayText.ToString ( );

    }//END displayServerPageCommandObjects method

    // ================================================================================
    /// <summary>
    /// This method get the default exit groupCommand.
    /// </summary>
    /// <param name="PageCommand">ClientPageCommand object</param>
    // ----------------------------------------------------------------------------------
    public Command getExitCommand ( Command PageCommand )
    {
      this._log = new StringBuilder ( );
      this.LogMethod ( "getExitCommand method, " );
      this.LogDebug ( "GlobalKey: '" + this._GlobalKey + "'" );
      this.LogDebug ( "PageCommand:" + PageCommand.getAsString ( false, false ) );

      //
      // Initialise the methods variables and objects.
      //
      Command exitCommand = this.getLastCommand ( );

      //
      // debug log of the objects in the groupCommand list.
      //
      this.LogDebug ( this.listCommandHistory ( true ) );

      //
      // Select the exit processing based on the groupCommand method.
      //
      switch ( PageCommand.Method )
      {
        //
        // For customer methods update the exit groupCommand parameters and exit the method.
        //
        case ApplicationMethods.Custom_Method:
          {

            //
            // Update the exit commands parameters.
            //
            this.updateCommandParameters ( PageCommand );

            //
            // get the previous groupCommand.
            //
            exitCommand = this.getPreviousCommand ( PageCommand );

            //this.LogDebug ( "CUSTOM: Exit Command: " + exitCommand.Title );

            //
            // Exit the method returning the exit groupCommand.
            //
            return exitCommand;
          }

        //
        // For create, save and delete methods exit the method.
        //
        case ApplicationMethods.Save_Object:
        case ApplicationMethods.Delete_Object:
          {
            //
            // get the previous groupCommand.
            //
            exitCommand = this.getPreviousCommand ( exitCommand );

            //
            // Delete all commands after the exit groupCommand.
            //
            this.deleteAfterCommand ( exitCommand );

            // this.LogDebug ( "SAVE: Exit Command: " + exitCommand.Title );

            //
            // Exit the method returning the exit groupCommand.
            //
            return exitCommand;
          }

        //
        // For default append the method to the history list and exit the method.
        //
        default:
          {
            //
            // get the previous groupCommand.
            //
            exitCommand = this.getPreviousCommand ( PageCommand );

            //
            // Delete all commands after the exit groupCommand.
            //
            this.deleteAfterCommand ( exitCommand );

            //
            // Append this groupCommand to the groupCommand history list.
            //
            this.addCommand ( PageCommand );

            //this.LogDebug ( "Exit Command: " + exitCommand.Title + " > default exit command " );

            //
            // Exit the method returning the exit groupCommand.
            //
            return exitCommand;
          }

      }//END page groupCommand method switch

    }//END getExitCommand method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region private class methods

    // ================================================================================
    /// <summary>
    /// This method get the default exit groupCommand.
    /// </summary>
    /// <param name="PageCommand">ClientPageCommand object: containing the groupCommand that 
    /// is called on web service</param>
    // ----------------------------------------------------------------------------------
    private Command getPreviousCommand ( Command PageCommand )
    {
      this.LogMethod ( "getPreviousCommand method, " );
      //this.LogDebug ( "PageCommand:" + PageCommand.getAsString ( false, false ) );
      //
      // Intialise the methods variables and objects.
      //
      int previousCount = 0;
      Command previousCommand = new Command ( );

      //
      // if the list has more than one item it it .
      //
      if ( this._CommandHistoryList.Count == 0 )
      {
        //this.LogDebug ( "Empty list return empty command" );

        return previousCommand;
      }

      //
      // Iterate through the groupCommand history list to find the matching page groupCommand
      // Then return the previous groupCommand.
      //
      for ( int count = 1; count < this._CommandHistoryList.Count; count++ )
      {
        previousCount = count - 1;

        this.LogDebug ( count + " > " + this._CommandHistoryList [ count ].getAsString ( false, false ) );

        if ( this._CommandHistoryList [ count ].Id == PageCommand.Id )
        {
          //this.LogDebug ( "ID test: PREVOUS Command: " + this._CommandHistoryList [ previousCount ].Title );

          return this._CommandHistoryList [ previousCount ];
        }

        if ( this._CommandHistoryList [ count ].isCommand ( PageCommand ) == true )
        {
          //this.LogDebug ( "Value test: PREVOUS Command: " + this._CommandHistoryList [ previousCount ].Title );

          return this._CommandHistoryList [ previousCount ];
        }

      }//END iteration loop

      //this.LogDebug ( "Command not found get last command: " );

      //
      // Get the last groupCommand.
      //
      return this.getLastCommand ( );

    }//END getPreviousCommandObject method.

    //  ==================================================================================
    /// <summary>
    /// add the current page groupCommand to the previous page list
    /// </summary>
    /// <param name="PageCommand">ClientPageCommand object</param>
    //  ----------------------------------------------------------------------------------
    private void addCommand ( Command PageCommand )
    {
      this.LogMethod ( "addCommand method. " );
     // this.LogDebug ( "PageCommand: " + PageCommand.getAsString ( false, false ) );

      //
      // copy the command so the header list is not affected by the 
      // history add process.
      //
      Command pageCommand = PageCommand.copyObject ( );

      //
      // If the groupCommand identifier is empty then exit.
      //
      if ( PageCommand.Id == Guid.Empty )
      {
        //this.LogDebug ( "The command identifier is null." );
        return;
      }

      //
      // if the groupCommand is in the list exit.
      //
      if ( this.hasCommand ( pageCommand ) == true )
      {
        //this.LogDebug ( "The command exists in the list." );
        return;
      }

      //this.LogDebug ( "ADD the command to the list." );
      //
      // Empty the header values as they are set by the client.
      //
      pageCommand.Header = new List<Parameter> ( );

      //
      // If they do not match add the new previous page groupCommand to the list.
      //  This is to stop consequetive duplicates.
      //
      this._CommandHistoryList.Add ( pageCommand );

      //
      // Update the session variable.
      //
      //this._SessionState [ CommandHistory.SESSION_COMMAND_HISTORY ] = this._CommandHistoryList;

      this._GlobalObjects [ this._GlobalKey ] = this._CommandHistoryList;

    }//END addCommand method

    //  ==================================================================================
    /// <summary>
    /// This method deletes the page groupCommand and all others in the last.
    /// </summary>
    /// <param name="PageCommand">Guid Command identifer</param>
    //  ----------------------------------------------------------------------------------
    private bool hasCommand ( Command PageCommand )
    {
      //
      // Iterate through the groupCommand list to find a matching groupCommand
      //
      foreach ( Command command in this._CommandHistoryList )
      {
        if ( command.isCommand ( PageCommand ) == true )
        {
          return true;
        }
      }
      return false;
    }

    //  ==================================================================================
    /// <summary>
    /// This method deletes the page groupCommand and all others in the last.
    /// </summary>
    /// <param name="PageCommand">Guid Command identifer</param>
    //  ----------------------------------------------------------------------------------
    private int deleteAfterCommand ( Command PageCommand )
    {
      this.LogMethod ( "deleteAfterCommand method. " );
      //this.LogDebug ( " PageCommand: " + PageCommand.getAsString ( false, false ) );
      //
      // Initialise the methods variables and objects.
      //
      bool bCommandFound = false;

      //
      // Iterate through the groupCommand list to find the groupCommand and then 
      // delete all commands after that groupCommand from the list.
      //
      for ( int count = 0; count < this._CommandHistoryList.Count; count++ )
      {
        //
        // groupCommand has been found then delete the remaining commands.
        //
        if ( bCommandFound == true )
        {
          //this.LogDebug ( "Removing command: " + this._CommandHistoryList [ count ].Title );

          this._CommandHistoryList.RemoveAt ( count );
          count--;
        }

        //
        // Check to see if the groupCommand has been found.
        // This test is placed after the deleiton step so it is not deleted too.
        //
        if ( this._CommandHistoryList [ count ].Id == PageCommand.Id )
        {
          bCommandFound = true;
        }
      }

      //
      // Update the session variable.
      //
      //this._SessionState [ CommandHistory.SESSION_COMMAND_HISTORY ] = this._CommandHistoryList;

      this._GlobalObjects [ this._GlobalKey ] = this._CommandHistoryList;

      //
      // return the length of the groupCommand history list.
      //
      return this._CommandHistoryList.Count;

    }//END deleteAfterCommand method.

    //  ==================================================================================
    /// <summary>
    /// This method deletes the page groupCommand and all others in the last.
    /// </summary>
    /// <param name="PageCommand">Guid Command identifer</param>
    //  ----------------------------------------------------------------------------------
    private int deleteCommand ( Command PageCommand )
    {
      this.LogMethod ( "deleteCommand method. " );
      //this.LogDebug ( " PageCommand: " + PageCommand.getAsString ( false, false ) );
      //
      // Initialise the methods variables and objects.
      //

      //
      // Iterate through the groupCommand list to find the groupCommand and then 
      // delete all commands after that groupCommand from the list.
      //
      for ( int count = 0; count < this._CommandHistoryList.Count; count++ )
      {
        //
        // Check to see if the groupCommand has been found.
        // This test is placed after the deleiton step so it is not deleted too.
        //
        if ( this._CommandHistoryList [ count ].Id != PageCommand.Id )
        {
          continue;
        }

        //LogDebug ( "Removing command: " + this._CommandHistoryList [ count ].Title );

        this._CommandHistoryList.RemoveAt ( count );

        count--;
      }

      //
      // Update the session variable.
      //
      this._GlobalObjects [ this._GlobalKey ] = this._CommandHistoryList;

      //
      // return the length of the groupCommand history list.
      //
      return this._CommandHistoryList.Count;

    }//END deleteAfterCommand method.

    //  ==================================================================================
    /// <summary>
    /// Gets the last previous groupCommand
    /// </summary>
    /// <param name="CustomCommand">Guid Command identifer</param>
    /// <returns>ClientPageCommand</returns>
    //  ----------------------------------------------------------------------------------
    private bool updateCommandParameters ( Command CustomCommand )
    {
      this.LogMethod ( "updateCommandParameters method. " );
      //
      // Intialise the methods variables and objects.
      //
      int lastCount = this._CommandHistoryList.Count - 1;
      Command lastCommand = this._CommandHistoryList [ lastCount ];
      //
      // Get the custom groupCommand parameter.
      //
      ApplicationMethods method = CustomCommand.getCustomMethod ( );

      //
      // custom groupCommand parameter method is not a selection groupCommand exit.
      if ( method != ApplicationMethods.Get_Object
        && method != ApplicationMethods.List_of_Objects )
      {
        return false;
      }

      //
      // To cater for when the custom command is on the default or home page.
      //
      if ( lastCommand.ApplicationId == "Default"
        || lastCommand.Object == "Default" )
      {
        lastCommand.ApplicationId = CustomCommand.ApplicationId;
        lastCommand.Object = CustomCommand.Object;
      }

      //
      // Iterate through the groupCommand parameters to update them.
      //
      foreach ( Parameter customParameter in CustomCommand.Parameters )
      {
        lastCommand.AddParameter ( customParameter.Name, customParameter.Value );

      }//END iteration loop

      return true;

    }//END getCommonPageCommand method.

    //  ==================================================================================
    /// <summary>
    /// Gets the last groupCommand
    /// </summary>
    /// <returns>ClientPageCommand</returns>
    //  ----------------------------------------------------------------------------------
    public Command getLastCommand ( )
    {
      this.LogMethod ( "getLastCommand method. " );
      //
      // retrieve the last groupCommand in the list.
      //
      if ( this._CommandHistoryList.Count > 0 )
      {
        int lastCount = this._CommandHistoryList.Count - 1;
        Command command = this._CommandHistoryList [ lastCount ];

        //this.writeDebugLogLine ( "Last Command: " + command.getAsString ( false, false ) );

        return command;
      }

      //this.writeDebugLogLine ( "return default command. " );

      //
      // if the groupCommand is not found return the home page groupCommand.
      //
      return new Command ( );

    }//END getLastCommand method.

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Debug methods.

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogInitialMethod ( String Value )
    {
      this._log.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
      + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
      + CONST_NAME_SPACE + Value );
    }
    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="DebugLogString">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogInitialDebug ( String DebugLogString )
    {
      this._log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + DebugLogString );
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogMethod ( String Value )
    {
      if ( _DebugOn == true )
      {
        this._log.AppendLine ( Evado.Model.EvStatics.CONST_METHOD_START
        + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
        + CONST_NAME_SPACE + Value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="DebugLogString">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogDebug ( String DebugLogString )
    {
      if ( _DebugOn == true )
      {
        this._log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + DebugLogString );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Format">String: format text.</param>
    /// <param name="args">Array of objects as parameters.</param>
    // ----------------------------------------------------------------------------------
    protected void LogDebug ( String Format, params object [ ] args )
    {
      if ( _DebugOn == true )
      {
        this._log.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " +
          String.Format ( Format, args ) );
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END CommandHistory Class

}//END namespace