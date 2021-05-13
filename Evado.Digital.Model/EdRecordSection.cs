/***************************************************************************************
 * <copyright file="EvFormSection.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the EvFormSection data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Digital.Model
{

  /// <summary>
  /// Business entity used to model accounts
  /// </summary>
  [Serializable]
  public class EdRecordSection
  {
    #region Class initialisation

    /// <summary>
    /// This method initialises the class.
    /// </summary>
    public EdRecordSection ( )
    {
    }

    /// <summary>
    /// This method initialises the class with parameters.
    /// </summary>
    /// <param name="No"> int object number</param>
    /// <param name="Order"> int section order.</param>
    /// <param name="Title"> String section title.</param>
    public EdRecordSection ( int No, int Order, String Title )
    {
      this._No = No;
      this._Order = Order;
      this._Title = Title;
    }



    #endregion

    #region enumerations

    /// <summary>
    /// This enumeration list defines the form class field names enumerated identifiers.
    /// </summary>
    public enum FormSectionClassFieldNames
    {
      /// <summary>
      /// This enumeration is the null value
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration identifies the section number field.
      /// </summary>
      Sectn_No,

      /// <summary>
      /// This enumeration identifies the section order field.
      /// </summary>
      Sectn_Order,

      /// <summary>
      /// This enumeration identifies the section title field
      /// </summary>
      Sectn_Title,

      /// <summary>
      /// This enumeration identified the section instruction field.
      /// </summary>
      Sectn_Instructions,

      /// <summary>
      /// This enumeration identifies the display field identifier field.
      /// </summary>
      Sectn_Field_Id,

      /// <summary>
      /// This enumeration identifies the field value to hide or display section field.
      /// </summary>
      Sectn_Field_Value,

      /// <summary>
      /// This enumeration identifies on match field field.
      /// </summary>
      Sectn_On_Match_Visible,

      /// <summary>
      /// This enumeration identifies the on open visible field.
      /// </summary>
      Sectn_On_Open_Visible,

      /// <summary>
      /// This enumeration identifies the section display roles field.
      /// </summary>
      Sectn_Display_Roles,

      /// <summary>
      /// This enumeration identifies the section edit roles field.
      /// </summary>
      Sectn_Edit_Roles,

      /// <summary>
      /// This enumeration identifies the section percentage width.
      /// </summary>
      Sectn_PercentWidth,
    }
    #endregion

    #region Class property

    private Guid _LayoutGuid = Guid.Empty;
    /// <summary>
    /// This property contains a global unique identifier of a form. 
    /// </summary>
    public Guid LayoutGuid
    {
      get
      {
        return this._LayoutGuid;
      }
      set
      {
        this._LayoutGuid = value;
      }
    }

    private int _No = -1;
    /// <summary>
    /// This property contains the number of a form section
    /// </summary>
    public int No
    {
      get { return this._No; }
      set { this._No = value; }
    }

    private int _Order = 0;
    /// <summary>
    /// This property contains the Order of a form section
    /// </summary>
    public int Order
    {
      get { return this._Order; }
      set { this._Order = value; }
    }

    private String _Title = String.Empty;
    /// <summary>
    /// This property contains a section of a form
    /// </summary>
    public string Title
    {
      get { return this._Title; }
      set { this._Title = value.Trim ( ); }
    }

    /// <summary>
    /// This property contains a section of a form
    /// </summary>
    public string Section
    {
      get { return this._Title; }
      set { this._Title = value.Trim ( ); }
    }

    private int _PercentWidth = 100;
    /// <summary>
    /// This property contains the percent width of the section (page group).
    /// 100 indicated full width.
    /// </summary>
    public int PercentWidth
    {
      get { return this._PercentWidth; }
      set
      {
        this._PercentWidth = value;

        if ( this._PercentWidth < 0 )
        {
          this._PercentWidth = 0;
        }
        if ( this._PercentWidth > 100 )
        {
          this._PercentWidth = 100;
        }


      }
    }

    /// <summary>
    /// This property returns the command link text for the section
    /// </summary>
    public String LinkText
    {
      get
      {
        String linkText = this._No
          + Evado.Digital.Model.EdLabels.Space_Hypen + this._Title
          + ", Order: " + this._Order;

        return linkText;
      }
    }

    private String _Instructions = String.Empty;
    /// <summary>
    /// This property contains the instructions of a form section
    /// </summary>
    public string Instructions
    {
      get { return this._Instructions; }
      set
      {
        this._Instructions = value.Trim ( );

        this._Instructions = this._Instructions.Replace ( "\n", "~" );
        this._Instructions = this._Instructions.Replace ( "\r", "~" );
        this._Instructions = this._Instructions.Replace ( "~~", "~" );
        this._Instructions = this._Instructions.Replace ( "~", " \r\n" );
        this._Instructions = this._Instructions.Replace ( "[b]", "__" );
        this._Instructions = this._Instructions.Replace ( "[/b]", "__" );
        this._Instructions = this._Instructions.Replace ( "[i]", "_" );
        this._Instructions = this._Instructions.Replace ( "[/i]", "_" );
      }
    }

    private String _FieldId = String.Empty;
    /// <summary>
    /// This property contains a field name of a form section
    /// </summary>
    public string FieldId
    {
      get { return this._FieldId; }
      set { this._FieldId = value.Trim ( ); }
    }

    private String _FieldValue = String.Empty;
    /// <summary>
    /// This property contains a field value of a form section
    /// </summary>
    public string FieldValue
    {
      get { return this._FieldValue; }
      set { this._FieldValue = value.Trim ( ); }
    }

    private bool _OnMatchVisible = true;
    /// <summary>
    /// This property indicates whether the form section is on match visible
    /// </summary>
    public bool OnMatchVisible
    {
      get { return this._OnMatchVisible; }
      set { this._OnMatchVisible = value; }
    }

    private bool _OnOpenVisible = true;
    /// <summary>
    /// This property indicates whether the form section is visible
    /// </summary>
    public bool OnOpenVisible
    {
      get { return this._OnOpenVisible; }
      set { this._OnOpenVisible = value; }
    }

    private String _ReadAccessRoles = String.Empty;
    /// <summary>
    /// This property contains default display roles of a form section
    /// Empty indicated all users have access.
    /// </summary>
    public string ReadAccessRoles
    {
      get { return this._ReadAccessRoles; }
      set
      {
        this._ReadAccessRoles = value.Trim ( );

        this.updateReadAccessRole ( );
      }
    }

    private String _EditAccessRoles = String.Empty;
    /// <summary>
    /// This property contains default display roles of a form section
    /// </summary>
    public string EditAccessRoles
    {
      get { return this._EditAccessRoles; }
      set
      {
        this._EditAccessRoles = value.Trim ( );

        this.updateReadAccessRole ( );
      }
    }

    #endregion

    #region Public methods

    // =====================================================================================
    /// <summary>
    /// This method test to see if the user has a role contain in the roles delimited list.
    /// Empty read access roles indicates that all users have access.
    /// </summary>
    /// <param name="Roles">';' delimted string of roles</param>
    /// <returns>True: if the role exists.</returns>
    // -------------------------------------------------------------------------------------
    public bool hasReadAccess ( String Roles )
    {
      if ( Roles == null )
      {
        return false;
      }

      //
      // no defined read access roles indicated all users have access.
      //
      if ( this._ReadAccessRoles == String.Empty )
      {
        return true;
      }

      //
      // if roles are defined and an empty string is passed return false access.
      //
      if ( Roles == String.Empty
        && this._ReadAccessRoles != String.Empty )
      {
        return false;
      }

      foreach ( String role in Roles.Split ( ';' ) )
      {
        foreach ( String role1 in this._ReadAccessRoles.Split ( ';' ) )
        {
          if ( role1.ToLower ( ) == role.ToLower ( ) )
          {
            return true;
          }
        }
      }
      return false;
    }//END method

    // =====================================================================================
    /// <summary>
    /// This method test to see if the user has a role contain in the roles delimited list.
    /// Empty read access roles indicates that all users have edit access.
    /// </summary>
    /// <param name="Roles">';' delimted string of roles</param>
    /// <returns>True: if the role exists.</returns>
    // -------------------------------------------------------------------------------------
    public bool hasRoleEdit ( String Roles )
    {
      if ( Roles == null )
      {
        return false;
      }

      //
      // no defined read access roles indicated all users have access.
      //
      if ( this._EditAccessRoles == String.Empty )
      {
        return true;
      }

      //
      // if roles are defined and an empty string is passed return false access.
      //
      if ( Roles == String.Empty
        && this._EditAccessRoles != String.Empty )
      {
        return false;
      }

      //
      // iterate through both role lists looking for a match.
      //
      foreach ( String role in Roles.Split ( ';' ) )
      {
        foreach ( String role1 in this._EditAccessRoles.Split ( ';' ) )
        {
          if ( role1.ToLower ( ) == role.ToLower ( ) )
          {
            return true;
          }
        }
      }
      return false;
    }//END method

    // ===================================================================================
    /// <summary>
    /// This method updates the read access roles to ensure that all 
    /// edit roles are included.
    /// </summary>
    // ----------------------------------------------------------------------------------
    private void updateReadAccessRole ( )
    {
      //
      // exit if the edit roles are not defined.
      //
      if ( this._EditAccessRoles == String.Empty
        || this._ReadAccessRoles == String.Empty )
      {
        return;
      }

      //
      // create an array of edit roles.
      //
      string [ ] arrEditRole = this._EditAccessRoles.Split ( ';' );

      //
      // Iterate through the edit roles add those that are not in the read access roles.
      //
      for ( int i = 0; i < arrEditRole.Length; i++ )
      {
        if ( this._ReadAccessRoles.Contains ( arrEditRole [ i ] ) == false )
        {
          if ( i > 0 )
          {
            this._ReadAccessRoles += ";";
          }
          this._ReadAccessRoles += arrEditRole [ i ];
        }
      }

    }//END Method.

    // =====================================================================================
    /// <summary>
    ///   This method sets the field value.
    /// </summary>
    /// <param name="FieldName">string: a field name</param>
    /// <param name="Value">string: a value of field name</param>  
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Initialize an internal variables and an enumerated object fieldname
    /// 
    /// 2. Try convert a string FieldName into an enumerated object fieldname
    /// 
    /// 3. Switch fieldname and update value for the property defined by form class field names.
    /// </remarks>
    //  ------------------------------------------------------------------------------------
    public void setValue ( FormSectionClassFieldNames FieldName, String Value )
    {
      // 
      // Switch to determine which value to return.
      // 
      switch ( FieldName )
      {
        case FormSectionClassFieldNames.Sectn_No:
          {
            this._No =  Evado.Model.EvStatics.getInteger ( Value );
            return;
          }
        case FormSectionClassFieldNames.Sectn_Order:
          {
            this._Order =  Evado.Model.EvStatics.getInteger ( Value );
            return;
          }
        case FormSectionClassFieldNames.Sectn_Title:
          {
            this._Title = Value;
            return;
          }
        case FormSectionClassFieldNames.Sectn_Instructions:
          {
            this._Instructions = Value;
            return;
          }
        case FormSectionClassFieldNames.Sectn_Field_Id:
          {
            this._FieldId = Value;
            return;
          }
        case FormSectionClassFieldNames.Sectn_Field_Value:
          {
            this._FieldValue = Value;
            return;
          }
        case FormSectionClassFieldNames.Sectn_On_Match_Visible:
          {
            this._OnMatchVisible =  Evado.Model.EvStatics.getBool ( Value );
            return;
          }
        case FormSectionClassFieldNames.Sectn_On_Open_Visible:
          {
            this._OnOpenVisible =  Evado.Model.EvStatics.getBool ( Value );
            return;
          }

        case FormSectionClassFieldNames.Sectn_Display_Roles:
          {
            this._ReadAccessRoles = Value;
            return;
          }

        case FormSectionClassFieldNames.Sectn_Edit_Roles:
          {
            this._EditAccessRoles = Value;
            return;
          }

        case FormSectionClassFieldNames.Sectn_PercentWidth:
          {
            this.PercentWidth =  Evado.Model.EvStatics.getInteger ( Value );
            return;
          }

        default:

          return;

      }//END Switch

    }//END setValue method
    #endregion

  }//END EvFormSection class

} // Close namespace Evado.Digital.Model
