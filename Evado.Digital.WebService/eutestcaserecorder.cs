/***************************************************************************************
 * <copyright file="Evado.Digital.WebService\TestCaseRecorder.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD.  All rights reserved.
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
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Web.SessionState;

using Evado.UniForm.Model;
using Evado.Model;

namespace Evado.UniForm
{
  /// <summary>
  /// This class manages the user navigation history to enable a user to navigate up the groupCommand hierarchy they have created.
  /// </summary>
  public partial class EuTestCaseRecorder
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// The class initialisation method.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EuTestCaseRecorder ( )
    {
      this.LogInitMethod ( "TestCaseRecorder method" );

      //
      // Set the test case directory path
      //
      if ( ConfigurationManager.AppSettings [ CONST_TEST_CASE_PATH ] != null )
      {
        this._TestCaseDirectoryPath = ConfigurationManager.AppSettings [ CONST_TEST_CASE_PATH ];
      }

      this.LogInitiValue ( "TestCaseDirectoryPath: " + this._TestCaseDirectoryPath );

    }


    // ==================================================================================
    /// <summary>
    /// The class initialisation method.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EuTestCaseRecorder (
      String ApplicationPath )
    {
      this.LogInitMethod ( "TestCaseRecorder method" );

      this.LogInitiValue ( "ApplicationPath: " + ApplicationPath );

      //
      // Set the test case directory path
      //
      if ( ConfigurationManager.AppSettings [ CONST_TEST_CASE_PATH ] != null )
      {
        this._TestCaseDirectoryPath = ConfigurationManager.AppSettings [ CONST_TEST_CASE_PATH ];
      }

      if ( this._TestCaseDirectoryPath.Contains ( ":" ) == false )
      {
        this._TestCaseDirectoryPath = ApplicationPath + this._TestCaseDirectoryPath;
        this._TestCaseDirectoryPath = this._TestCaseDirectoryPath.Replace ( @"\.\", @"\" );
      }

      this.LogInitiValue ( "TestCaseDirectoryPath: " + this._TestCaseDirectoryPath );

    }

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class Global Objects

    public const string CONST_TEST_CASE_PATH = "TEST_CASE_PATH";

    private String _TestCaseDirectoryPath = @".\test_cases\";

    private string _OutputFilePath = String.Empty;

    private Command _PageCommand = new Command ( );

    private Command _ExitCommand = new Command ( );

    private AppData _ClientData = new AppData ( );

    private List<EutAction> _TestCaseList = new List<EutAction> ( );

    private int _SectionNo = 1;

    /// <summary>
    /// this property defines the section number of the test cases.
    /// </summary>
    public int SectionNo
    {
      get { return _SectionNo; }
      set { _SectionNo = value; }
    }


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    // ==================================================================================
    /// <summary>
    /// This method generates and then saves the application data test cases for the 
    /// passed page command and client data.
    /// </summary>
    /// <param name="PageCommand">Evado.UniForm.Model.Command: page command object.</param>
    /// <param name="ExitCommand">Evado.UniForm.Model.Command: exit command object.</param>
    /// <param name="ClientData">Evado.UniForm.Model.AppDate: client page data object.</param>
    // ----------------------------------------------------------------------------------
    public void saveTestCases (
      Command PageCommand,
      Command ExitCommand,
     AppData ClientData )
    {
      this._ClassLog = new StringBuilder ( );
      this.LogMethod ( "saveTestCases" );
      //
      // Initialise the method variables and objects.
      //
      this._PageCommand = PageCommand;
      this._ExitCommand = ExitCommand;
      this._ClientData = ClientData;
      this._TestCaseList = new List<EutAction> ( );

      this.LogValue ( "ClientData.Title: " + this._ClientData.Title );
      //
      // Generate the test cases
      //
      this.generateTestCases ( );

      this.LogValue ( "TestCaseList.Count: " + this._TestCaseList.Count );
      //
      // Save the test cases.
      //
      this.saveTestCaseList ( );

      //
      // Increment the test section number.
      //
      this._SectionNo++;


      this.LogMethodEnd ( "saveTestCases" );
    }//END Method

    #region generate test case methods.

    // ==================================================================================
    /// <summary>
    /// This method geneates the app data test cases.
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void generateTestCases ( )
    {
      this.LogMethod ( "generateTestCases" );
      //
      // Initialise the methods variables and objects.
      //
      EutAction action = new EutAction ( );

      //
      // Create test case for application object title.
      //
      action = new EutAction ( );
      action.Action = EutCommand.Test_Application_Title;
      action.setParameter ( EutCommandParameters.Value,
        this._ClientData.Title );

      this._TestCaseList.Add ( action );

      //
      // Create test case for page object title.
      //
      action = new EutAction ( );
      action.Action = EutCommand.Test_Page_Title;
      action.Description = EvStatics.enumValueToString ( action.Action );
      action.setParameter ( EutCommandParameters.Value,
        this._ClientData.Page.Title );

      this._TestCaseList.Add ( action );

      //
      // Create test case for page object status.
      //
      action = new EutAction ( );
      action.Action = EutCommand.Test_Page_Status;
      action.Description = EvStatics.enumValueToString ( action.Action );
      action.setParameter ( EutCommandParameters.Value,
        this._ClientData.Page.EditAccess.ToString ( ) );
      this._TestCaseList.Add ( action );

      //
      // Create test case for page group object title.
      //
      this.generateGroupTestCases ( );

    }//END method 

    // ==================================================================================
    /// <summary>
    /// This method creates page group test cases.
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void generatePageCommandTestCases ( )
    {
      this.LogMethod ( "generateGroupCommandTestCases" );
      //
      // Initialise the methods variables and objects.
      //
      EutAction action = new EutAction ( );

      //
      // Create test case for page field title.
      //
      for ( int i = 0; i < this._ClientData.Page.CommandList.Count; i++ )
      {
        Command command = this._ClientData.Page.CommandList [ i ];

        action = new EutAction ( );
        action.Action = EutCommand.Test_Page_Command_Index;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
           i.ToString ( ) );

        string title = command.Title;
        title = title.Replace ( "\r", String.Empty );
        title = title.Replace ( "\n", String.Empty );

        this._TestCaseList.Add ( action );
        action = new EutAction ( );
        action.Action = EutCommand.Test_Page_Command_Title;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
        title );

        this._TestCaseList.Add ( action );

        //
        // Create test case for field status.
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Command_Type;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          command.Type.ToString ( ) );

        this._TestCaseList.Add ( action );

        //
        // Create test case for field layout.
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Command_Application;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          command.ApplicationId );

        //
        // Create test case for field data type.
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Command_Object;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          command.Object );

        this._TestCaseList.Add ( action );

        //
        // Create test case for field is mandatory setting
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Command_Method;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          command.Method.ToString ( ) );

        this._TestCaseList.Add ( action );

        //
        // Create test case for group parameters
        //
        foreach ( Parameter prm in command.Parameters )
        {
          action = new EutAction ( );
          action.Action = EutCommand.Test_Group_Command_Parameter_Value;
          action.Description = EvStatics.enumValueToString ( action.Action );
          action.setParameter ( EutCommandParameters.Parameter_Name,
           prm.Name );
          action.setParameter ( EutCommandParameters.Value,
            prm.Value );

          this._TestCaseList.Add ( action );

        }//END parameter interation loop.

      }//END iteration loop

    }//END generateGroupTestCases method 

    // ==================================================================================
    /// <summary>
    /// This method creates page group test cases.
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void generateGroupTestCases ( )
    {
      this.LogMethod ( "generateGroupTestCases" );
      //
      // Initialise the methods variables and objects.
      //
      EutAction action = new EutAction ( );

      //
      // Create test case for page group object title.
      //
      foreach ( Group group in this._ClientData.Page.GroupList )
      {
        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Title;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          group.Title );

        this._TestCaseList.Add ( action );

        //
        // Create test case for group object status.
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Edit_Status;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          group.EditAccess.ToString ( ) );

        this._TestCaseList.Add ( action );

        //
        // Create test case for group object layout.
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Layout_Setting;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          group.Layout.ToString ( ) );

        //
        // Create test case for group object type.
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Type_Setting;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          group.GroupType.ToString ( ) );

        this._TestCaseList.Add ( action );

        //
        // Create test case for group object type.
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Command_Layout_Setting;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          group.CmdLayout.ToString ( ) );

        this._TestCaseList.Add ( action );

        //
        // Create test case for group parameters
        //
        foreach ( Parameter prm in group.Parameters )
        {
          action = new EutAction ( );
          action.Action = EutCommand.Test_Group_Parameter_Value;
          action.Description = EvStatics.enumValueToString ( action.Action );
          action.setParameter ( EutCommandParameters.Parameter_Name,
           prm.Name );
          action.setParameter ( EutCommandParameters.Value,
            prm.Value );

          this._TestCaseList.Add ( action );
        }//END parameter interation loop.

        //
        // Generat the group fields test cases.
        //
        this.generateGroupFieldsTestCases ( group );

        this.generateGroupCommandTestCases ( group );

      }//END iteration loop

    }//END generateGroupTestCases method 

    // ==================================================================================
    /// <summary>
    /// This method creates page group test cases.
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void generateGroupFieldsTestCases ( Group group )
    {
      this.LogMethod ( "generateFieldsTestCases" );
      //
      // Initialise the methods variables and objects.
      //
      EutAction action = new EutAction ( );

      //
      // Create test case for page field title.
      //
      foreach ( Field field in group.FieldList )
      {
        action = new EutAction ( );
        action.Action = EutCommand.Test_Field_Title;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          field.Title );

        this._TestCaseList.Add ( action );

        //
        // Create test case for field status.
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Edit_Status;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          field.EditAccess.ToString ( ) );

        this._TestCaseList.Add ( action );

        //
        // Create test case for field layout.
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Layout_Setting;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          field.Layout.ToString ( ) );

        //
        // Create test case for field data type.
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Field_Data_Type;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          field.Type.ToString ( ) );

        this._TestCaseList.Add ( action );

        //
        // Create test case for field is mandatory setting
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Field_Mandatory;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          field.Mandatory.ToString ( ) );

        this._TestCaseList.Add ( action );

        //
        // Create test case for field identifier
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Field_Identifier;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          field.FieldId.ToString ( ) );

        this._TestCaseList.Add ( action );

        //
        // Create test case for group parameters
        //
        foreach ( Parameter prm in field.Parameters )
        {
          action = new EutAction ( );
          action.Action = EutCommand.Test_Field_Parameter_Value;
          action.Description = EvStatics.enumValueToString ( action.Action );
          action.setParameter ( EutCommandParameters.Parameter_Name,
           prm.Name );
          action.setParameter ( EutCommandParameters.Value,
            prm.Value );

          this._TestCaseList.Add ( action );
        }//END parameter interation loop.

      }//END iteration loop

    }//END generateGroupTestCases method 

    // ==================================================================================
    /// <summary>
    /// This method creates page group test cases.
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void generateGroupCommandTestCases ( Group group )
    {
      this.LogMethod ( "generateGroupCommandTestCases" );
      //
      // Initialise the methods variables and objects.
      //
      EutAction action = new EutAction ( );

      //
      // Create test case for page field title.
      //
      for ( int i = 0; i < group.CommandList.Count; i++ )
      {
        Command command = group.CommandList [ i ];

        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Command_Index;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          i .ToString() );

        string title = command.Title;
        title = title.Replace ( "\r", String.Empty );
        title = title.Replace ( "\n", String.Empty );

        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Command_Title;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          title );

        this._TestCaseList.Add ( action );

        //
        // Create test case for field status.
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Command_Type;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          command.Type.ToString ( ) );

        this._TestCaseList.Add ( action );

        //
        // Create test case for field layout.
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Command_Application;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          command.ApplicationId );

        //
        // Create test case for field data type.
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Command_Object;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          command.Object );

        this._TestCaseList.Add ( action );

        //
        // Create test case for field is mandatory setting
        //
        action = new EutAction ( );
        action.Action = EutCommand.Test_Group_Command_Method;
        action.Description = EvStatics.enumValueToString ( action.Action );
        action.setParameter ( EutCommandParameters.Value,
          command.Method.ToString ( ) );

        this._TestCaseList.Add ( action );

        //
        // Create test case for group parameters
        //
        foreach ( Parameter prm in command.Parameters )
        {
          action = new EutAction ( );
          action.Action = EutCommand.Test_Group_Command_Parameter_Value;
          action.Description = EvStatics.enumValueToString ( action.Action );
          action.setParameter ( EutCommandParameters.Parameter_Name,
           prm.Name );
          action.setParameter ( EutCommandParameters.Value,
            prm.Value );

          this._TestCaseList.Add ( action );

        }//END parameter interation loop.

      }//END iteration loop

    }//END generateGroupTestCases method 

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region save methods.
    // ==================================================================================
    /// <summary>
    /// This method creates the output file path for the test cases.
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void setOutputFilePath ( )
    {
      this.LogMethod ( "setOutputFilePath" );
      this._OutputFilePath = this._TestCaseDirectoryPath;
      EvStatics.Files.createDirectory ( this._OutputFilePath );

      this._OutputFilePath += DateTime.Now.ToString ( "yy-MM-dd" ) + @"\"; ;

      EvStatics.Files.createDirectory ( this._OutputFilePath );

      this._OutputFilePath += this._PageCommand.ApplicationId + @"\";
      EvStatics.Files.createDirectory ( this._OutputFilePath );

      this._OutputFilePath += this._PageCommand.Object + @"\";
      EvStatics.Files.createDirectory ( this._OutputFilePath );

      this.LogValue ( "OutputFilePath: " + this._OutputFilePath );
    }

    // ==================================================================================
    /// <summary>
    /// This method creates the output file path for the test cases.
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void saveTestCaseList ( )
    {
      this.LogMethod ( "saveTestCases" );
      //
      // Initialise the methods variables and objects.
      //
      StringBuilder output = new StringBuilder ( );
      string fileName = string.Empty;

      //fileName += this._SectionNo + "_";

      String pageId = this._PageCommand.GetPageId ( );
      if ( pageId != String.Empty )
      {
        //fileName += pageId + "_";
      }

      if ( this._PageCommand.hasParameter ( CommandParameters.Custom_Method ) == false )
      {
        fileName += this._PageCommand.Method + ".CSV";
      }
      else
      {
        var method = this._PageCommand.getCustomMethod() ;
        fileName += method + ".CSV";
      }

      this.LogValue ( "fileName: " + fileName );

      //
      // Create the file path for the output test cases.
      //
      this.setOutputFilePath ( );

      //
      // Add the test case header.
      //
      output.AppendLine ( EutAction.TestCaseHeader ( ) );

      for ( int i = 0; i < this._TestCaseList.Count; i++ )
      {
        EutAction action = this._TestCaseList [ i ];
        action.TestNo = i;
        action.SectionNo = this._SectionNo;

        output.AppendLine ( action.CsvTestCase ( ) );
      }

      this.LogValue ( "output.Length: " + output.Length );
      //
      // Save the output to the disk file.
      //
      bool result = EvStatics.Files.saveFileAppend (
         this._OutputFilePath,
         fileName,
         output.ToString ( ) );

      this.LogValue ( "result: " + result );
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END CommandHistory Class

}//END namespace