/***************************************************************************************
 * <copyright file="EvFormFieldValidationNotValid.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvFormFieldValidationNotValid data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace Evado.Model.Digital
{
  /// <summary>
  /// This class defines the form field validation rules.
  /// </summary>

  [Serializable]
  public class EvFormFieldValidationNotValid
  {

    #region Global variables and constants

    /// <summary>
    /// This constant defines a separator of a field string.
    /// </summary>
    private const string FieldSeparator = "~";

    /// <summary>
    /// This constant defines a separator of a field character. 
    /// </summary>
    private const char chFieldSeparator = '~';

    /// <summary>
    /// This constant defines a null rule string.  
    /// </summary>
    private const string nullRule = "~~";

    private List<String> _Rules = new List<String> ( );
    #endregion

    #region Property
    /// <summary>
    /// The property defines a list of string rules for the validation
    /// </summary>
    public List<String> Rules
    {
      get
      {
        return this._Rules;
      }
      set
      {
        this._Rules = value;
      }
    }

    /// <summary>
    /// The property get or sets a encoded list of validation rules.
    /// </summary>
    public String EncodedRules
    {
      get
      {
        String output = String.Empty;

        foreach ( string rule in this._Rules )
        {
          if ( output != String.Empty )
          {
            output += ";";
          }
          output = rule;
        }

        return output;
      }
      set
      {
        string stValues = value;
        string [ ] arrRules = stValues.Split ( ';' );
        this._Rules = new List<string> ( );

        for ( int i = 0; i < arrRules.Length; i++ )
        {
          this._Rules.Add ( arrRules [ i ] );
        }
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Public Methods.

    //  ==================================================================================
    /// <summary>
    /// This method sets a rule into the rule list.
    /// </summary>
    /// <param name="FieldName">string:The Java script field name</param>
    /// <param name="Value">string: The value as a string.</param>
    /// <param name="Option">string: The option text as a string.</param>
    /// <returns>boolean: true, if rule exists</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Iteration through the option list to identify options that exist.
    /// 
    /// 2. Split the rule array to identify the values
    ///  
    /// 3. If the content is less than 3 then the rule has not been defined.
    /// 
    /// 4. Test if the field and rule exist.    
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public bool hasRule ( string FieldName, string Value, string Option )
    {
      //
      // Iteration through the option list to identify options that exist.
      //
      foreach ( String rule in this._Rules )
      {
        //
        // Split the rule array to identify the values
        //
        string [ ] content = rule.Split ( chFieldSeparator );

        //
        // If the content is less than 3 then the rule has not been defined.
        //
        if ( content.Length < 3 )
        {
          return false;
        }

        //
        // Test if the field and rule exist.
        //
        if ( content [ 0 ] == FieldName.Trim ( ) )
        {
          if ( content [ 1 ] == Value.Trim ( ) )
          {
            if ( content [ 2 ].Contains ( Option.Trim ( ) ) == true )
            {
              return true;

            }//END option selection

          }//END value selection

        }//END field selection

      }//END interation loop

      return false;

    }//END hasRule method

    //  ==================================================================================
    /// <summary>
    /// This method set a rule into the rule list.
    /// </summary>
    /// <param name="FieldName">string: The Java script field name</param>
    /// <param name="Value">string: The value as a string.</param>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Call the setRule method.
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void setRule ( string FieldName, string Value )
    {
      setRule ( FieldName, Value, String.Empty );
    }//END setRule method.

    //  ==================================================================================
    /// <summary>
    /// This method set a rule into the rule list.
    /// </summary>
    /// <param name="FieldName">string: The Java script field name</param>
    /// <param name="Value">string: The value as a string.</param>
    /// <param name="Options">string: The option string</param>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Remove separators from the field name and value.
    /// 
    /// 2. Define the rule to be updated or added
    /// 
    /// 3. Iterate through the Rules array to update an existing rule
    /// and then exit.
    /// 
    /// 4. Add the rule to the list.
    /// 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public void setRule ( string FieldName, string Value, string Options )
    {
      //
      // remove separators from the field name and value.
      // 
      FieldName = FieldName.Replace ( FieldSeparator, String.Empty ).Trim ( );
      Value = Value.Replace ( FieldSeparator, String.Empty ).Trim ( );
      Options = Options.Replace ( FieldSeparator, String.Empty );

      //
      // Define the rule to be updated or added
      //
      string stRule = FieldName + FieldSeparator + Value + FieldSeparator + Options;

      // 
      // Iterate through the Rules array to update an existing rule
      // and then exit.
      // 
      for ( int i = 0; i < this._Rules.Count; i++ )
      {
        String rule = this._Rules [ i ];
        string [ ] arRule = rule.Split ( chFieldSeparator );

        if ( arRule [ 0 ] == FieldName
          && arRule [ 1 ] == Value )
        {
          rule = stRule;

          return;

        }//END rule match

      }//END rules iteration loop to update an existing rule.

      //
      // Add the rule to the list.
      //
      this._Rules.Add ( stRule );

    }//END setRule method 

    //  ==================================================================================
    /// <summary>
    /// This method retrieves the indexed rule.
    /// </summary>
    /// <param name="Index">integer: an index number</param>
    /// <returns >string array: an array of rule</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialise the string array.
    /// 
    /// 2. If the index is in range then retrieve the indexed value.
    /// 
    /// 3. Split the ruls in to field name and value.
    /// index 0: fieldname
    /// index 1: value
    /// 
    /// 4. If the array is too small then return empty string.
    /// 
    /// 5. Return the rule content. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public string [ ] getRule ( int Index )
    {
      //
      // Initialise the string array.
      //
      string [ ] asEmpty = new string [ 3 ];
      asEmpty [ 0 ] = String.Empty;
      asEmpty [ 1 ] = String.Empty;
      asEmpty [ 2 ] = String.Empty;

      // 
      // If the index is in range then retrieve the indexed value.
      // 
      if ( Index < 0
        || Index >= this.Rules.Count )
      {
        return asEmpty;
      }

      // 
      // Split the ruls in to field name and value.
      // index 0: fieldname
      // index 1: value.
      // 
      string [ ] content = this.Rules [ Index ].Split ( char.Parse ( FieldSeparator ) );

      // 
      // If the array is too small then return empty string.
      // 
      if ( content.Length < 3 )
      {
        return asEmpty;
      }

      // 
      // Return the value
      // 
      return content;

    }//END getRule method.


    //  ==================================================================================
    /// <summary>
    /// This method retrieves the indexed FieldName.
    /// </summary>
    /// <param name="Index">integer: an index value</param>
    /// <returns>string: a Field name</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. If the index is in range then retrieve the indexed value.
    /// 
    /// 2. Split the ruls in to field name and value.
    /// index 0: fieldname
    /// index 1: value
    /// 
    /// 3. If the array is too small then return empty string.
    /// 
    /// 4. Return the rule field name. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public string getFieldName ( int Index )
    {
      // 
      // If the index is in range then retrieve the indexed value.
      // 
      if ( Index < 0
        || Index >= this.Rules.Count )
      {
        return String.Empty;
      }

      // 
      // Split the ruls in to field name and value.
      // index 0: fieldname
      // index 1: value.
      // 
      string [ ] content = this.Rules [ Index ].Split ( char.Parse ( FieldSeparator ) );

      // 
      // If the array is too small then return empty string.
      // 
      if ( content.Length < 1 )
      {
        return String.Empty;
      }

      // 
      // Return the value
      // 
      return content [ 0 ];

    }//END getFieldName method.

    //  ==================================================================================
    /// <summary> 
    /// This method retrieves the indexed value.   
    /// </summary>
    /// <param name="Index">integer: an index value</param>
    /// <returns>string: a rule value</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. If the index is in range then retrieve the indexed value.
    /// 
    /// 2. Split the ruls in to field name and value.
    /// index 0: fieldname
    /// index 1: value
    /// 
    /// 3. If the array is too small then return empty string.
    /// 
    /// 4. Return the rule value. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public string getValue ( int Index )
    {
      // 
      // If the index is in range then retrieve the indexed value.
      // 
      if ( Index < 0
        || Index >= this.Rules.Count )
      {
        return String.Empty;
      }

      // 
      // Split the ruls in to field name and value.
      // index 0: fieldname
      // index 1: value.
      // 
      string [ ] content = this._Rules [ Index ].Split ( char.Parse ( FieldSeparator ) );

      // 
      // If the array is too small then return empty string.
      // 
      if ( content.Length < 2 )
      {
        return String.Empty;
      }

      // 
      // Return the value
      // 
      return content [ 1 ];

    }//END getValue method.

    //  ==================================================================================
    /// <summary>
    /// This method retrieves the indexed value.
    /// </summary>
    /// <param name="Index">integer: an index value</param>
    /// <returns >string: an option string.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. If the index is in range then retrieve the indexed value.
    /// 
    /// 2. Split the ruls in to field name and value.
    /// index 0: fieldname
    /// index 1: value
    /// 
    /// 3. If the array is too small then return empty string.
    /// 
    /// 4. Return the rule value for using as an option string. 
    /// </remarks>
    //  ---------------------------------------------------------------------------------
    public string getOptions ( int Index )
    {
      // 
      // If the index is in range then retrieve the indexed value.
      // 
      if ( Index < 0
        || Index >= this.Rules.Count )
      {
        return String.Empty;
      }

      // 
      // Split the ruls in to field name and value.
      // index 0: fieldname
      // index 1: value.
      // 
      string [ ] content = this._Rules [ Index ].Split ( char.Parse ( FieldSeparator ) );

      // 
      // If the array is too small then return empty string.
      // 
      if ( content.Length < 3 )
      {
        return String.Empty;
      }

      // 
      // Return the value
      // 
      return content [ 2 ];

    }//END getValue method.

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END EvFormFieldValidationNotValid class

}//END namespace Evado.Model.Digital
