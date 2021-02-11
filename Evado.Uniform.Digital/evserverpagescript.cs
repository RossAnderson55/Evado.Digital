/***************************************************************************************
 * <copyright file="Evado.UniForm.Clinical\EvServerPageScript.cs" company="EVADO HOLDING PTY. LTD.">
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
 *
 ****************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Evado.Model;
using Evado.Bll;
using  Evado.Model.Digital;
using Evado.Bll.Digital;

using CSScriptLibrary;

namespace Evado.UniForm.Digital
{
  /// <summary>
  /// This class handles the dynamically compiled C# scripts for forms and records.
  /// </summary>
  public class EvServerPageScript
  {
    /************************************************************************************
     *
     *  THE CS SCRIPT PROJECT HAS BEEN ADDED TO EVADO HOLDING PTY. LTD. TO ENSURE .Net VERSINO COMPATIBILITY
     *
     ************************************************************************************/
    #region Class global enumerators.
    /// <summary>
    /// This enumeration defines the ScripEventTypes
    /// </summary>
    public enum ScripEventTypes
    {
      /// <summary>
      /// This enumeration value indicates ScripEventTypes is null type.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration value indicates ScripEventTypes is onLoadForm type.
      /// </summary>
      OnOpen,

      /// <summary>
      /// This enumeration value indicates ScripEventTypes is onUpdateForm type.
      /// </summary>
      OnUpdate
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class global objects.

    public const bool EnableCsScripts = true;
    private string _Script = String.Empty;
    /// 
    /// Define the script file to be used.
    ///

    private string _CsScriptPath = String.Empty;

    /// <summary>
    /// The class property contains CsScriptPath for the object.
    /// </summary>
    public string CsScriptPath
    {
      set
      {
        this._CsScriptPath = value;
      }
    }

    private System.Text.StringBuilder _debugLog = new StringBuilder ( );

    /// <summary>
    /// The class property contains status for the object.
    /// </summary>
    public string DebugLog
    {
      get
      {
        return _debugLog.ToString ( );
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class public methods

    //  ==================================================================================
    /// <summary>
    ///
    ///  This method is executed when the field ResultData is loaded and passess an array of 
    ///  fields to the script for initial processing.
    /// 
    /// </summary>
    /// <param name="Type">Type object</param>
    /// <param name="Form">Form object</param>
    /// <returns>bool : True: SubmittedRecords Successfully,  False : script exited permaturally</returns>
    //  ---------------------------------------------------------------------------------
    public EvEventCodes runScript ( EvServerPageScript.ScripEventTypes Type, EdRecord Form )
    {
      this._debugLog.AppendLine (  Evado.Model.Digital.EvcStatics.CONST_METHOD_START
       + "Evado.UniForm.Clinical.EvServerPageScript.runScript" );

      // 
      // Initialise the methods variables and objects.
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;
      string stFileName = Form.LayoutId;

      this._debugLog.AppendLine ( "Type: " + Type );
      this._debugLog.AppendLine ( "FileName: " + stFileName );

      if ( Form.Design.hasCsScript == false )
      {
        return EvEventCodes.CsFormScript_Form_Script_Enabled_False;
      }

      // 
      // Select the script be run.
      // 
      switch ( Type )
      {
        case EvServerPageScript.ScripEventTypes.OnOpen:
          {
            // 
            // Execute the onload script.
            // 
            iReturn = this.OnOpenPage (
              stFileName,
              Form );

            break;
          }
        case EvServerPageScript.ScripEventTypes.OnUpdate:
          {
            // 
            // Execute the onload script.
            // 
            iReturn = this.onUpdatePage (
             stFileName,
             Form );
            break;
          }
      }//END Script selection.

      // 
      // Return the outcome of the script.
      // 
      return iReturn;

    }//END runScript method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region class private methods.

    //  ==================================================================================
    /// <summary>
    ///
    ///  This method is executed when the field ResultData is loaded and passess an array of 
    ///  fields to the script for initial processing.
    /// 
    /// </summary>
    /// <typeparam name="FileName">FileName object</typeparam>
    /// <returns>bool : True: SubmittedRecords Successfully,  False : script exited permaturally</returns>
    //  ---------------------------------------------------------------------------------
    private EvEventCodes getScript ( String FileName )
    {
      this._debugLog.AppendLine (  Evado.Model.Digital.EvcStatics.CONST_METHOD_START
       + "Evado.UniForm.Clinical.EvServerPageScript.getScript" );
      this._debugLog.AppendLine ( "FileName: " + FileName );

      if ( FileName == String.Empty )
      {
        // 
        // return FileNameError : indicating that the file name was not provided.
        // 
        return EvEventCodes.CsFormScript_File_Name_Null_Error;

      }//END check filename passed.

      // 
      // Initialise the methods variables and objects.
      // 
      string filePath = this._CsScriptPath + FileName + ".cs";

      // 
      // Log the file name.
      // 
      this._debugLog.AppendLine ( "Set File Path: " + filePath );

      // 
      // If the script file is null set the path to empty.
      // 
      if ( File.Exists ( filePath ) == false )
      {
        // 
        // On error empty the scipt variable content.
        // 
        this._debugLog.AppendLine ( "Path does not exist." );
        this._Script = String.Empty;

        // 
        // stReturn ScriptNotFound indicating that the script has not been loaded.
        // 
        return EvEventCodes.CsFormScript_Script_Not_Found_Error;
      }

      // 
      // Read the script into memory.
      // 
      this._Script = File.ReadAllText ( filePath );

      //
      // stReturn true indicating that the script has been loadded.
      //
      return EvEventCodes.Ok;

    }//END getScript method.

    //  ==================================================================================
    /// <summary>
    /// 
    ///   This method is executed when the field ResultData is loaded and passess an array of 
    ///   fields to the script for initial processing.
    /// 
    /// </summary>
    /// <param name="FileName">FileName Object</param>
    /// <param name="Form">Form object</param>
    /// <returns>bool : True: SubmittedRecords Successfully,  False : script exited permaturally</returns>
    //  ---------------------------------------------------------------------------------
    private EvEventCodes OnOpenPage ( string FileName, EdRecord Form )
    {
      this._debugLog.AppendLine (  Evado.Model.Digital.EvcStatics.CONST_METHOD_START
       + "Evado.UniForm.Clinical.EvServerPageScript.OnOpenPage" );

      try
      {
        // 
        // Define local variables.
        // 
        bool bResult = true;

        // 
        // Read in the script file to be used.
        // 
        EvEventCodes iReturn = this.getScript ( FileName );

        if ( iReturn != EvEventCodes.Ok )
        {
          this._debugLog.AppendLine ( "Script not found." );

          // 
          // stReturn ScriptNotFound indicating that the script has not been loaded.
          // 
          return iReturn;
        }

        // 
        // Load the script into memory and flash compile it for use.
        // 
        AsmHelper scriptAsm = new AsmHelper ( CSScript.LoadCode ( this._Script, null, true ) );

        // 
        // Execute the script method
        // 
        bResult = (bool) scriptAsm.Invoke ( "ServerPageScript.OnOpenPage", Form );

        // 
        // Displose of the script object.
        // 
        scriptAsm.Dispose ( );

        this._debugLog.AppendLine ( "ScriptMessage: " + Form.ScriptMessage );

        // 
        // Set the return error status.
        // 
        if ( bResult == false )
        {
          // 
          // stReturn ReturnError: indicating that the script exited permaturely.
          // 
          return EvEventCodes.CsFormScript_Method_Return_Value_Error;
        }

        // 
        // stReturn the updates script result.
        //
        return EvEventCodes.Ok;

      }
      catch ( Exception Ex )
      {
        // 
        // Append the exception to the status variable.
        // 
        this._debugLog.AppendLine ( "Evado.UniForm.Clinical.EvServerPageScript.OnOpenPage exception:" );
        this._debugLog.AppendLine (  Evado.Model.Digital.EvcStatics.getException ( Ex ) );

        // 
        // stReturn MethodException : indicating that the script raised an Error Exception.
        // 
        return EvEventCodes.CsFormScript_Method_Exception_Error;

      }//END catch

    }//END onLoadForm method

    //  ==================================================================================
    /// <summary>
    /// 
    /// This method is executed when the field is updated.  The array of fields is 
    /// passed to the script to provide access to the other field values.
    /// 
    /// </summary>
    /// <param name="FileName">FileName object</param>
    /// <param name="Form">Form object</param>
    /// <returns>New computed OnUpdateForm value.</returns>
    //  ---------------------------------------------------------------------------------
    private EvEventCodes onUpdatePage ( string FileName, EdRecord Form )
    {
      this._debugLog.AppendLine (  Evado.Model.Digital.EvcStatics.CONST_METHOD_START
       + "Evado.UniForm.Clinical.EvServerPageScript.onUpdatePage" );

      try
      {
        // 
        // Define local variables.
        // 
        bool bResult = true;

        // 
        // Read in the script file to be used.
        // 
        EvEventCodes iReturn = getScript ( FileName );

        if ( iReturn != EvEventCodes.Ok )
        {
          this._debugLog.AppendLine ( "Script not found." );

          // 
          // stReturn ScriptNotFound indicating that the script has not been loaded.
          // 
          return iReturn;
        }

        // 
        // Load the script into memory and flash compile it for use.
        // 
        AsmHelper scriptAsm = new AsmHelper ( CSScript.LoadCode ( this._Script, null, true ) );

        // 
        // Execute the script method
        // 
        bResult = (bool) scriptAsm.Invoke ( "ServerPageScript.onUpdatePage", Form );

        // 
        // Displose of the script object.
        // 
        scriptAsm.Dispose ( );


        this._debugLog.AppendLine ( "ScriptMessage: " + Form.ScriptMessage );

        // 
        // Set the return error status.
        // 
        if ( bResult == false )
        {
          // 
          // stReturn ReturnError: indicating that the script exited permaturely.
          // 
          return EvEventCodes.CsFormScript_Method_Return_Value_Error;
        }

        // 
        // stReturn the updates script result.
        //
        return EvEventCodes.Ok;

      }
      catch ( Exception Ex )
      {
        // 
        // Append the exception to the status variable.
        // 
        this._debugLog.AppendLine ( "Evado.UniForm.Clinical.EvServerPageScript.onUpdatePage exception:" );
        this._debugLog.AppendLine (  Evado.Model.Digital.EvcStatics.getException ( Ex ) );

        // 
        // stReturn MethodException : indicating that the script raised an Error Exception.
        // 
        return EvEventCodes.CsFormScript_Method_Exception_Error;

      }//END catch

    }//END onUpdateForm method


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END evFormScripts class

}//END ScriptTest namespace
