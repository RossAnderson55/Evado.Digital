/***************************************************************************************
 * <copyright file="EvFormSection.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvFormSection data object.
 *
 ****************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
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
    }
    #endregion

    #region Class property

    private Guid _Guid = Guid.Empty;
    /// <summary>
    /// This property contains a global unique identifier of a section. 
    /// </summary>
    public Guid Guid
    {
      get
      {
        return this._Guid;
      }
      set
      {
        this._Guid = value;
      }
    }

    private Guid _FormGuid = Guid.Empty;
    /// <summary>
    /// This property contains a global unique identifier of a form. 
    /// </summary>
    public Guid FormGuid
    {
      get
      {
        return this._FormGuid;
      }
      set
      {
        this._FormGuid = value;
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
      set { this._Title = value.Trim(); }
    }
    /// <summary>
    /// This property contains a section of a form
    /// </summary>
    public string Section
    {
      get { return this._Title; }
      set { this._Title = value.Trim ( ); }
    }

    /// <summary>
    /// This property returns the command link text for the section
    /// </summary>
    public String LinkText
    {
      get {
        String linkText = this._No
          + Evado.Model.Digital.EdLabels.Space_Hypen + this._Title
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
      set { this._FieldId = value.Trim(); }
    }

    private String _FieldValue = String.Empty;
    /// <summary>
    /// This property contains a field value of a form section
    /// </summary>
    public string FieldValue
    {
      get { return this._FieldValue; }
      set { this._FieldValue = value.Trim(); }
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

    private String _DisplayRoles = String.Empty;
    /// <summary>
    /// This property contains default display roles of a form section
    /// </summary>
    public string UserDisplayRoles
    {
      get { return this._DisplayRoles; }
      set { this._DisplayRoles = value.Trim(); }
    }

    private String _EditRoles = String.Empty;
    /// <summary>
    /// This property contains default display roles of a form section
    /// </summary>
    public string UserEditRoles
    {
      get { return this._EditRoles; }
      set { this._EditRoles = value.Trim ( ); }
    }

    #endregion

    #region Public methods

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
            this._No = EvStatics.getInteger( Value );
            return;
          }
        case FormSectionClassFieldNames.Sectn_Order:
          {
            this._Order = EvStatics.getInteger ( Value );
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
            this._OnMatchVisible = EvStatics.getBool( Value );
            return;
          }
        case FormSectionClassFieldNames.Sectn_On_Open_Visible:
          {
            this._OnOpenVisible = EvStatics.getBool ( Value );
            return;
          }

        case FormSectionClassFieldNames.Sectn_Display_Roles:
          {
              this._DisplayRoles = Value;
            return;
          }

        case FormSectionClassFieldNames.Sectn_Edit_Roles:
          {
            this._EditRoles = Value;
            return;
          }

        default:

          return;

      }//END Switch

    }//END setValue method


    /// <summary>
    /// This method adds a default display role to the section.
    /// </summary>
    /// <param name="Role">String: a role string</param>
    public void addRole ( EdRecord.FormAccessRoles Role )
    {
      string role = Role.ToString ( );

      if ( this._DisplayRoles.Contains ( role ) == false )
      {
        if ( this._DisplayRoles != String.Empty )
        {
        this._DisplayRoles += ";";
        }
        this._DisplayRoles += role;
      }
    }
    /// <summary>
    /// This method deletes a default display role to the section.
    /// </summary>
    /// <param name="Role">String: a role string</param>
    public void deleteRole ( EdRecord.FormAccessRoles Role )
    {
      string role = Role.ToString ( );

      if ( this._DisplayRoles.Contains ( role ) == false )
      {
        return;
      }

      this._DisplayRoles.Replace ( role, String.Empty );
      this._DisplayRoles.Replace ( ";;", ";" );
    }

    /// <summary>
    /// This method returns True of the role exists, False if the role does not exist.
    /// </summary>
    /// <param name="Role">EvRoleList: a role enumeration</param>
    /// <returns></returns>
    public bool hasRole ( EdRecord.FormAccessRoles Role )
    {
      if ( this._DisplayRoles == String.Empty )
      {
        return true;
      }

      if (this._DisplayRoles.Contains(Role.ToString()) == true)
      {
        return true;
      }
      return false;
    }
    #endregion

  }//END EvFormSection class

} // Close namespace Evado.Model.Digital
