/***************************************************************************************
 * <copyright file="EvBinaryFileMetaData.cs" company="EVADO HOLDING PTY. LTD.">
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
  public class EvBinaryFileMetaData : EvHasSetValue<EvBinaryFileMetaData.ClassFieldNames>
  {
    #region Class initialisation

    //  =================================================================================
    /// <summary>
    /// The class initialisation method
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public EvBinaryFileMetaData()
    {
    }

    //  =================================================================================
    /// <summary>
    /// The class initialisation method
    /// </summary>
    /// <param name="ProjectGuid">Guid:  Project Guid</param>
    /// <param name="GroupGuid">Guid: Subject Guid</param>
    /// <param name="SubGroupGuid">Guid: filed index Guid</param>
    /// <param name="ProjectId">String: Project identifier</param>
    /// <param name="GroupId">String: Subect Identifier</param>
    /// <param name="SubGroupId">String: Record or Visit identifier</param>
    /// <param name="Title">String: File title</param>
    /// <param name="FileName">String: File name</param>
    /// <param name="MimeType">String: MimeType</param>
    //  ---------------------------------------------------------------------------------
    public EvBinaryFileMetaData(
      Guid ProjectGuid,
      Guid GroupGuid,
      Guid SubGroupGuid,
      String ProjectId,
      String GroupId,
      String SubGroupId,
      String Title,
      String FileName,
      String MimeType)
    {
      this._FileGuid = Guid.NewGuid ( );
      this._ProjectGuid = ProjectGuid;
      this._GroupGuid = GroupGuid;
      this._SubGroupGuid = SubGroupGuid;
      this._ProjectId = ProjectId;
      this._GroupId = GroupId;
      this._SubGroupId = SubGroupId;
      this._Title = Title;
      this._FileName = FileName;
      this._MimeType = MimeType;
    }

    //  =================================================================================
    /// <summary>
    /// The class initialisation method for versioned document and files.
    /// The FileGuid and FileId are used to group versions of a file together. 
    /// i.e. all files in a group must have the same FileGuid and FileId.
    /// </summary>
    /// <param name="FileGuid">Guid:  File Guid</param>
    /// <param name="ProjectGuid">Guid:  Project Guid</param>
    /// <param name="GroupGuid">Guid: Subject Guid</param>
    /// <param name="SubGroupGuid">Guid: filed index Guid</param>
    /// <param name="ProjectId">String: Project identifier</param>
    /// <param name="GroupId">String: First level group e.g. Subect Identifier</param>
    /// <param name="SubGroupId">String: Second level group e.g Record or Visit identifier</param>
    /// <param name="FileId">String: File text identifier identifier</param>
    /// <param name="Title">String: File title</param>
    /// <param name="FileName">String: File name</param>
    /// <param name="MimeType">String: MimeType</param>
    //  ---------------------------------------------------------------------------------
    public EvBinaryFileMetaData (
      Guid FileGuid,
      Guid ProjectGuid,
      Guid GroupGuid,
      Guid SubGroupGuid,
      String ProjectId,
      String GroupId,
      String SubGroupId,
      String FileId,
      String Title,
      String FileName,
      String MimeType )
    {
      this._FileGuid = FileGuid;
      this._ProjectGuid = ProjectGuid;
      this._GroupGuid = GroupGuid;
      this._SubGroupGuid = SubGroupGuid;
      this._ProjectId = ProjectId;
      this._GroupId = GroupId;
      this._SubGroupId = SubGroupId;
      this._FileId = FileId;
      this._Title = Title;
      this._FileName = FileName;
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
      /// The BinaryGuid field enumeration.
      /// </summary>
      FileGuid,

      /// <summary>
      /// The TrialGuid field enumeration
      /// </summary>
      Project_Guid,

      /// <summary>
      /// The SubjectGuid field enumeration.
      /// </summary>
      Group_Guid,

      /// <summary>
      /// The FilingIndexGuid field enumeration.
      /// </summary>
      Sub_Group_Guid,

      /// <summary>
      /// The project id field enumeration.
      /// </summary>
      ProjectId,

      /// <summary>
      /// The gorup field enumeration.
      /// </summary>
      GroupId,

      /// <summary>
      /// The FileIndex field enumeration.
      /// </summary>
      SubGroupId,

      /// <summary>
      /// The field identifier field enumeration.
      /// </summary>
      FileId,

      /// <summary>
      /// The Title field enumeration.
      /// </summary>
      Title,

      /// <summary>
      /// The Title field enumeration.
      /// </summary>
      Comments,

      /// <summary>
      /// The FileName field enumeration.
      /// </summary>
      FileName,

      /// <summary>
      /// The RepositoryFilePath field enumeration.
      /// </summary>
      RepositoryFilePath,

      /// <summary>
      /// The WebPath field enumeration.
      /// </summary>
      WebPath,

      /// <summary>
      /// The MimeType field enumeration.
      /// </summary>
      MimeType,

      /// <summary>
      /// The Status field enumeration.
      /// </summary>
      Status,

      /// <summary>
      /// The upload date field enumeration.
      /// </summary>
      Upload_Date,

      /// <summary>
      /// The release date field enumeration.
      /// </summary>
      Release_Date,

      /// <summary>
      /// The Version field enumeration.
      /// </summary>
      Version,

      /// <summary>
      /// The Version field enumeration.
      /// </summary>
      Encrypted,

    }

    /// <summary>
    /// This enumeration list contains the miilestone activity action codes.
    /// </summary>
    public enum FileStatus
    {
      /// <summary>
      /// Thi enumeration contains null value or no selection state.
      /// </summary>
      Null,

      /// <summary>
      /// This enumeration contains a uploaded file status.
      /// </summary>
      Uploaded,

      /// <summary>
      /// This enumeration contains a released for user file status.
      /// </summary>
      Released,

      /// <summary>
      /// This enumeration contains a superseded file status.
      /// </summary>
      Superseded,
    }

    /// <summary>
    /// This enumeration list contains the miilestone activity action codes.
    /// </summary>
    public enum ActionsCodes
    {
      /// <summary>
      /// This enumeration contains a update the file and binary file metadata
      /// </summary>
      Add,

      /// <summary>
      /// This enumeration contains a update the binary file metadata
      /// </summary>
      Update,

      /// <summary>
      /// This enumeration contains a release and uploaded binary file metadata
      /// </summary>
      Release,

      /// <summary>
      /// This enumeration contains a delete the file and binary file metadata
      /// </summary>
      Delete,
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region class variables and constants

    /// <summary>
    /// This constant contains the supported image mime types.
    /// </summary>
    public const string CONST_SUPPORTED_IMAGE_MIME_TYPES = "image/gif,image/bmp,image/jpeg,image/x-pict,image/pict,image/png,";

    /// <summary>
    /// This constant contains the list of support video mime types.
    /// </summary>
    public const string CONST_SUPPORTED_SOUND_MIME_TYPES = "audio/mpeg,audio/mpeg3 ";

    /// <summary>
    /// This constant contains the list of support video mime types.
    /// </summary>
    public const string CONST_SUPPORTED_VIDEO_MIME_TYPES = "video/mpeg,video/x-mpeg, ";

    /// <summary>
    /// This constant contains the list of support video mime types.
    /// </summary>
    public const string CONST_SUPPORTED_DOCUMENTS_TYPES = "application/msword,application/pdf,text/plain,application/mspowerpoint,application/mspowerpoint, "
      + "video/quicktime, text/richtext, application/plain,  application/excel, ";

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

    private Guid _FileGuid = Guid.Empty;
    /// <summary>
    /// This property contains the binary files global unique identifier.
    /// </summary>
    public Guid FileGuid
    {
      get { return _FileGuid; }
      set { _FileGuid = value; }
    }

    private Guid _ProjectGuid = Guid.Empty;
    /// <summary>
    /// This property contains the binary files project unique identifier.
    /// </summary>
    public Guid TrialGuid
    {
      get { return _ProjectGuid; }
      set { _ProjectGuid = value; }
    }

    private Guid _GroupGuid = Guid.Empty;
    /// <summary>
    /// This property contains the binary files group global unique identifier (Subject.Guid or Organisation.Guid).
    /// </summary>
    public Guid GroupGuid
    {
      get { return _GroupGuid; }
      set { _GroupGuid = value; }
    }

    private Guid _SubGroupGuid = Guid.Empty;
    /// <summary>
    /// This property contains the binary files sub group global unique identifier.
    /// </summary>
    public Guid SubGroupGuid
    {
      get { return _SubGroupGuid; }
      set { _SubGroupGuid = value; }
    }

    private String _Language = "en";
    /// <summary>
    /// This property contains the binary files language identifier.
    /// </summary>
    public String Language
    {
      get { return _Language; }
      set { _Language = value; }
    }

    private String _ProjectId = String.Empty;
    /// <summary>
    /// This property contains the binary files project identifier.
    /// </summary>
    public String TrialId
    {
      get { return _ProjectId; }
      set { _ProjectId = value; }
    }

    private String _GroupId = String.Empty;
    /// <summary>
    /// This property contains the binary group identifier (SubjectId or OrgId ).
    /// </summary>
    public String GroupId
    {
      get { return _GroupId; }
      set { _GroupId = value; }
    }

    private String _SubGroupId = String.Empty;
    /// <summary>
    /// This property contains the binary files sub group identifier.
    /// </summary>
    public String SubGroupId
    {
      get { return _SubGroupId; }
      set { _SubGroupId = value; }
    }

    private String _FileId = String.Empty;
    /// <summary>
    /// This property contains the binary object identifier.
    /// </summary>
    public String FileId
    {
      get { return _FileId; }
      set { _FileId = value; }
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

    private String _Comment = String.Empty;
    /// <summary>
    /// This property contains the binary files title.
    /// </summary>
    public String Comments
    {
      get { return _Comment; }
      set { _Comment = value; }
    }

    private String _FileName = String.Empty;
    /// <summary>
    /// This property contains the binary files filename (with extension) identifier.
    /// </summary>
    public String FileName
    {
      get { return _FileName.Replace(" ", "_"); }
      set
      {
        this._FileName = value;

      }
    }

    /// <summary>
    /// This property contains the binary files filename (with extension) identifier.
    /// </summary>
    public String FileLink
    {
      get { return _FileName.Replace(" ", "_"); }
    }

    private String _RepositoryFilePath = String.Empty;
    /// <summary>
    /// This property contains the binary files respository path.
    /// </summary>
    public String RepositoryFilePath
    {
      get { return _RepositoryFilePath; }
      set { _RepositoryFilePath = value; }
    }

    private bool _TextPath = false;
    /// <summary>
    /// This property sets the file path to text rather than guid for debuging.
    /// </summary>
    public bool TextPath
    {
      get { return _TextPath; }
      set { _TextPath = value; }
    }

    /// <summary>
    /// This property contains the binary repositry directory path.
    /// </summary>
    public String FullRepositoryPath
    {
      get
      {
        String directory = this._RepositoryFilePath;
        //
        // Define the level of the directory hierachy.
        //
        int directoryLevel = 0;

        if ( this._ProjectGuid != Guid.Empty )
        {
          directoryLevel = 1;
        }
        if ( this._GroupGuid != Guid.Empty )
        {
          directoryLevel = 2;
        }
        if ( this._SubGroupGuid != Guid.Empty )
        {
          directoryLevel = 3;
        }

        //
        // The switch statement determines the levels in the directory path.
        //
        switch ( directoryLevel )
        {
          case 1:
            {
              directory = this._RepositoryFilePath
               + this._ProjectGuid.ToString ( "N" ) + "\\";
              break;
            }
          case 2:
            {
              directory = this._RepositoryFilePath
                + this._ProjectGuid.ToString ( "N" ) + "\\"
                + this._GroupGuid.ToString ( "N" ) + "\\";
              break;
            }
          case 3:
            {
              directory = this._RepositoryFilePath
                + this._ProjectGuid.ToString ( "N" ) + "\\"
                + this._GroupGuid.ToString ( "N" ) + "\\"
                + this._SubGroupGuid.ToString ( "N" ) + "\\";
              break;
            }
        }
        return directory;
      }
    }

    /// <summary>
    /// This property contains the binary files directory path (with extension) identifier.
    /// </summary>
    public String FullBinaryFilePath
    {
      get
      {
        return this.FullRepositoryPath
          + this._Guid.ToString("N")
          + ".ebin";
      }
    }

    private String _WebPath = String.Empty;
    /// <summary>
    /// This property contains the binary files Url path.
    /// </summary>
    public String WebPath
    {
      get { return _WebPath; }
      set { _WebPath = value; }
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

    FileStatus _Status = FileStatus.Uploaded;

    /// <summary>
    /// This property defines the files current status.
    /// </summary>
    public FileStatus Status
    {
      get { return _Status; }
      set { _Status = value; }
    }

    DateTime _UploadDate = EvStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains the date the file was uploaded.
    /// </summary>
    public DateTime UploadDate
    {
      get { return _UploadDate; }
      set { _UploadDate = value; }
    }

    DateTime _ReleaseDate = EvStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains the date the file was release for use.
    /// </summary>
    public DateTime ReleaseDate
    {
      get { return _ReleaseDate; }
      set { _ReleaseDate = value; }
    }

    DateTime _SupersededDate = EvStatics.CONST_DATE_NULL;
    /// <summary>
    /// This property contains the date the file was supersed by a new release version.
    /// </summary>
    public DateTime SupersededDate
    {
      get { return _SupersededDate; }
      set { _SupersededDate = value; }
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

    private bool _FileEncrypted = false;
    /// <summary>
    /// This property indicates if the file is encrypted.
    /// </summary>
    public bool FileEncrypted
    {
      get { return _FileEncrypted; }
      set { _FileEncrypted = value; }
    }

    private bool _FileExists = false;
    /// <summary>
    /// This property contains indicates if the binary file exists in the repository.
    /// </summary>
    public bool FileExists
    {
      get { return _FileExists; }
      set { _FileExists = value; }
    }

    private bool _NewFile = false;
    /// <summary>
    /// This property contains indicates if the binary file exists in the repository.
    /// </summary>
    public bool NewFile
    {
      get { return _NewFile; }
      set { _NewFile = value; }
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

    /// <summary>
    /// This property indicates if the mime type is a document.
    /// </summary>
    public bool IsDocumenet
    {
      get
      {
        if ( EvBinaryFileMetaData.CONST_SUPPORTED_DOCUMENTS_TYPES.Contains ( this._MimeType ) == true )
        {
          return true;
        }
        return false;
      }
    }

    #endregion

    #region class methods.
    // ==================================================================================
    /// <summary>
    /// This method generates the mime list to resolve the mime type from the extension.
    /// </summary>
    //  ----------------------------------------------------------------------------------
    public bool createBinaryDirectories ( )
    {
      //
      // exit if the full directory path does not exist.
      //
      if ( System.IO.Directory.Exists ( this.FullRepositoryPath ) == true )
      {
        return false;
      }

      //
      // Define the level of the directory hierachy.
      //
      int directoryLevel = 0;

      if ( this._ProjectGuid != Guid.Empty )
      {
        directoryLevel = 1;
      }
      if ( this._GroupGuid != Guid.Empty )
      {
        directoryLevel = 2;
      }
      if ( this._SubGroupGuid != Guid.Empty )
      {
        directoryLevel = 3;
      }

      //
      // Create the trial directory.
      //
      if ( directoryLevel >= 1 )
      {
        if ( System.IO.Directory.Exists ( this._RepositoryFilePath
            + this._ProjectGuid.ToString ( "N" ) ) == false )
        {
          System.IO.Directory.CreateDirectory ( this._RepositoryFilePath
            + this._ProjectGuid.ToString ( "N" ) );
        }
      }

      //
      // Create the subject directory.
      //
      if ( directoryLevel >= 2 )
      {
        if ( System.IO.Directory.Exists ( this._RepositoryFilePath
            + this._ProjectGuid.ToString ( "N" ) + "\\"
            + this._GroupGuid.ToString ( "N" ) ) == false )
        {
          System.IO.Directory.CreateDirectory ( this._RepositoryFilePath
            + this._ProjectGuid.ToString ( "N" ) + "\\"
            + this._GroupGuid.ToString ( "N" ) );
        }
      }

      //
      // Create the subject directory.
      //
      if ( directoryLevel >= 3 )
      {
      if ( System.IO.Directory.Exists ( this._RepositoryFilePath
          + this._ProjectGuid.ToString ( "N" ) + "\\"
          + this._GroupGuid.ToString ( "N" ) + "\\"
          + this._SubGroupGuid.ToString ( "N" ) ) == false )
      {
        System.IO.Directory.CreateDirectory ( this._RepositoryFilePath
          + this._ProjectGuid.ToString ( "N" ) + "\\"
          + this._GroupGuid.ToString ( "N" ) + "\\"
          + this._SubGroupGuid.ToString ( "N" ) );
      }
    }
      return true;

    }//END createBinaryDirectories method.

    //  ================================================================================
    /// <summary>
    /// This class sets the value of this activity class field name. Validate the format of the
    /// value. 
    /// </summary>
    /// <param name="fieldName">ClassFieldNames: Name of the field to be setted.</param>
    /// <param name="value">String: value to be setted</param>
    /// <returns>EvEventCodes: true if the value has the proper format</returns>
    /// <remarks>
    /// This method consists of the following step:
    /// 
    /// 1. Switch fieldName and update the value for the property defined by class field names.
    /// </remarks>
    //  --------------------------------------------------------------------------------
    public EvEventCodes setValue(EvBinaryFileMetaData.ClassFieldNames fieldName, String value)
    {
      switch (fieldName)
      {
        case EvBinaryFileMetaData.ClassFieldNames.FileGuid:
          {
            this.FileGuid = new Guid(value);
            break;
          }
        case EvBinaryFileMetaData.ClassFieldNames.Project_Guid:
          {
            this.TrialGuid = new Guid(value);
            break;
          }
        case EvBinaryFileMetaData.ClassFieldNames.Group_Guid:
          {
            this.GroupGuid = new Guid(value);
            break;
          }
        case EvBinaryFileMetaData.ClassFieldNames.Sub_Group_Guid:
          {
            this.SubGroupGuid = new Guid(value);
            break;
          }

        case EvBinaryFileMetaData.ClassFieldNames.ProjectId:
          {
            this.TrialId = value;
            break;
          }

        case EvBinaryFileMetaData.ClassFieldNames.GroupId:
          {
            this.GroupId = value;
            break;
          }
        case EvBinaryFileMetaData.ClassFieldNames.SubGroupId:
          {
            this.SubGroupId = value;
            break;
          }
        case EvBinaryFileMetaData.ClassFieldNames.FileId:
          {
            this._FileId = value;
            break;
          }
        case EvBinaryFileMetaData.ClassFieldNames.Title:
          {
            this.Title = value;
            break;
          }
        case EvBinaryFileMetaData.ClassFieldNames.Comments:
          {
            this.Comments = value;
            break;
          }
        case EvBinaryFileMetaData.ClassFieldNames.Encrypted:
          {
            this.FileEncrypted = EvStatics.getBool( value );
            break;
          }
        case EvBinaryFileMetaData.ClassFieldNames.FileName:
          {
            this.FileName = value;
            break;
          }
        case EvBinaryFileMetaData.ClassFieldNames.RepositoryFilePath:
          {
            this.RepositoryFilePath = value;
            break;
          }
        case EvBinaryFileMetaData.ClassFieldNames.WebPath:
          {
            this.WebPath = value;
            break;
          }
        case EvBinaryFileMetaData.ClassFieldNames.MimeType:
          {
            this.MimeType = value;
            break;
          }
        case EvBinaryFileMetaData.ClassFieldNames.Version:
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
