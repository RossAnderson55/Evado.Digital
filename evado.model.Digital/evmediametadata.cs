/***************************************************************************************
 * <copyright file="EvBinaryFileMetaData.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvFormContent data object.
 *
 ****************************************************************************************/

using System;
using System.IO;

namespace Evado.Model.Digital
{
  /// <summary>
  ///  This Xml data class contains the trial objects Xml content.
  /// </summary>
  [Serializable]
  public class EvMediaMetaData : EvHasSetValue<EvMediaMetaData.ClassFieldNames>
  {
    #region Class initialisation

    //  =================================================================================
    /// <summary>
    /// The class initialisation method
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public EvMediaMetaData()
    {
    }

    //  =================================================================================
    /// <summary>
    /// The class initialisation method
    /// </summary>
    /// <param name="Category">String: media category</param>
    /// <param name="Language">String: Media language setting</param>
    /// <param name="MediaId">String: Media Identifier used to identify media of different languages</param>
    /// <param name="Title">String: Media title</param>
    /// <param name="MediaUrl">String: Media URL</param>
    /// <param name="MimeType">String: MimeType</param>
    //  ---------------------------------------------------------------------------------
    public EvMediaMetaData(
      String Category,
      String Language,
      String MediaId,
      String Title,
      String MediaUrl,
      String MimeType)
    {
      this._Category = Category;
      this._Language = Language;
      this._MediaId = MediaId;
      this._Title = Title;
      this._MediaUrl = MediaUrl;
      this._MimeType = MimeType;
    }

    #endregion

    #region Public Enumerators.

    /************************************************************************************
     * 
     * Non-clinical activities associated with a clinical milestone will be signed off
     * as completed when the milestone is signed off as complete.
     * 
     ************************************************************************************/
    /// <summary>
    /// This enumeration contains the activity filenames for data update or extraction.
    /// </summary>
    public enum ClassFieldNames
    {
      /// <summary>
      /// Thi enumeration contains null value or no selection state.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration is media category enumerationvalue
      /// </summary>
      Category,

      /// <summary>
      /// The gorup field enumeration.
      /// </summary>
      Language,

      /// <summary>
      /// The FileIndex field enumeration.
      /// </summary>
      MediaId,

      /// <summary>
      /// The Title field enumeration.
      /// </summary>
      Title,

      /// <summary>
      /// The FileName field enumeration.
      /// </summary>
      MediaUrl,

      /// <summary>
      /// The MimeType field enumeration.
      /// </summary>
      MimeType,

      /// <summary>
      /// The Version field enumeration.
      /// </summary>
      Version,

    }

    /// <summary>
    /// This enumeration list contains the miilestone activity action codes.
    /// </summary>
    public enum ActionsCodes
    {
      /// <summary>
      /// This enumeration contains a save state of action code.
      /// </summary>
      Save = 0,

      /// <summary>
      /// This enumeration contains a delete state of action code.
      /// </summary>
      Delete = 1,
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region class properties

    private Guid _Guid = Guid.Empty;
    /// <summary>
    /// This property contains the binary files global unique identifier.
    /// </summary>
    public Guid Guid
    {
      get { return _Guid; }
      set { _Guid = value; }
    }


    private String _Category = String.Empty;
    /// <summary>
    /// This property contains the media category label.
    /// </summary>
    public String Category
    {
      get { return _Category; }
      set { _Category = value; }
    }

    private String _Language = String.Empty;
    /// <summary>
    /// This property contains language setting for the media content.
    /// </summary>
    public String Language
    {
      get { return _Language; }
      set { _Language = value; }
    }


    private String _MediaId = String.Empty;
    /// <summary>
    /// This property contains the media identifier.
    /// </summary>
    public String MediaId
    {
      get { return _MediaId; }
      set { _MediaId = value; }
    }

    private String _Title = String.Empty;
    /// <summary>
    /// This property contains the binary files title.
    /// </summary>
    public String Title
    {
      get { return _Title; }
      set { _Title = value; }
    }

    private String _MediaUrl = String.Empty;
    /// <summary>
    /// This property contains the media URL
    /// </summary>
    public String MediaUrl
    {
      get { return _MediaUrl; }
      set { _MediaUrl = value; }
    }

    private String _MimeType = String.Empty;
    /// <summary>
    /// This property contains the binary files mime type
    /// </summary>
    public String MimeType
    {
      get { return _MimeType; }
      set { _MimeType = value; }
    }

    private int _Version = 0;
    /// <summary>
    /// This property contains the binary update version
    /// </summary>
    public int Version
    {
      get { return _Version; }
      set { _Version = value; }
    }

    private String _UpdatedBy = String.Empty;
    /// <summary>
    /// This property contains the  name of the person that uploaded the file
    /// </summary>
    public String UpdatedBy
    {
      get { return _UpdatedBy; }
      set { _UpdatedBy = value; }
    }

    private String _UpdatedByUserId = String.Empty;
    /// <summary>
    /// This property contains the user id of the person that uploaded the file
    /// </summary>
    public String UpdatedByUserId
    {
      get { return _UpdatedByUserId; }
      set { _UpdatedByUserId = value; }
    }

    private DateTime _UpdatedByDate = EvcStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains the date time stamp when the file was uploaded.
    /// </summary>
    public DateTime UpdatedByDate
    {
      get { return _UpdatedByDate; }
      set { _UpdatedByDate = value; }
    }

    /// <summary>
    /// This property indicates if the mime type is an image.
    /// </summary>
    public bool IsImage
    {
      get
      {
        if (EvBinaryFileMetaData.CONST_SUPPORTED_IMAGE_MIME_TYPES.Contains(this._MimeType) == true)
        {
          return true;
        }
        return false;
      }
    }

    /// <summary>
    /// This property indicates if the mime type is a video.
    /// </summary>
    public bool IsSound
    {
      get
      {
        if (EvBinaryFileMetaData.CONST_SUPPORTED_SOUND_MIME_TYPES.Contains(this._MimeType) == true)
        {
          return true;
        }
        return false;
      }
    }

    /// <summary>
    /// This property indicates if the mime type is a video.
    /// </summary>
    public bool IsVideo
    {
      get
      {
        if (EvBinaryFileMetaData.CONST_SUPPORTED_VIDEO_MIME_TYPES.Contains(this._MimeType) == true)
        {
          return true;
        }
        return false;
      }
    }


    

    #endregion

    #region class methods.

    //  ================================================================================
    /// <summary>
    /// This class sets the value of this activity class field name. Validate the format of the
    /// value. 
    /// </summary>
    /// <param name="FieldId">ClassFieldNames: Name of the field to be setted.</param>
    /// <param name="value">String: value to be setted</param>
    /// <returns>EvEventCodes: true if the value has the proper format</returns>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Switch fieldName and update the value for the property defined by class field names.
    /// </remarks>
    //  --------------------------------------------------------------------------------
    public EvEventCodes setValue ( EvMediaMetaData.ClassFieldNames FieldId, String value )
    {
      switch (FieldId)
      {

        case EvMediaMetaData.ClassFieldNames.Category:
          {
            this.Category = value;
            break;
          }

        case EvMediaMetaData.ClassFieldNames.Language:
          {
            this.Language = value;
            break;
          }

        case EvMediaMetaData.ClassFieldNames.MediaId:
          {
            this.MediaId = value;
            break;
          }
        case EvMediaMetaData.ClassFieldNames.Title:
          {
            this.Title = value;
            break;
          }
        case EvMediaMetaData.ClassFieldNames.MediaUrl:
          {
            this.MediaUrl = value;
            break;
          }
        case EvMediaMetaData.ClassFieldNames.MimeType:
          {
            this.MimeType = value;
            break;
          }
        case EvMediaMetaData.ClassFieldNames.Version:
          {
            this.Version = int.Parse(value);
            break;
          }
      }// End of switch field name

      return EvEventCodes.Ok;

    }//END setValue method.

    //***********************************************************************************
    #endregion
  }//END EvFormBinary class

} //END namespace Evado.Model.Digital
