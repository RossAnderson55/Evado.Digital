/***************************************************************************************
 * <copyright file="EvFormContent.cs" company="EVADO HOLDING PTY. LTD.">
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
 * Content: 
 *  This class contains the EvFormContent data object.
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
  public class EdFormRecordComment
  {

    #region Public Methods
    /// <summary>
    /// The class initiation.
    /// </summary>
    public EdFormRecordComment ( )
    {
    }
    /// <summary>
    /// The class initiation.
    /// </summary>
    public EdFormRecordComment (
      Guid RecordGuid,
      AuthorTypeCodes AuthorType,
      String UserId,
      String UserCommonName,
      String Content )
    {
      this._RecordGuid = RecordGuid;
      this._AuthorType = AuthorType;
      this._UserId = UserId;
      this._UserCommonName = UserCommonName;
      this._Content = Content;
      this._CommentDate = DateTime.Now;

      //
      // IF the record guid is not empty then set the comment type to record field.
      //
      if ( this._RecordGuid != Guid.Empty )
      {
        _CommentType = CommentTypeCodes.Form;
      }

    }

    /// <summary>
    /// The class initiation.
    /// </summary>
    public EdFormRecordComment (
      Guid RecordGuid,
      AuthorTypeCodes AuthorType,
      String UserId,
      String UserCommonName,
      String Content,
      String Date )
    {
      this._RecordGuid = RecordGuid;
      this._AuthorType = AuthorType;
      this._UserId = UserId;
      this._UserCommonName = UserCommonName;
      this._Content = Content;
      if ( Date.Length > 0 )
      {
        this._CommentDate = DateTime.Parse ( Date );
      }

      //
      // IF the record guid is not empty then set the comment type to record field.
      //
      if ( this._RecordGuid != Guid.Empty )
      {
        _CommentType = CommentTypeCodes.Form;
      }
    }

    /// <summary>
    /// The class initiation.
    /// </summary>
    public EdFormRecordComment (
      Guid RecordGuid,
      Guid RecordFieldGuid,
      AuthorTypeCodes AuthorType,
      String UserId,
      String UserCommonName,
      String Content )
    {
      this._RecordGuid = RecordGuid;
      this._RecordFieldGuid = RecordFieldGuid;
      this._AuthorType = AuthorType;
      this._UserId = UserId;
      this._UserCommonName = UserCommonName;
      this._Content = Content;
      this._CommentDate = DateTime.Now;

      //
      // IF the record guid is not empty then set the comment type to record field.
      //
      if ( this._RecordGuid != Guid.Empty )
      {
        _CommentType = CommentTypeCodes.Form;
      }

      //
      // IF the record field guid is not empty then set the comment type to record field.
      //
      if ( this._RecordFieldGuid != Guid.Empty )
      {
        _CommentType = CommentTypeCodes.Form_Field;
      }
    }

    /// <summary>
    /// The class initiation.
    /// </summary>
    public EdFormRecordComment (
      Guid RecordGuid,
      Guid RecordFieldGuid,
      AuthorTypeCodes AuthorType,
      String UserId,
      String UserCommonName,
      String Content,
      String Date )
    {
      this._RecordGuid = RecordGuid;
      this._RecordFieldGuid = RecordFieldGuid;
      this._AuthorType = AuthorType;
      this._UserId = UserId;
      this._UserCommonName = UserCommonName;
      this._Content = Content;
      if ( Date.Length > 0 )
      {
        this._CommentDate = DateTime.Parse ( Date );
      }

      //
      // IF the record guid is not empty then set the comment type to record field.
      //
      if ( this._RecordGuid != Guid.Empty )
      {
        _CommentType = CommentTypeCodes.Form;
      }

      //
      // IF the record field guid is not empty then set the comment type to record field.
      //
      if ( this._RecordFieldGuid != Guid.Empty )
      {
        _CommentType = CommentTypeCodes.Form_Field;
      }
    }
    #endregion

    #region Public variables and constant
    /// <summary>
    /// This constant define the by phase
    /// </summary>
    public const String ByText = "by";
    /// <summary>
    /// this constant defines the by phrase
    /// </summary>
    public const String OnText = "on";

    /// <summary>
    /// This constant defines the space character of the record comment. 
    /// </summary>
    private const String space = " ";
    #endregion

    #region Enumerators
    /// <summary>
    /// This enumeration defines the comment types for the application.
    /// </summary>
    public enum CommentTypeCodes
    {
      /// <summary>
      /// This enueration defines the default comment type.
      /// </summary>
      Not_Set = 0,

      /// <summary>
      /// This enumeration defines form level comments
      /// </summary>
      Form = 1,

      /// <summary>
      /// This enumeration defines the field level comments.
      /// </summary>
      Form_Field = 2,

      /// <summary>
      /// This enumeration defines form level comments
      /// </summary>
      Visit = 3,

      /// <summary>
      /// This enumeration defines form level comments
      /// </summary>
      Subject = 4,
    }

    /// <summary>
    /// This enumeration list defines the comment author types.
    /// </summary>
    public enum AuthorTypeCodes
    {
      /// <summary>
      /// This enumeration is the default and indicates the enumeration is not set.
      /// </summary>
      Not_Set = 0,

      /// <summary>
      /// Thie enumeration defines the comment authoer type as a record author.
      /// </summary>
      Record_Author = 1,

      /// <summary>
      /// This enumeration defines the comment author type as a monitor.
      /// </summary>
      Monitor = 2,

      /// <summary>
      /// This enumeraiton defines the comment author type a data manager
      /// </summary>
      Data_Manager = 3,

      /// <summary>
      /// This enumeraiton defines the comment author type a record reviewer
      /// </summary>
      Reviewer = 4,
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Public members

    private Guid _RecordGuid = Guid.Empty;
    /// <summary>
    /// This property contains the form record  Guid reference this 
    /// comment relates to.
    /// </summary>
    public Guid RecordGuid
    {
      get
      {
        return this._RecordGuid;
      }
      set
      {
        this._RecordGuid = value;

        //
        // IF the record guid is not empty then set the comment type to record field.
        //
        if ( this._RecordGuid != Guid.Empty )
        {
          _CommentType = CommentTypeCodes.Form;
        }
      }
    }

    private Guid _RecordFieldGuid = Guid.Empty;
    /// <summary>
    /// This property contains the forem record field Guid reference this 
    /// comment relates to.
    /// Value = Guid.Empty means that the comment is a form level comment.
    /// </summary>
    public Guid RecordFieldGuid
    {
      get
      {
        return this._RecordFieldGuid;
      }
      set
      {
        this._RecordFieldGuid = value;

        //
        // IF the record field guid is not empty then set the comment type to record field.
        //
        if ( this._RecordFieldGuid != Guid.Empty )
        {
          _CommentType = CommentTypeCodes.Form_Field;
        }
      }
    }

    CommentTypeCodes _CommentType = CommentTypeCodes.Not_Set;
    /// <summary>
    /// This property contains the comment type enumeration value.
    /// </summary>
    public CommentTypeCodes CommentType
    {
      get
      {
        return _CommentType;
      }
      set
      {
        this._CommentType = value;
      }
    }

    private AuthorTypeCodes _AuthorType = AuthorTypeCodes.Not_Set;
    /// <summary>
    /// This property contains the comment author type eumerated value.
    /// </summary>
    public AuthorTypeCodes AuthorType
    {
      get
      {
        return this._AuthorType;
      }
      set
      {
        this._AuthorType = value;
      }
    }

    private String _UserId = String.Empty;
    /// <summary>
    /// This property contains the authors user id
    /// </summary>
    public String UserId
    {
      get
      {
        return this._UserId;
      }
      set
      {
        this._UserId = value;
      }
    }

    private String _UserCommonName = String.Empty;
    /// <summary>
    /// This property contains the authoers common name
    /// </summary>
    public String UserCommonName
    {
      get
      {
        return this._UserCommonName;
      }
      set
      {
        this._UserCommonName = value;
      }
    }

    private String _Content = String.Empty;
    /// <summary>
    /// This property contains the comment content
    /// </summary>
    public String Content
    {
      get
      {
        return this._Content;
      }
      set
      {
        this._Content = value;
      }
    }

    private DateTime _CommentDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains the date the comment was added to the form record or form record field.
    /// </summary>
    public DateTime CommentDate
    {
      get
      {
        if ( this._UserCommonName == String.Empty )
        {
          this._CommentDate = EvcStatics.CONST_DATE_NULL;
        }
        return this._CommentDate;
      }
      set
      {
        this._CommentDate = value;
      }
    }

    /// <summary>
    /// This prioperty contains the comment date as a string expression.
    /// An empty string is returned if the date is null date.
    /// </summary>
    public String stCommentDate
    {
      get
      {
        if ( this._UserCommonName == String.Empty )
        {
          this._CommentDate = EvcStatics.CONST_DATE_NULL;
        }
        if ( this._CommentDate > EvcStatics.CONST_DATE_NULL )
        {
          return this._CommentDate.ToString ( "dd MMM yyyy" );
        }
        return String.Empty;
      }

      set
      {
        String dValue = value;
      }
    }

    private bool _NewComment = false;
    /// <summary>
    /// This property is a flag indicating a new comment to be added to the databases.
    /// </summary>
    public bool NewComment
    {
      get
      {
        return this._NewComment;
      }
      set
      {
        this._NewComment = value;
      }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region public static methods
    // =====================================================================================
    /// <summary>
    ///  This method outputs the contents of the text string.
    /// </summary>
    /// <param name="CommentList">List of EvFormRecordComment objects: a comment list</param>
    /// <param name="withHeader">Bool: True = include comment header.</param>
    /// <returns>String: a Html text string.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Validate whether the commentlist is null and empty
    /// 
    /// 2. Initialise the local variables.
    /// 
    /// 3. Iterate through the current adding items to the new list.
    /// 
    /// 4. Process the item if exists.
    /// 
    /// 5. Initialize the comment object with a commentList array
    /// 
    /// 6. If userCommonName exists, Append content, user common name and comment date to sbHtml 
    /// 
    /// 7. Return the new commentHtml array.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static String getCommentHtml (
      List<EdFormRecordComment> CommentList, bool withHeader )
    {
      //
      // Validate whether the commentlist is null and empty
      //
      if ( CommentList == null )
      {
        return Evado.Digital.Model.EdLabels.Label_No_Comments;
      }
      if ( CommentList.Count == 0 )
      {
        return Evado.Digital.Model.EdLabels.Label_No_Comments;
      }

      // 
      // Initialise the local variables.
      // 
      System.Text.StringBuilder sbHtml = new System.Text.StringBuilder ( );

      if ( withHeader == true )
      {
        sbHtml.AppendLine ( "<strong>" + Evado.Digital.Model.EdLabels.Label_Comments_Log + "</strong><br/>" );
      }
      
      // 
      // Iterate through the current adding items to the new list.
      // 
      foreach ( EdFormRecordComment comment in CommentList )
      {
        // 
        // Skip all null items
        // 
        if ( comment == null )
        {
          continue;
        }

        sbHtml.AppendLine ( comment.Content.Replace ( "\n", "<br/>" ) + "<br/>" );
        //
        // If userCommonName exists, Append content, user common name and comment date to sbHtml 
        //
        if ( comment.UserCommonName != String.Empty )
        {
           sbHtml.AppendLine ( ByText + space + comment.UserCommonName
            + space + OnText + space + comment.CommentDate.ToString ( "dd MMM yyyy hh:mm" )
            + "<br/><br/>" );
        }


      }//END annotation list iteration loop

      // 
      // Return the new annotation array.
      // 
      return sbHtml.ToString ( );

    }//END getCommentHtml method

    // =====================================================================================
    /// <summary>
    ///  This method outputs the contents of the text string.
    /// </summary>
    /// <param name="CommentList">List of EvFormRecordComment objects: a comment list</param>
    /// <param name="withHeader">Bool: True = include comment header.</param>
    /// <returns>String: a Html text string.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Validate whether the commentlist is null and empty
    /// 
    /// 2. Initialise the local variables.
    /// 
    /// 3. Iterate through the current adding items to the new list.
    /// 
    /// 4. Process the item if exists.
    /// 
    /// 5. Initialize the comment object with a commentList array
    /// 
    /// 6. If userCommonName exists, Append content, user common name and comment date to sbHtml 
    /// 
    /// 7. Return the new commentHtml array.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static String getCommentMD ( 
      List<EdFormRecordComment> CommentList, bool withHeader )
    {
      //
      // Validate whether the commentlist is null and empty
      //
      if ( CommentList == null )
      {
        return Evado.Digital.Model.EdLabels.Label_No_Comments;
      }
      if ( CommentList.Count == 0 )
      {
        return Evado.Digital.Model.EdLabels.Label_No_Comments;
      }

      // 
      // Initialise the local variables.
      // 
      System.Text.StringBuilder sbMarkDown = new System.Text.StringBuilder ( );

      if ( withHeader == true )
      {
        sbMarkDown.Append ( "__" + Evado.Digital.Model.EdLabels.Label_Comments_Log + "__" );
      }

      // 
      // Iterate through the current adding items to the new list.
      // 
      foreach ( EdFormRecordComment comment in CommentList )
      {
        // 
        // Skip all null items
        // 
        if ( comment == null )
        {
          continue;
        }

        // 
        // Process the item if exists.
        // 
        sbMarkDown.AppendLine ( comment.Content );

        //
        // If userCommonName exists, Append content, user common name and comment date to sbHtml 
        //
        if ( comment.UserCommonName != String.Empty )
        {
          sbMarkDown.AppendLine ( ByText + space + comment.UserCommonName
            + space + OnText + space + comment.CommentDate.ToString ( "dd MMM yyyy hh:mm" ) );
        }

      }//END annotation list iteration loop

      // 
      // Return the new annotation array.
      // 
      return sbMarkDown.ToString ( );

    }//END getCommentMD method

    // =====================================================================================
    /// <summary>
    /// Content:
    ///  This method outputs the contents of the comments as a text string.
    /// </summary>
    /// <param name="CommentList">List of EvFormRecordComment objects: a comment list</param>
    /// <returns>String: a Html text string.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Validate whether the commentlist is null and empty
    /// 
    /// 2. Initialise the local variables.
    /// 
    /// 3. Iterate through the current adding items to the new list.
    /// 
    /// 4. Process the item if exists.
    /// 
    /// 5. Set the comment display.
    /// 
    /// 6. If userCommonName exists, Append content, user common name and comment date to sbHtml 
    /// 
    /// 7. Set the comment display.
    /// 
    /// 8. Return the new commentHtml array.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static String getFieldAnnotationHtml (
      List<EdFormRecordComment> CommentList )
    {
      //
      // Validate whether the commentlist is null and empty
      //
      if ( CommentList == null )
      {
        return String.Empty;
      }
      if ( CommentList.Count == 0 )
      {
        return String.Empty;
      }

      // 
      // Initialise the local variables.
      // 
      System.Text.StringBuilder sbHtml = new System.Text.StringBuilder ( );
      
      // 
      // Iterate through the current adding items to the new list.
      // 
      foreach ( EdFormRecordComment comment in CommentList )
      {
        // 
        // Skip all null items
        // 
        if ( comment == null )
        {
          continue;
        }

        //
        // Set the comment display.
        //
        if ( comment.AuthorType == AuthorTypeCodes.Monitor
          || comment.AuthorType == AuthorTypeCodes.Data_Manager )
        {
          sbHtml.AppendLine ( "<strong>" + comment.Content.Replace ( "\n", "<br/>" ) + "</strong> " );
          //
          // If userCommonName exists, Append content, user common name and comment date to sbHtml 
          //
          if ( comment.UserCommonName != String.Empty )
          {
            sbHtml.AppendLine ( ByText + space + comment.UserCommonName
              + space + OnText + space + comment.CommentDate.ToString ( "dd MMM yyyy hh:mm" ) );
          }
        }
        else
        {
          sbHtml.AppendLine ( comment.Content.Replace ( "\n", "<br/>" ) );
          //
          // If userCommonName exists, Append content, user common name and comment date to sbHtml 
          //
          if ( comment.UserCommonName != String.Empty )
          {
            sbHtml.AppendLine ( ByText + space + comment.UserCommonName
              + space + OnText + space + comment.CommentDate.ToString ( "dd MMM yyyy hh:mm" ) );
          }
        }

        sbHtml.AppendLine ( "<br/>" );

      }//END annotation list iteration loop

      // 
      // Close the table tag
      // 
      sbHtml.AppendLine ( "<br/>" );

      // 
      // Return the new annotation array.
      // 
      return sbHtml.ToString ( );

    }//END getFieldAnnotationHtml method

    // =====================================================================================
    /// <summary>
    /// Content:
    ///  This method outputs the contents of the comments as a text string.
    /// </summary>
    /// <param name="CommentList">List of EvFormRecordComment objects: a comment list</param>
    /// <returns>String: a Html text string.</returns>
    /// <remarks>
    /// This method consists of the following steps:
    /// 
    /// 1. Validate whether the commentlist is null and empty
    /// 
    /// 2. Initialise the local variables.
    /// 
    /// 3. Iterate through the current adding items to the new list.
    /// 
    /// 4. Process the item if exists.
    /// 
    /// 5. Set the comment display.
    /// 
    /// 6. If userCommonName exists, Append content, user common name and comment date to sbHtml 
    /// 
    /// 7. Set the comment display.
    /// 
    /// 8. Return the new commentHtml array.
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static String getFieldAnnotationMD ( 
      List<EdFormRecordComment> CommentList )
    {
      // 
      // Initialise the local variables.
      // 
      System.Text.StringBuilder sbMarkDown = new System.Text.StringBuilder ( );

      // 
      // Display existing annotations by adding them as a parameter.
      // 
      if ( CommentList == null )
      {
        return String.Empty;
      }
      if ( CommentList.Count == 0 )
      {
        return String.Empty ;
      }

      // 
      // Iterate through the current adding items to the new list.
      // 
      foreach ( EdFormRecordComment comment in CommentList )
      {
        // 
        // Process the item if exists.
        // 
        if ( comment == null )
        {
          continue;
        }
        //
        // Set the comment display.
        //
        if ( comment.AuthorType == AuthorTypeCodes.Monitor
          || comment.AuthorType == AuthorTypeCodes.Data_Manager )
        {
          sbMarkDown.AppendLine ( comment.Content.Trim() );

          //
          // If userCommonName exists, Append content, user common name and comment date to sbHtml 
          //
          if ( comment.UserCommonName != String.Empty )
          {
            sbMarkDown.AppendLine ( space + ByText + space + comment.UserCommonName
              + space + OnText + space + comment.CommentDate.ToString ( "dd MMM yyyy hh:mm" ) );
          }
        }
        else
        {
          sbMarkDown.AppendLine ( comment.Content );
          //
          // If userCommonName exists, Append content, user common name and comment date to sbHtml 
          //
          if ( comment.UserCommonName != String.Empty )
          {
            sbMarkDown.AppendLine ( space + ByText + space + comment.UserCommonName
              + space + OnText + space + comment.CommentDate.ToString ( "dd MMM yyyy hh:mm" ) );
          }

        }

      }//END annotation list iteration loop

      // 
      // Close the table tag
      // 
      sbMarkDown.AppendLine ( "" );

      // 
      // Return the new annotation array.
      // 
      return sbMarkDown.ToString ( );

    }//END getFieldAnnotationMD method

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

  }//END EvFormComment class

}//END Evado.Model namespace
