/***************************************************************************************
 * <copyright file="BLL\EvAlerts.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2002 - 2020 EVADO HOLDING PTY. LTD..  All rights reserved.
 *     
 *      The use and distribution terms for this software are contained in the file
 *      named license.txt, which can be found in the root of this distribution.
 *      By using this software in any fashion, you are agreeing to be bound by the
 *      terms of this license.
 *     
 *      You must not remove this notice, or any other, from this software.
 *     
 * </copyright>
 * 
 * Description: 
 *  This class contains the EvAlerts business object.
 *
 ****************************************************************************************/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

//Evado. namespace references.
using Evado.Model;
using Evado.Dal;
using Evado.Model.Digital;


namespace Evado.Bll.Clinical
{
  /// <summary>
  /// This business object manages the EvActvityFormss in the system.
  /// </summary>
  public class EvBinaryFiles : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvBinaryFiles ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvBinaryFiles.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvBinaryFiles ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvBinaryFiles.";

      this._DalBinaryFiles = new Evado.Dal.Clinical.EvBinaryFiles ( Settings );
    }
    #endregion

    #region Class enumeration
    /// <summary>
    /// This enumeration list defines the Action codes of Binary files object. 
    /// </summary>
    public enum ActionsCodes
    {
      /// <summary>
      /// This enumeration defines the save action for binary files object
      /// </summary>
      Save,

      /// <summary>
      /// This enumeration defines the delete action for binary files object
      /// </summary>
      Delete,

      /// <summary>
      /// This enumeration defines the reorder action for binary files object
      /// </summary>
      Reorder,
    }
    #endregion

    #region Class Variables and Property
    //
    //  Create instantiate the DAL class 
    // 
    private Evado.Dal.Clinical.EvBinaryFiles _DalBinaryFiles = new Evado.Dal.Clinical.EvBinaryFiles ( );

    #endregion

    #region Class methods
    // =====================================================================================
    /// <summary>
    /// This class return a list of Binary File Meta ResultData objects based on ProjectId, SubjectId and ObjectId
    /// </summary>
    /// <param name="ProjectId">String: A Project identifier</param>
    /// <param name="OrgId">String: A Group identifier</param>
    /// <returns>List of EvBinaryFileMetaData: A list of binary file meta ResultData object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of Binary file objects
    /// 
    /// 2. Return the list of Binary file objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvBinaryFileMetaData> getProjectFileList (
      String OrgId )
    {
      this.LogMethod ( "getProjectFileList method. " );
      this.LogDebug ( "OrgId: " + OrgId );

      // 
      // Execute the method for retrieving the list of Binary file objects
      // 
      List<EvBinaryFileMetaData> fileList = this._DalBinaryFiles.getProjectFileList (
        OrgId );

      this.LogClass ( this._DalBinaryFiles.Log );

      this.LogMethodEnd ( "getProjectFileList" );

      return fileList;

    }//End getBinaryFileList method.

    // =====================================================================================
    /// <summary>
    /// This class return a list of Binary File Meta ResultData objects based on ProjectId, SubjectId and ObjectId
    /// </summary>
    /// <param name="GroupId">String: A Group identifier</param>
    /// <param name="SubGroupId">String: A sub group identifier</param>
    /// <returns>List of EvBinaryFileMetaData: A list of binary file meta ResultData object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of Binary file objects
    /// 
    /// 2. Return the list of Binary file objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvBinaryFileMetaData> getBinaryFileList (
      String GroupId,
      String SubGroupId )
    {
      this.LogMethod ( "getBinaryFileList method. " );
      this.LogDebug ( "GroupId: " + GroupId );
      this.LogDebug ( "SubGroupId: " + SubGroupId );

      // 
      // Execute the method for retrieving the list of Binary file objects
      // 
      List<EvBinaryFileMetaData> fileList = this._DalBinaryFiles.getBinaryFileList (
        GroupId,
        SubGroupId );

      this.LogClass ( this._DalBinaryFiles.Log );

      this.LogMethodEnd ( "getBinaryFileList" );

      return fileList;

    }//End getBinaryFileList method.

    // =====================================================================================
    /// <summary>
    /// This class return a list of Binary File Meta ResultData objects based on ProjectId, SubjectId and ObjectId
    /// </summary>
    /// <param name="ProjectId">String: A Project identifier</param>
    /// <param name="GroupId">String: A Group identifier</param>
    /// <param name="SubGroupId">String: A sub group identifier</param>
    /// <param name="FileId">String: An Object identifier</param>
    /// <returns>List of EvBinaryFileMetaData: A list of binary file meta ResultData object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the list of Binary file objects
    /// 
    /// 2. Return the list of Binary file objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvBinaryFileMetaData> GetVersionedFileList (
      String GroupId,
      String SubGroupId,
      String FileId )
    {
      this.LogMethod ( "GetVersionedFileList method. " );
      this.LogDebug ( "GroupId: " + GroupId );
      this.LogDebug ( "SubGroupId: " + SubGroupId );
      this.LogDebug ( "FileId: " + FileId );

      // 
      // Execute the method for retrieving the list of Binary file objects
      // 
      List<EvBinaryFileMetaData> fileList = this._DalBinaryFiles.GetVersionedFileList (
        GroupId,
        SubGroupId,
        FileId );

      this.LogClass ( this._DalBinaryFiles.Log );

      this.LogMethodEnd ( "GetVersionedFileList" );
      return fileList;

    }//End GetVersionedFileList method.

    // =====================================================================================
    /// <summary>
    /// This class retrieves the Binary file based on it Guid
    /// </summary>
    /// <param name="BinaryGuid">Guid: A Binary global unique identifier</param>
    /// <returns>EvBinaryFileMetaData: the binary file ResultData object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the Binary file ResultData objects. 
    /// 
    /// 2. Return the Binary file ResultData object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvBinaryFileMetaData GetFile ( Guid BinaryGuid )
    {
      this.LogMethod ( "GetFile method. " );
      this.LogDebug ( "BinaryGuid: " + BinaryGuid );

      // 
      // Execute the method for retrieving the Binary File ResultData objects. 
      // 
      EvBinaryFileMetaData file = this._DalBinaryFiles.GetFile ( BinaryGuid );

      this.LogClass ( this._DalBinaryFiles.Log );

      this.LogMethodEnd ( "GetFile" );
      return file;

    }//End getItem method.


    // =====================================================================================
    /// <summary>
    /// This class saves items to the Binary file ResultData table.
    /// </summary>
    /// <param name="BinaryFile">EvBinaryFileMetaData: A Binary file ResultData object</param>
    /// <param name="Action">EvBinaryFileMetaData.ActionsCodes enumerated value</param>
    /// <returns>EvEventCodes: An event code for saving items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Exit, if the Binary file object has no value. 
    /// 
    /// 2. Add items to the Binary file table. 
    /// 
    /// 3. Return the event code for adding items. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes SaveItem (
      EvBinaryFileMetaData BinaryFile,
      EvBinaryFileMetaData.ActionsCodes Action )
    {
      this.LogMethod ( "saveItem method. " );
      this.LogDebug ( "ProjectGuid: " + BinaryFile.TrialGuid );
      this.LogDebug ( "ProjectId: " + BinaryFile.TrialId );
      this.LogDebug ( "GroupGuid: " + BinaryFile.GroupGuid );
      this.LogDebug ( "GroupId: " + BinaryFile.GroupId );
      this.LogDebug ( "SubGroupGuid: " + BinaryFile.SubGroupGuid );
      this.LogDebug ( "SubGroupId: " + BinaryFile.SubGroupId );
      this.LogDebug ( "FileGuid: " + BinaryFile.FileGuid );
      this.LogDebug ( "FileId: " + BinaryFile.FileId );
      this.LogDebug ( "FileName: " + BinaryFile.FileName );
      this.LogDebug ( "MimeType: " + BinaryFile.MimeType );
      this.LogDebug ( "FileEncrypted: " + BinaryFile.FileEncrypted );

      // 
      // Instantiate the local variables
      // 
      EvEventCodes iReturn = EvEventCodes.Ok;

      //
      // Exit, if the Binary file ResultData object has no value. 
      //
      if ( BinaryFile.TrialGuid == Guid.Empty )
      {
        this.LogDebug ( "ERROR: TrialGuid Empty" );

        return EvEventCodes.Identifier_Project_Id_Error;
      }
      if ( BinaryFile.TrialId == String.Empty )
      {
        this.LogDebug ( "ERROR: ProjectId Empty" );

        return EvEventCodes.Identifier_Project_Id_Error;
      }
      if ( BinaryFile.FileName == String.Empty )
      {
        this.LogDebug ( "ERROR: FileName Empty" );

        return EvEventCodes.Data_Null_Data_Error;
      }

      //
      // If the binary file is empty update by adding a new record.
      //
      switch ( Action )
      {
        case EvBinaryFileMetaData.ActionsCodes.Update:
          {
            this.LogDebug ( "Update BinaryFile" );
            //
            // Execute the method for adding items 
            //
            iReturn = this._DalBinaryFiles.updateItem ( BinaryFile );
            this.LogClass ( this._DalBinaryFiles.Log );

            return iReturn;
          }
        case EvBinaryFileMetaData.ActionsCodes.Release:
          {
            this.LogDebug ( "Release BinaryFile" );
            //
            // Update the status to released.
            //
            if ( BinaryFile.Status == EvBinaryFileMetaData.FileStatus.Uploaded )
            {
              BinaryFile.Status = EvBinaryFileMetaData.FileStatus.Released;
              BinaryFile.ReleaseDate = DateTime.Now;
            }
            
            //
            // Execute the method for adding items 
            //
            iReturn = this._DalBinaryFiles.updateItem ( BinaryFile );
            this.LogClass ( this._DalBinaryFiles.Log );

            return iReturn;
          }
        case EvBinaryFileMetaData.ActionsCodes.Add:
        default:
          {
            this.LogDebug ( "Add BinaryFile" );
            BinaryFile.Status = EvBinaryFileMetaData.FileStatus.Uploaded;
            BinaryFile.UploadDate = DateTime.Now;

            if ( BinaryFile.FileGuid == Guid.Empty )
            {
              BinaryFile.FileGuid = BinaryFile.Guid;
            }
            if ( BinaryFile.FileId == String.Empty )
            {
              BinaryFile.FileId = BinaryFile.FileName;
            }
            if ( BinaryFile.Title == String.Empty )
            {
              string title = BinaryFile.FileName;
              int index = title.LastIndexOf ( '.' );
              if ( index >= 0 )
              {
                title = title.Substring ( 0, index );
              }
              title = title.Replace ( "-", " " );
              BinaryFile.Title = title.Replace ( "  ", " " );
            }

            if ( BinaryFile.Version == 0 )
            {
              BinaryFile.Version = 1;
            }
            else
            {
              BinaryFile.Version++;
            }
            this.LogDebug ( " BinaryFile.Version: " + BinaryFile.Version );
            this.LogDebug ( " BinaryFile.FileEncrypted: " + BinaryFile.FileEncrypted );
            this.LogDebug ( " BinaryFile.FileExists: " + BinaryFile.FileExists );
            this.LogDebug ( " BinaryFile.Comments: " + BinaryFile.Comments );
            this.LogDebug ( " BinaryFile.Language: " + BinaryFile.Language );


            //
            // Execute the method for adding items 
            //
            iReturn = this._DalBinaryFiles.addItem ( BinaryFile );
            this.LogClass ( this._DalBinaryFiles.Log );

            return iReturn;
          }
      }

    } // Close method updateIndex
    #endregion

  }//END EvBinaryFiles Class.

}//END namespace Evado.Evado.BLL 
