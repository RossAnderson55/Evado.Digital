/***************************************************************************************
 * <copyright file="Evado.Digital.WebService\CommandHistory.cs" company="EVADO HOLDING PTY. LTD.">
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

using Evado.UniForm.Model;
using Evado.Model;

namespace Evado.UniForm
{
  /// <summary>
  /// This class manages the user navigation history to enable a user to navigate up the groupCommand hierarchy they have created.
  /// </summary>
  public partial class EuTestCaseRecorder
  {
    /// <summary>
    /// Business entity used to model accounts
    /// </summary>
    [Serializable]
    public enum EutCommandParameters
    {
      /// <summary>
      /// This enumeration value indicates null value.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration value indicates the parameter value is a user id.
      /// </summary>
      UserId,

      /// <summary>
      /// This enumeration value indicates the parameter value is a password.
      /// </summary>
      Password,

      /// <summary>
      /// This enumeration value indicates the parameter value is a list index value.
      /// </summary>
      Index,

      /// <summary>
      /// This enumeration value indicates the parameter value is a list count value.
      /// </summary>
      Count,

      /// <summary>
      /// This enumerated value data indicates the parameter value is a data value.
      /// </summary>
      Data_Type,

      /// <summary>
      /// This enumerated value data indicates the parameter value is a command value
      /// </summary>
      Command_Type,

      /// <summary>
      /// This enumerated value data indicates the parameter value is a command value
      /// </summary>
      Parameter_Name,

      /// <summary>
      /// This enumerated value data indicates the parameter value is a field value
      /// </summary>
      Value,

      /// <summary>
      /// This enumerated value indicates that the action will skip on action failure.
      /// </summary>
      SkipSectionOnFailure,

      /// <summary>
      /// This enumerated value indicates that the action will submit first group command to the service.
      /// </summary>
      SubmitGroupCommand,

      /// <summary>
      /// This enumerated value indicates that the action value will be reversed for the action normal result.
      /// </summary>
      ReverseStatus,

    }//END enumeration list.

    /// <summary>
    /// Business entity used to model command results
    /// </summary>
    [Serializable]
    public enum EutCommandResults
    {
      /// <summary>
      /// This enumeration value indicates test action executed successfully .
      /// </summary>
      Ok,
      /// <summary>
      /// This enumeration value indicates test case passed.
      /// </summary>
      Test_Case_Passed,

      /// <summary>
      /// This enumeration value indicates a required test parameter is missing.
      /// </summary>
      No_Parameters,

      /// <summary>
      /// This enumeration value indicates a required test parameter is missing.
      /// </summary>
      Parameter_Missing,

      /// <summary>
      /// This enumeration value indicates a required test parameter name is missing.
      /// </summary>
      Parameter_Name_Missing,

      /// <summary>
      /// This enumeration value indicates a required alue parameter value is missing.
      /// </summary>
      Parameter_Value_Missing,

      /// <summary>
      /// This enumeration value indicates a required alue parameter value is empty.
      /// </summary>
      Parameter_Value_Empty,

      /// <summary>
      /// This enumeration value indicates a required alue parameter index is missing.
      /// </summary>
      Parameter_Index_Missing,

      /// <summary>
      /// This enumeration value indicates a required test parameter datae type is missing.
      /// </summary>
      Parameter_Date_Type_Missing,

      /// <summary>
      /// This enumerated value data value's data value validation has failed.
      /// </summary>
      Parameter_Validation_Failure,

      /// <summary>
      /// This enumerated value data value's data value validation has failed.
      /// </summary>
      Object_Not_Found,

      /// <summary>
      /// This enumerated value data value's data value validation has failed.
      /// </summary>
      Data_Type_Validation_Failure,

      /// <summary>
      /// This enumerated value indicates test data value is missing from application data.
      /// </summary>
      Data_Value_Missing,

      /// <summary>
      /// This enumerated value indicated that the page group was not found in the list.
      /// </summary>
      Group_Not_Found,

      /// <summary>
      /// This enumerated value indicated that the command was not found in the list.
      /// </summary>
      Command_Not_Found,

      /// <summary>
      /// This enumerated value indicated that the page field was not found in the list.
      /// </summary>
      Field_Not_Found,

      /// <summary>
      /// This enumerated value indicates that the test script has has a test case failured.
      /// </summary>
      Test_Case_Failed,

      /// <summary>
      /// This enumeration indicates that a value has failed its data value or range validation.
      /// </summary>
      Failed_Data_Validation,

      /// <summary>
      /// This enumeration indicates that a value has failed integer validation..
      /// </summary>
      Failed_Value_Not_Integer,

    }//END enumeration list.

    public class EutParameter
    {
      public EutParameter ( )
      { }

      /// <summary>
      /// This value initialises the class withe parameter name and value.
      /// </summary>
      /// <param name="Name">EutParameterList enumerated value</param>
      /// <param name="Value">String: Parameter value</param>
      public EutParameter (
        EutCommandParameters Name,
        String Value )
      {
        this._Name = Name;
        this._Value = Value;
      }
      #region properties

      EutCommandParameters _Name = EutCommandParameters.Null;
      /// <summary>
      /// This parameter contains the parameter name.
      /// </summary>
      public EutCommandParameters Name
      {
        get
        {
          return this._Name;
        }
        set
        {
          this._Name = value;
        }
      }

      String _Value = String.Empty;

      /// <summary>
      /// THis property contains the parameter value
      /// </summary>
      public String Value
      {
        get
        {
          return _Value;
        }
        set
        {
          this._Value = value;
        }
      }

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion


    }//END EvDataChangeItem class

    /// <summary>
    /// This class contains the the pain and object. These page objects are rendered 
    /// into page fields on the client device.
    /// </summary>
    [Serializable]
    public enum EutCommand
    {
      /// <summary>
      /// This option is used to define non selected or null entry.
      /// </summary>
      Null = 0,  // json enumeration: 0

      #region CADO commands.
      /// <summary>
      /// This enumeration defines a header value for a test file or a test section.
      /// </summary>
      Get_Cado_Title,

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

      #region Page commands.
      /// <summary>
      /// This enumeration defines the page value for a test file or a test section.
      /// </summary>
      Get_Page_Title,

      /// <summary>
      /// This enumeration defines a select page exit as the current command
      /// </summary>
      Get_Exit_Command,

      /// <summary>
      /// This enumeration defines a select page command action as the current command
      /// </summary>
      Get_Page_Command_Count,

      /// <summary>
      /// This enumeration defines a select page command as the current command by title.
      /// </summary>
      Get_Page_Command,

      /// <summary>
      /// This enumeration defines a getting the next command in the page command list as the current command.
      /// </summary>
      Get_Page_Next_Command,

      /// <summary>
      /// This enumeration defines a select page command by is list index action.
      /// </summary>
      Get_Page_Command_by_Index,

      /// <summary>
      /// This enumeration defines a select page command if its title contains a text value.
      /// </summary>
      Get_Page_Command_Contains,

      /// <summary>
      /// This enumeration defines getting the current command parameter count.
      /// </summary>
      Get_Page_Command_Parameters,

      /// <summary>
      /// This enumeration defines a select page group action
      /// </summary>
      Get_Page_Group_Count,

      /// <summary>
      /// This enumeration defines a select page group action
      /// </summary>
      Get_Page_Group,

      /// <summary>
      /// This enumeration defines a getting the next group in the page list.
      /// </summary>
      Get_Page_Next_Group,

      /// <summary>
      /// This enumeration defines a select page group by index action
      /// </summary>
      Get_Page_Group_by_Index,

      /// <summary>
      /// This enumeration defines a select page group if it contains a value
      /// </summary>
      Get_Page_Group_Contains,

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

      #region Group commands.

      /// <summary>
      /// This enumeration defines a getting the group's parameters.
      /// </summary>
      Get_Group_Parameters,

      /// <summary>
      /// This enumeration defines a getting the group's parameters.
      /// </summary>
      Get_Group_Parameter,


      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

      #region Group Command commands.


      /// <summary>
      /// This enumeration defines a select page command count
      /// </summary>
      Get_Group_Command_Count,

      /// <summary>
      /// This enumeration defines a select page command List
      /// </summary>
      Get_Group_Command_List,

      /// <summary>
      /// This enumeration defines getting the first command in the group.
      /// </summary>
      Get_First_Group_Command,

      /// <summary>
      /// This enumeration defines getting the next command in the group.
      /// </summary>
      Get_Group_Next_Command,

      /// <summary>
      /// This enumeration defines a select group command action
      /// </summary>
      Get_Group_Command,

      /// <summary>
      /// This enumeration defines a select group command by is list index action
      /// </summary>
      Get_Group_Command_by_Index,

      /// <summary>
      /// This enumeration defines a select group command if it contains a value
      /// </summary>
      Get_Group_Command_Contains,

      /// <summary>
      /// This enumeration defines a select first group command
      /// </summary>
      Get_Group_First_Command,

      /// <summary>
      /// This enumeration defines a select group command if it contains a value
      /// </summary>
      Get_Command_Parameters,
      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

      #region Group Field commands.

      /// <summary>
      /// This enumeration defines a select group field count.
      /// </summary>
      Get_Group_Field_Count,

      /// <summary>
      /// This enumeration defines a select group field List.
      /// </summary>
      Get_Group_Field_List,

      /// <summary>
      /// This enumeration defines a select group field action
      /// </summary>
      Get_Next_Selection_Field,

      /// <summary>
      /// This enumeration defines a getting the next field in group field list.
      /// </summary>
      Get_Group_Next_Field,

      /// <summary>
      /// This enumeration defines a select group field action
      /// </summary>
      Get_Group_Field,

      /// <summary>
      /// This enumeration defines a select Page field by index action
      /// </summary>
      Get_Group_Field_By_Index,

      /// <summary>
      /// This enumeration defines a selection option by index action
      /// </summary>
      Get_Selection_Option_By_Index,

      /// <summary>
      /// This enumeration defines a selection option by value action
      /// </summary>
      Get_Selection_Option_By_Value,

      /// <summary>
      /// This enumeration defines a getting the currently selected fields value.
      /// </summary>
      Get_Field_Value,

      /// <summary>
      /// This enumeration defines updating of the currently selected fields value.
      /// </summary>
      Set_Field_Value,

      /// <summary>
      /// This enumeration defines a getting the field comment parameter value.
      /// </summary>
      Get_Field_Comment_Value,

      /// <summary>
      /// This enumeration defines a setting the field comment value.
      /// </summary>
      Set_Field_Comment_Value,

      /// <summary>
      /// This enumeration defines an update of the currently selected fields value.
      /// </summary>
      Get_Field_Parameters,

      /// <summary>
      /// This enumeration defines an increment to the suffix of the currently selected fields value.
      /// </summary>
      Increment_Field_Value,

      //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

      #region Command actions

      /// <summary>
      /// This enumeration defines the execute a user login command.
      /// </summary>
      Send_Login_Command,

      /// <summary>
      /// This enumeration defines the execute a user logout command.
      /// </summary>
      Send_Logout_Command,

      /// <summary>
      /// This enumeration defines a execute the page's exit command.
      /// </summary>
      Send_Exit_Command,

      /// <summary>
      /// This enumeration defines a execute the currently selected page command.
      /// </summary>
      Send_Current_Page_Command,

      /// <summary>
      /// This enumeration defines a execute the currently selected group command
      /// </summary>
      Send_Current_Group_Command,

      //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

      #region Test actions

      /// <summary>
      /// This enumeration defines device value test.
      /// </summary>
      Test_Device_Status,

      /// <summary>
      /// This enumeration defines a test case to compare the applications value.
      /// </summary>
      Test_Application_Title,

      /// <summary>
      /// This enumeration defines a test case to compare the page value.
      /// </summary>
      Test_Page_Title,

      /// <summary>
      /// This enumeration defines a test case to 
      /// </summary>
      Test_Page_Exit_Command_Title,

      /// <summary>
      /// This enumeration defines a test case to 
      /// </summary>
      Test_Page_Exit_Command_Type,

      /// <summary>
      /// This enumeration defines a test case to 
      /// </summary>
      Test_Page_Exit_Command_Application,

      /// <summary>
      /// This enumeration defines a test case to 
      /// </summary>
      Test_Page_Exit_Command_Object,

      /// <summary>
      /// This enumeration defines a test case to 
      /// </summary>
      Test_Page_Exit_Command_Method,

      /// <summary>
      /// This enumeration defines a test case to 
      /// </summary>
      Test_Page_Exit_Command_Custom_Method,

      /// <summary>
      /// This enumeration defines a test case to 
      /// </summary>
      Test_Page_Exit_Command_Parameter_Value,

      /// <summary>
      /// This enumeration defines a test case to 
      /// </summary>
      Test_Page_Command_Count,

      /// <summary>
      /// This enumeration defines a test case to 
      /// </summary>
      Test_Page_Command_Index,

      /// <summary>
      ///  by value and load it at the current page command.
      /// </summary>
      Test_Page_Command_Exists,

      /// <summary>
      /// This enumeration defines a test case to verify the currently selected page command's value.
      /// </summary>
      Test_Page_Command_Title,

      /// <summary>
      /// This enumeration defines a test case to verify the currently selected page command's value.
      /// </summary>
      Test_Page_Command_Type,

      /// <summary>
      /// This enumeration defines a test case to verify the currently selected page command's application id
      /// </summary>
      Test_Page_Command_Application,

      /// <summary>
      /// This enumeration defines a test case to verify the currently selected page command's application object.
      /// </summary>
      Test_Page_Command_Object,

      /// <summary>
      /// This enumeration defines a test case to verify the currently selected page command's value.
      /// </summary>
      Test_Page_Command_Method,

      /// <summary>
      /// This enumeration defines a test case to verify the currently selected page command's common value value
      /// </summary>
      Test_Page_Command_Custom_Method,

      /// <summary>
      /// This enumeration defines a test case to verify parameter value in the currently selected page command
      /// </summary>
      Test_Page_Command_Parameter_Value,

      /// <summary>
      /// This enumeration defines a test case to verify the currently selected page command's value value
      /// </summary>
      Test_Page_Status,

      /// <summary>
      /// This enumeration defines a test case to verify the page's group count with a value.
      /// </summary>
      Test_Page_Group_Count,

      /// <summary>
      /// This enumeration defines a test case to verify the selected group's title with given text. 
      /// </summary>
      Test_Group_Title,

      /// <summary>
      /// This enumeration defines a test case to verify the selected group's value with given text. 
      /// </summary>
      Test_Group_Description,

      /// <summary>
      /// This enumeration defines a test case to verify the selected group's edit value.
      /// </summary>
      Test_Group_Edit_Status,

      /// <summary>
      /// This enumeration defines a test case to verify the selected group's value setting.
      /// </summary>
      Test_Group_Layout_Setting,

      /// <summary>
      /// This enumeration defines a test case to verify the selected group's type setting.
      /// </summary>
      Test_Group_Type_Setting,

      /// <summary>
      /// This enumeration defines a test case to verify the selected group's type setting.
      /// </summary>
      Test_Group_Command_Layout_Setting,

      /// <summary>
      /// This enumeration defines a test case to verify a parameter value in the selected group.
      /// </summary>
      Test_Group_Parameter_Value,

      /// <summary>
      /// This enumeration defines a test case to verify the field count in the currently selected group.
      /// </summary>
      Test_Field_Count,

      /// <summary>
      /// This enumeration defines a test case to verify the field identifier in the currently selected group field.
      /// </summary>
      Test_Field_Identifier,

      /// <summary>
      /// This enumeration defines a test case to verify the value in the currently selected group field,
      /// </summary>
      Test_Field_Title,

      /// <summary>
      /// This enumeration defines a test case to verify the value of the currently selected group field.
      /// </summary>
      Test_Field_Description,

      /// <summary>
      /// This enumeration defines a test case to verify the data value of the currently selected group field.
      /// </summary>
      Test_Field_Data_Type,

      /// <summary>
      /// This enumeration defines a test case to verify the value setting of the currently selected group field.
      /// </summary>
      Test_Field_Layout_Setting,

      /// <summary>
      /// This enumeration defines a test case to verify the value setting of the currently selected group field.
      /// </summary>
      Test_Field_Edit_Status,

      /// <summary>
      /// This enumeration defines a test case to verify the value setting of the currently selected group field.
      /// </summary>
      Test_Field_Mandatory,

      /// <summary>
      /// This enumeration defines a test case to verify the value setting of the currently selected group field.
      /// </summary>
      Test_Field_Value,

      /// <summary>
      /// This enumeration defines a test case to verify the value setting of the currently selected group field.
      /// </summary>
      Test_Field_Parameter_Value,

      /// <summary>
      /// This enumeration defines a test case to verify the value setting of the currently selected group field.
      /// </summary>
      Test_Group_Command_Count,

      /// <summary>
      /// This enumeration defines a test case to verify the command exists of the currently selected group.
      /// By value and load it at the current group command.
      /// </summary>
      Test_Group_Command_Exist,

      /// <summary>
      /// This enumeration defines a test case to verify the index of the current group command.
      /// </summary>
      Test_Group_Command_Index,

      /// <summary>
      /// This enumeration defines a test case to verify the title of the current group command.
      /// </summary>
      Test_Group_Command_Title,

      /// <summary>
      /// This enumeration defines a test case to verify the value of the currently selected group command.
      /// </summary>
      Test_Group_Command_Type,

      /// <summary>
      /// This enumeration defines a test case to verify the application of the currently selected group command.
      /// </summary>
      Test_Group_Command_Application,

      /// <summary>
      /// This enumeration defines a test case to verify the application object of the currently selected group command.
      /// </summary>
      Test_Group_Command_Object,

      /// <summary>
      /// This enumeration defines a test case to verify the value of the currently selected group command.
      /// </summary>
      Test_Group_Command_Method,

      /// <summary>
      /// This enumeration defines a test case to verify the common value of the currently selected group command.
      /// </summary>
      Test_Group_Command_Custom_Method,

      /// <summary>
      /// This enumeration defines a test case to verify a parameter value of the currently selected group command.
      /// </summary>
      Test_Group_Command_Parameter_Value,

      //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

    }//END Enumeration

    /// <summary>
    /// Business entity used to model accounts
    /// </summary>
    [Serializable]
    private class EutAction
    {
      #region enumerators

      public enum EuTestCaseMembers
      {
        Section = 0,
        No = 1,
        Action = 2,
        Parameters = 3,
        Description = 4,
        TestResponse = 5,
        TestResult = 6,
        TestStatus = 7,
      }

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

      #region properties

      /// <summary>
      /// This property contains the section number.
      /// </summary>
      public int SectionNo { get; set; }

      /// <summary>
      /// This property contains the test action number.
      /// </summary>
      public int TestNo { get; set; }

      private EutCommand _Action = EutCommand.Null;

      /// <summary>
      /// This property contains the enumerated value of the test action to be executed.
      /// </summary>
      public EutCommand Action
      {
        get
        {
          return _Action;
        }
        set
        {
          _Action = value;
        }
      }

      String _Parameters = String.Empty;

      /// <summary>
      /// This property contains an encoded string of parameter values.
      /// </summary>
      public String Parameters
      {
        get
        {
          foreach ( EutParameter prm in this._optionList )
          {
            _Parameters += prm.Name + "=" + prm.Value + ";";
          }

          return _Parameters;


        }
        set
        {
          _Parameters = value;

          createParameterList ( );
        }
      }

      private List<EutParameter> _optionList = new List<EutParameter> ( );

      /// <summary>
      /// This property contains a list of the test action parameters.
      /// </summary>
      public List<EutParameter> ParmeterList
      {
        get { return _optionList; }
      }

      private String _Description = String.Empty;

      /// <summary>
      /// This property contains the value of the test action to be 
      /// carried out by the test harness.
      /// </summary>
      public String Description
      {
        get
        {
          return _Description;
        }
        set
        {
          this._Description = value;
        }
      }

      private String _Response = String.Empty;

      /// <summary>
      /// This property contains the response to the test action that 
      /// is displayed in the test report.
      /// </summary>
      public String Response
      {
        get
        {
          return _Response;
        }
        set
        {
          this._Response = value;
        }
      }

      /// <summary>
      /// This property add a new response to the existing response string.
      /// </summary>
      public String AddResponse
      {
        set
        {
          if ( this._Response != String.Empty )
          {
            this._Response += "; ";
          }
          this._Response += value;
        }
      }

      /// <summary>
      /// This property contains the result of the test action.
      /// </summary>
      public EutCommandResults Result { get; set; }

      /// <summary>
      /// This property displays the tests final value of true or false.
      /// </summary>
      public bool Status
      {
        get
        {
          //
          // reverse the result as false is the correct outcome for this test action.
          //
          if ( _ReverseStatus == true )
          {
            if ( Result == EutCommandResults.Ok
              || Result == EutCommandResults.Test_Case_Passed )
            {
              return false;
            }
            return true;
          }

          if ( Result == EutCommandResults.Ok
            || Result == EutCommandResults.Test_Case_Passed )
          {
            return true;
          }
          return false;
        }
      }

      private bool _ReverseStatus = false;

      /// <summary>
      /// This method contains the reverse value setting.
      /// </summary>
      public bool ReverseStatus
      {
        get { return _ReverseStatus; }
      }


      private bool _SkipToNextSection = false;

      /// <summary>
      /// This property indicates that the test script should skip to the next section or exit the script.
      /// </summary>
      public bool SkipToNextSection
      {
        get { return _SkipToNextSection; }
        set { _SkipToNextSection = value; }
      }

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion

      #region class methods

      //===================================================================================
      /// <summary>
      /// This value outputs the test result as a CSV record.
      /// </summary>
      /// <returns>String: as an encoded CSV record.</returns>
      //-----------------------------------------------------------------------------------
      public static String TestCaseHeader ( )
      {
        return "\"SectionNo\""
          + ",\"TestNo\""
          + ",\"TestAction\""
          + ",\"TestParameters\""
          + ",\"TestDescription\"";
      }

      //===================================================================================
      /// <summary>
      /// This value outputs the test result as a CSV record.
      /// </summary>
      /// <returns>String: as an encoded CSV record.</returns>
      //-----------------------------------------------------------------------------------
      public String CsvTestCase ( )
      {
        if ( this.Action == EutCommand.Null )
        {
          if ( this.SectionNo > 0 )
          {
            return "\"" + this.SectionNo
              + "\",\""
              + "\",\"" + this.Description
              + "\",\""
              + "\",\""
              + "\",\""
              + "\",\""
              + "\",\"" + "\"";
          }
          else
          {
            return String.Empty;
          }
        }

        String sectionNo = String.Empty;
        String no = String.Empty;

        //
        // Set section = 0 to empty value.
        //
        if ( this.SectionNo > 0 )
        {
          sectionNo = this.SectionNo.ToString ( "00" );
        }

        //
        // Set no = 0 to empty value.
        //
        if ( this.TestNo > 0 )
        {
          no = this.TestNo.ToString ( "00" );
        }

        return "\"" + sectionNo
          + "\",\"" + no
          + "\",\"" +  this.Action
          + "\",\"" + this.Parameters
          + "\",\"" + this.Description + "\"";
      }

      //===================================================================================
      /// <summary>
      /// This value checks that a parameter exists in the test action
      /// 
      /// </summary>
      /// <param name="Parameter">EuTestActionParameters enumerated value.</param>
      /// <returns>Bool:  True indicates the parameter exists.</returns>
      //-----------------------------------------------------------------------------------
      public bool createParameterList ( )
      {
        //
        // If option list exists then exit as it does not need to be created.
        //
        if ( this._optionList.Count > 0 )
        {
          return true;
        }

        //
        // If there are no parameters then exit with error as nothing can be created.
        //
        if ( this._Parameters == String.Empty )
        {
          return false;
        }

        //
        // initialise the methods variables and objects.
        //
        string [ ] arParameters = this._Parameters.Split ( ';' );
        EutCommandParameters enParameter = EutCommandParameters.Null;
        this.AddResponse = String.Empty;

        //
        // Iterate through the parameters array creating a parameter list.
        //
        foreach ( String parameter in arParameters )
        {
          //
          // exit is parmeter is empty.
          //
          if ( parameter == String.Empty )
          {
            return false;
          }


          //
          // exit is parmeter is empty.
          //
          if ( parameter.Contains ( "=" ) == false )
          {
            return false;
          }

          int eqIndex = parameter.IndexOf ( '=' );
          string parmeterName = parameter.Substring ( 0, eqIndex );
          String parmeterValue = parameter.Substring ( eqIndex + 1 );

          parmeterName = parmeterName.Trim ( );

          //
          // validate that the parameter is valid.
          //
          if ( EvStatics.tryParseEnumValue<EutCommandParameters> ( parmeterName, out enParameter ) == false )
          {
            this.Result = EutCommandResults.Parameter_Validation_Failure;
            this.AddResponse = "Parameter : " + parmeterName + " failed type validation. ";
            return false;
          }

          this._optionList.Add ( new EutParameter ( enParameter, parmeterValue.Trim ( ) ) );

        }//End parameter iteration loop.

        //
        // test to see if the value value needs to be reversed.
        //
        this.hasReverseStatus ( );

        return true;

      }//END createParameterList method

      //===================================================================================
      /// <summary>
      /// This value checks that a parameter exists in the test action
      /// 
      /// </summary>
      /// <param name="ParameterName">EuTestActionParameters enumerated value.</param>
      /// <returns>Bool:  True indicates the parameter exists.</returns>
      //-----------------------------------------------------------------------------------
      public void hasReverseStatus ( )
      {
        //
        // Initialise the methods variables and objects.
        //
        this._ReverseStatus = false;

        //
        // Iterate through the parmeter list looking for the parameter 
        //
        foreach ( EutParameter parameter in this._optionList )
        {
          //
          // Compare the parameter value 
          //
          if ( parameter.Name.ToString ( ).ToLower ( ) == EutCommandParameters.ReverseStatus.ToString ( ).ToLower ( ) )
          {
            this._ReverseStatus = true;
          }
        }

      }//END hasReverseStatus method

      //===================================================================================
      /// <summary>
      /// This value checks that a parameter exists in the test action
      /// 
      /// </summary>
      /// <param name="ParameterName">EuTestActionParameters enumerated value.</param>
      /// <returns>Bool:  True indicates the parameter exists.</returns>
      //-----------------------------------------------------------------------------------
      public bool hasParameterOptional (
        EutCommandParameters ParameterName )
      {
        //
        // create the parameter list if it does not exist.
        //
        if ( this.createParameterList ( ) == false )
        {
          return false;
        }

        //
        // create the list if it does not exist and return validatioen errors.
        //
        if ( this._optionList.Count == 0 )
        {
          return false;
        }

        //
        // Iterate through the parmeter list looking for the parameter 
        //
        foreach ( EutParameter parameter in this._optionList )
        {
          //
          // Compare the parameter value 
          //
          if ( parameter.Name.ToString ( ).ToLower ( ) == ParameterName.ToString ( ).ToLower ( ) )
          {
            return true;
          }
        }//END parameter iteration loop

        return false;

      }//END hasTestParameter value

      //===================================================================================
      /// <summary>
      /// This value checks that a parameter exists in the test action
      /// 
      /// </summary>
      /// <param name="ParameterName">EuTestActionParameters enumerated value.</param>
      /// <returns>Bool:  True indicates the parameter exists.</returns>
      //-----------------------------------------------------------------------------------
      public bool hasParameter (
        EutCommandParameters ParameterName )
      {
        //
        // create the parameter list if it does not exist.
        //
        if ( this.createParameterList ( ) == false )
        {
          return false;
        }

        //
        // create the list if it does not exist and return validatioen errors.
        //
        if ( this._optionList.Count == 0 )
        {
          this.Result = EutCommandResults.No_Parameters;
          this.AddResponse = "No action parameters ";
          return false;
        }

        //
        // Iterate through the parmeter list looking for the parameter 
        //
        foreach ( EutParameter parameter in this._optionList )
        {
          //
          // Compare the parameter value 
          //
          if ( parameter.Name == ParameterName )
          {
            return true;
          }
        }//END parameter iteration loop

        this.Result = EutCommandResults.Parameter_Missing;
        this.AddResponse = ParameterName + " is missing";

        return false;

      }//END hasTestParameter value

      //===================================================================================
      /// <summary>
      /// This value checks that a parameter exists in the test action
      /// 
      /// </summary>
      /// <param name="ParameterName">EuTestActionParameters enumerated value.</param>
      /// <returns>Bool:  True indicates the parameter exists.</returns>
      //-----------------------------------------------------------------------------------
      public String getParameter (
        EutCommandParameters ParameterName )
      {
        //
        // create the parameter list if it does not exist.
        //
        if ( this.createParameterList ( ) == false )
        {
          return String.Empty;
        }

        //
        // create the list if it does not exist and return validatioen errors.
        //
        if ( this._optionList.Count == 0 )
        {
          return String.Empty;
        }

        //
        // Iterate through the parmeter list looking for the parameter 
        //
        foreach ( EutParameter parameter in this._optionList )
        {
          //
          // Compare the parameter value 
          //
          if ( parameter.Name == ParameterName )
          {
            return parameter.Value.Trim ( );
          }
        }//END parameter iteration loop

        this.Result = EutCommandResults.Parameter_Missing;
        this.AddResponse = ParameterName + " missing.";

        return String.Empty;

      }//END hasTestParameter value

      //===================================================================================
      /// <summary>
      /// This value checks that a parameter exists in the test action
      /// 
      /// </summary>
      /// <param name="ParameterName">EuTestActionParameters enumerated value.</param>
      /// <returns>Bool:  True indicates the parameter exists.</returns>
      //-----------------------------------------------------------------------------------
      public void setParameter (
        EutCommandParameters ParameterName,
        String Value )
      {
        //
        // Initialise the methods variables and objects.
        //
        EutParameter parameter = new EutParameter ( );
        parameter.Name = ParameterName;
        parameter.Value = Value.Trim();

        //
        // if the option list is null initialise it
        //
        if ( this._optionList == null )
        {
          this._optionList = new List<EutParameter> ( );
        }

        //
        // if the list is empty add the parameter..
        //
        if ( this._optionList.Count == 0 )
        {
          this._optionList.Add ( parameter );
        }

        //
        // Iterate through the parmeter list looking for the parameter 
        //
        foreach ( EutParameter parm in this._optionList )
        {
          //
          // Compare the parameter value 
          //
          if ( parm.Name == ParameterName )
          {
            parm.Value = Value.Trim ( );
          }
        }//END parameter iteration loop

      }//END hasTestParameter value

      //===================================================================================
      /// <summary>
      /// This value set a class member value.
      /// </summary>
      //-----------------------------------------------------------------------------------
      public bool SetValue ( EuTestCaseMembers Member, String Value )
      {
        //
        // initialise the methods variables
        //
        int iValue = 0;
        EutCommand testType = EutCommand.Null;
        EutCommandResults result = EutCommandResults.Ok;

        switch ( Member )
        {
          case EuTestCaseMembers.Section:
            {
              if ( int.TryParse ( Value, out iValue ) == false )
              {
                return false;
              }
              this.SectionNo = iValue;
              return true;
            }
          case EuTestCaseMembers.No:
            {
              if ( int.TryParse ( Value, out iValue ) == false )
              {
                return false;
              }
              this.TestNo = iValue;
              return true;
            }
          case EuTestCaseMembers.Action:
            {
              if ( EvStatics.tryParseEnumValue<EutCommand> ( Value, out testType ) == false )
              {
                return false;
              }
              this.Action = testType;
              return true;
            }
          case EuTestCaseMembers.Parameters:
            {
              this.Parameters = Value;

              this.createParameterList ( );

              return true;
            }
          case EuTestCaseMembers.Description:
            {
              this.Description = Value;
              return true;
            }
          case EuTestCaseMembers.TestResponse:
            {
              this.AddResponse = Value;
              return true;
            }
          case EuTestCaseMembers.TestResult:
            {
              if ( EvStatics.tryParseEnumValue<EutCommandResults> ( Value, out result ) == false )
              {
                return false;
              }
              this.Result = result;
              return true;
            }
        }
        return false;
      }

      //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
      #endregion


    }//END EutTestAction class


    #region Logging  methods.

    const string CONST_NAME_SPACE = "Evado.UniForm.TestCaseRecorder.";

    private bool _LoggingOn = false;
    /// <summary>
    /// This property sets the debug state for the class.
    /// </summary>
    public bool LoggingOn
    {
      get { return _LoggingOn; }
      set { _LoggingOn = value; }
    }

    /// 
    /// Status stores the debug status information.
    /// 
    private StringBuilder _ClassLog = new StringBuilder ( );

    public String Log
    {
      get { return this._ClassLog.ToString ( ); }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="Value">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogInitMethod ( String Value )
    {
      this._ClassLog.AppendLine ( EvStatics.CONST_METHOD_START
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
    protected void LogInitiValue ( String DebugLogString )
    {
      this._ClassLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + DebugLogString );
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
      if ( _LoggingOn == true )
      {
        this._ClassLog.AppendLine ( EvStatics.CONST_METHOD_START
        + DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": "
        + CONST_NAME_SPACE + Value );
      }
    }
    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="MethodName">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    private void LogMethodEnd ( String MethodName )
    {
      if ( _LoggingOn == true )
      {
        String value = Evado.Model.EvStatics.CONST_METHOD_END;
        value = value.Replace ( " END OF METHOD ", " END OF " + MethodName + " METHOD " );
        this._ClassLog.AppendLine ( value );
      }
    }

    // ==================================================================================
    /// <summary>
    /// This method appendes the debuglog string to the debug log for the class and adds
    /// a new line at the end of the text.
    /// </summary>
    /// <param name="DebugLogString">String:  debug text.</param>
    // ----------------------------------------------------------------------------------
    protected void LogValue ( String DebugLogString )
    {
      if ( _LoggingOn == true )
      {
        this._ClassLog.AppendLine ( DateTime.Now.ToString ( "dd-MM-yy hh:mm:ss" ) + ": " + DebugLogString );
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END CommandHistory Class

}//END namespace