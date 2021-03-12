/* <copyright file="BLL\EvReportTemplates.cs" company="EVADO HOLDING PTY. LTD.">
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
 ****************************************************************************************/

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

//Evado. namespace references.
using Evado.Model;
using Evado.Model.Digital;


namespace Evado.Bll.Digital
{
  /// <summary>
  /// A business Component used to manage report templates.
  /// 
  /// </summary>
  public class EvReportTemplates : EvBllBase
  {
    #region class initialisation methods
    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // ----------------------------------------------------------------------------------
    public EvReportTemplates ( )
    {
      this.ClassNameSpace = "Evado.Bll.Clinical.EvReportTemplates.";
    }

    // ==================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    /// <param name="Settings">EvApplicationSetting data object.</param>
    // ----------------------------------------------------------------------------------
    public EvReportTemplates ( EvClassParameters Settings )
    {
      this.ClassParameter = Settings;
      this.ClassNameSpace = "Evado.Bll.Clinical.EvReportTemplates.";

      if ( this.ClassParameter.LoggingLevel == 0 )
      {
        this.ClassParameter.LoggingLevel = Evado.Dal.EvStaticSetting.LoggingLevel;
      }

      this._dalReportTemplates = new Evado.Dal.Digital.EdReportTemplates ( Settings );
    }
    #endregion

    #region Class variables and properties.
    // 
    // Instantiate the DAL Class\
    // 
    private Evado.Dal.Digital.EdReportTemplates _dalReportTemplates = new Evado.Dal.Digital.EdReportTemplates ( );

    private System.Text.StringBuilder _DebugLog = new System.Text.StringBuilder ( );
     
    #endregion

    #region Class methods

    // =====================================================================================
    /// <summary>
    /// This method retrieves a list of report templates filtered by trial Id, Report QueryType,
    /// report category.
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) The selection trial identifier</param>
    /// <param name="ReportType">EvReport.ReportTypeCode: (Mandatory) The selection Report QueryType identifier</param>
    /// <param name="Category">string: (Optional) The selection category</param>
    /// <returns>List of EvReport: a list of report objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of report objects
    /// 
    /// 2. Return a list of report objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvReport> getReportList (
      EvReport.ReportTypeCode ReportType,
      string Category )
    {
      this._DebugLog = new System.Text.StringBuilder ( );
      this.LogMethod( "getReportList method " );

      List<EvReport> view = this._dalReportTemplates.getReportList ( 
        ReportType, 
        EvReport.ReportScopeTypes.Null, 
        Category, false );

      this.LogClass ( this._dalReportTemplates.Log );
      return view;

    }//END getMilestoneList method.

    // =====================================================================================
    /// <summary>
    /// This class retrieves all of the Report templates no matter the scope.
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) The selection trial identifier</param>
    /// <param name="ReportTypeId">EvReport.ReportTypeCode: (Mandatory) The selection Report QueryType identifier</param>
    /// <param name="Category">string: (Optional) The selection category</param>
    /// <returns>List of EvReport: a list of report objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of report objects
    /// 
    /// 2. Return a list of report objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvReport> getAllReportList (
      EvReport.ReportTypeCode ReportTypeId,
      String Category )
    {
      this._DebugLog = new System.Text.StringBuilder ( );
      this.LogMethod ( "getAllReportList method " );

      List<EvReport> reportList = this._dalReportTemplates.getReportList (
        ReportTypeId,
        EvReport.ReportScopeTypes.Null,
        Category,
        false );

      this.LogClass ( this._dalReportTemplates.Log );
      return reportList;
    }//END getAllView method.

    // =====================================================================================
    /// <summary>
    /// This class retrieves the list of reports based on the scope.
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) The selection trial identifier</param>
    /// <param name="ReportType">EvReport.ReportTypeCode: (Mandatory) The selection Report QueryType identifier</param>
    /// <param name="ReportScope">EvReport.ReportScopeTypes: a report scope</param>
    /// <returns>List of EvReport: a list of report objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of report objects
    /// 
    /// 2. Return a list of report objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvReport> getReportList (
      EvReport.ReportTypeCode ReportType,
      EvReport.ReportScopeTypes ReportScope )
    {
      this._DebugLog = new System.Text.StringBuilder ( );
      this.LogMethod ( "getReportList method " );
      this.LogDebug ( "ReportTypeId: " + ReportType);
      this.LogDebug ( "ReportScope: " + ReportScope );

      List<EvReport> view = this._dalReportTemplates.getReportList (
        ReportType, 
        ReportScope,
        String.Empty,
        false );

      this.LogClass ( this._dalReportTemplates.Log );

      return view;
    }

    // =====================================================================================
    /// <summary>
    /// This class retrieves the list of reports based on the scope.
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) The selection trial identifier</param>
    /// <param name="ReportTypeId">EvReport.ReportTypeCode: (Mandatory) The selection Report QueryType identifier</param>
    /// <param name="Category">string: (Optional) The selection category</param>
    /// <param name="ReportScope">EvReport.ReportScopeTypes: a report scope</param>
    /// <returns>List of EvReport: a list of report objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of report objects
    /// 
    /// 2. Return a list of report objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvReport> getReportList (
      EvReport.ReportTypeCode ReportTypeId,
      String Category,
      EvReport.ReportScopeTypes ReportScope )
    {
      this._DebugLog = new System.Text.StringBuilder ( );
      this.LogMethod( "getReportList method ");
      this.LogDebug ( "ReportTypeId: " + ReportTypeId);
      this.LogDebug ( "Category: " + Category);
      this.LogDebug ( "ReportScope: " + ReportScope );

      List<EvReport> view = this._dalReportTemplates.getReportList (
         ReportTypeId, ReportScope, Category, false );

      this.LogClass ( this._dalReportTemplates.Log );

      return view;
    }

    // =====================================================================================
    /// <summary>
    /// This class retrieves the list of reports based on the scope.
    /// </summary>
    /// <returns>List of Option: a list of report objects</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of report objects
    /// 
    /// 2. Return a list of report objects. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getReportListAll ( )
    {
      this._DebugLog = new System.Text.StringBuilder ( );
      this.LogMethod ( "getReportList method " );

      List<EvOption> view = this._dalReportTemplates.getReportListAll ( );

      this.LogClass ( this._dalReportTemplates.Log );

      return view;
    }


    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Report Source queries
    
    // =====================================================================================
    /// <summary>
    /// This class returns a list of report objects based on the passed parameters. 
    /// </summary>
    /// <returns>List of EvReport: the list of report data object containing the template.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the ReportSourceList methods
    /// 
    /// 2. Return the Report list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvReportSource> getSourceList ( )
    {
      this._DebugLog = new System.Text.StringBuilder ( );
      this.LogMethod ( "getSourceList method. " );

      // 
      // Define the local variables
      // 
      List<EvReportSource> sourceList = new List<EvReportSource> ( );

      sourceList = this._dalReportTemplates.getSourceList ( );

      this.LogClass ( this._dalReportTemplates.Log );

      this.LogDebug ( "EXiT: getSourceList METHOD " );

      // 
      // Return the ArrayList containing the Report data object.
      // 
      return sourceList;

    } // Close getSourceList method.

    // =====================================================================================
    /// <summary>
    /// This class returns a list of report objects based on the passed parameters. 
    /// </summary>
    /// <param name="IsSelectionList">Bool: True the list is a selection list.</param>
    /// <returns>List of EvReport: the list of report data object containing the template.</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the ReportSourceList methods
    /// 
    /// 2. Return the Report list. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getSourceOptionList ( bool IsSelectionList )
    {
      this.LogMethod ( "getSourceOptionList method. " );
      this.LogDebug ( "IsSelectionList: " + IsSelectionList );

      // 
      // Initialise local variables and objects.
      // 
      List<EvReportSource> sourceList = new List<EvReportSource> ( );
      List<EvOption> optionList = new List<EvOption> ( );
      EvOption option = new EvOption ( );

      //
      // Add an empty option for a selection lists.
      //
      if ( IsSelectionList == true )
      {
        optionList.Add ( option );
      }

      //
      // Retrieve the list of report source objects.
      //
      sourceList = this._dalReportTemplates.getSourceList ( );

      this.LogClass ( this._dalReportTemplates.Log );

      this.LogDebug ( "Source list count: " + sourceList.Count );

      //
      // convert the list to an option list.
      //
      foreach ( EvReportSource source in sourceList )
      {
        option = new EvOption ( source.SourceId, source.Name );
        optionList.Add ( option );
      }

      // 
      // Return the list containing the option objects.
      // 
      return optionList;

    } // Close getSourceOptionList method.

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Report Selection list methods.
    // =====================================================================================
    /// <summary>
    /// This class retrieves a currentSchedule of Report categories.
    /// </summary>
    /// <param name="ProjectId">string: (Mandatory) The selection trial identifier</param>
    /// <returns>List of EvOption: a list of option objects for report categories</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving a list of option objects for report categories
    /// 
    /// 2. Return a list of option objects for report categories
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getCategoryList ( string ProjectId )
    {
      List<EvOption> view = this._dalReportTemplates.getCategoryList ( ProjectId );
      this.LogClass ( this._dalReportTemplates.Log );
      return view;

    }//END getCategoryList method.

    // =====================================================================================
    /// <summary>
    /// This class obtains an array of selection option based on the selection source code and the parameters.
    /// </summary>
    /// <param name="Project">EvProject: (Mandatory) The selection project object</param>
    /// <param name="selectionType">EvReport.SelectionListTypes: (Mandatory) The selection Report QueryType</param>
    /// <param name="ListParameters">string: (Mandatory) The parameter string</param>
    /// <param name="profile">Evado.Model.Digital.EdUserProfile: (Mandatory) the User profile object</param>
    /// <returns>List of EvOption: a list of options</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Switch the selection QueryType and execute the method for retrieving the selection list. 
    /// 
    /// 2. Return the selection list 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public List<EvOption> getSelectionList (
      EvReport.SelectionListTypes selectionType,
      string ListParameters,
      Evado.Model.Digital.EdUserProfile profile )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> returnList = new List<EvOption> ( );
      this.LogMethod ( "getSelectionList method " );
      this.LogDebug ( "selectionType: " + selectionType );
      this.LogDebug ( "ListParameters: " + ListParameters );

      //
      // Select the ResultData source
      //
      switch ( selectionType )
      {
        case EvReport.SelectionListTypes.Status:
          {
            this.LogValue ( "Status source selected." );

            List<EvOption> tempList = new List<EvOption>();

            //Transforming the ArrayList into a list<EvOption>
            foreach ( EvOption option in tempList )
            {
              returnList.Add ( option );
            }

            break;
          }
        case EvReport.SelectionListTypes.Record_State:
          {
            this.LogValue ( "Record source selected." );
            returnList = EdRecord.getRptRecordStates ( );
            break;
          }


        case EvReport.SelectionListTypes.LayoutId:
          {
            this.LogValue ( "Form source selected." );
            EdRecordLayouts formsBll = new EdRecordLayouts ( this.ClassParameter );
            returnList = formsBll.getList ( EdRecordTypes.Null, EdRecordObjectStates.Form_Issued, false );
            break;
          }

      }//END Switch 

      return returnList;

    }//END getSelectionList method.

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Report static methods

    // =====================================================================================
    /// <summary>
    /// This method returns a list of option objects for report types
    /// </summary>
    /// <param name="UserRoleId">EvRoleList enumeration defining the user role </param>
    /// <param name="CtmsEnabled">Boolean: True indicate that the CTMS module is enabled. </param>
    /// <returns>List of EvOption: a list of option objects for report types</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create a list of option and add the items to the list. 
    /// 
    /// 2. Return the list of option objects for report types. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static List<EvOption> getReportTypeList ( )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> list = new List<EvOption> ( );

      list.Add ( Evado.Model.EvStatics.getOption ( EvReport.ReportTypeCode.Null ) );

      list.Add ( Evado.Model.EvStatics.getOption ( EvReport.ReportTypeCode.General ) );

      list.Add ( Evado.Model.EvStatics.getOption ( EvReport.ReportTypeCode.Entity ) );

      list.Add ( Evado.Model.EvStatics.getOption ( EvReport.ReportTypeCode.Record ) );

      // 
      // Return the repot QueryType currentSchedule.
      // 
      return list;

    }//END getReportTypeList method.

    // =====================================================================================
    /// <summary>
    /// This method returns a list of option objects for report types
    /// </summary>
    /// <param name="UserRoleId">EvRoleList enumeration defining the user role </param>
    /// <param name="CtmsEnabled">Boolean: True indicate that the CTMS module is enabled. </param>
    /// <returns>List of EvOption: a list of option objects for report types</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Create a list of option and add the items to the list. 
    /// 
    /// 2. Return the list of option objects for report types. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public static List<EvOption> getReportScopeList (  )
    {
      //
      // Initialise the methods variables and objects.
      //
      List<EvOption> list = new List<EvOption> ( );

      list.Add ( new EvOption ( EvReport.ReportScopeTypes.Null, String.Empty ) );

      list.Add ( Evado.Model.EvStatics.getOption ( EvReport.ReportScopeTypes.Operational_Reports ) );

      list.Add ( Evado.Model.EvStatics.getOption ( EvReport.ReportScopeTypes.Monitoring_Reports ) );

      list.Add ( Evado.Model.EvStatics.getOption ( EvReport.ReportScopeTypes.Data_Management_Reports ) );

      list.Add ( Evado.Model.EvStatics.getOption ( EvReport.ReportScopeTypes.Site_Reports ) );

      // 
      // Return the repot QueryType currentSchedule.
      // 
      return list;

    }//END getReportTypeList method.

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Report retrieval methods

    // =====================================================================================
    /// <summary>
    /// This class retrieves a report object based on Guid
    /// </summary>
    /// <param name="ReportGuid">Guid: (Mandatory) The global unique identifier.</param>
    /// <returns>EvReport: a report object</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for retrieving the report object based on the Guid
    /// 
    /// 2. Return the report object. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvReport getReport ( Guid ReportGuid )
    {
      this._DebugLog = new System.Text.StringBuilder ( );
      this.LogMethod( "getReport method. ");
      this.LogDebug ( "Guid: " + ReportGuid );

      EvReport report = this._dalReportTemplates.getReport ( ReportGuid );

      this.LogClass ( this._dalReportTemplates.Log );

      //
      // Return the role.
      //
      return report;

    }//END getReport class

    // =====================================================================================
    /// <summary>
    /// This class retrieves a report object by is scope and retrieves the first instance.
    /// </summary>
    /// <param name="Scope">EvReport.ReportScopeTypes: Report Scope.</param>
    /// <returns>EvReport: a report object</returns>
    // -------------------------------------------------------------------------------------
    public EvReport getReportByScope ( EvReport.ReportScopeTypes Scope )
    {
      this._DebugLog = new System.Text.StringBuilder ( );
      this.LogMethod ( "getReportByScope method. " );
      this.LogDebug ( "Scope: " + Scope );
      EvReport report = new EvReport ( );

      List<EvReport> reportList = this._dalReportTemplates.getReportList (
      EvReport.ReportTypeCode.Null,
      Scope, String.Empty, false );

      if ( reportList.Count == 0 )
      {
        return report;
      }

      report = this._dalReportTemplates.getReport ( reportList [ 0 ].Guid );

      this.LogClass ( this._dalReportTemplates.Log );

      //
      // Return the role.
      //
      return report;

    }//END getReport class

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Report update methods.

    // =====================================================================================
    /// <summary>
    /// This class saves items to the Report ResultData table. 
    /// The update and add process are the same as in each execution the currentMonth objects are 
    /// set to superseded and then a new object is inserted to the database.
    /// </summary>
    /// <param name="Report">EvReport: a report object</param>
    /// <returns>EvEventCodes: an event code for saving items</returns>
    /// <remarks>
    /// This method consists of the following steps: 
    /// 
    /// 1. Execute the method for deleting items, if the Guid is not empty.
    /// 
    /// 2. Else, execute the method for adding items
    /// 
    /// 3. Else, execute the method for updating items. 
    /// 
    /// 4. Return the event code of the method execution. 
    /// </remarks>
    // -------------------------------------------------------------------------------------
    public EvEventCodes saveReport ( EvReport Report )
    {
      this._DebugLog = new System.Text.StringBuilder ( );
      EvEventCodes iReturn = EvEventCodes.Ok;
      this.LogMethod( "saveReport method." );

      //
      // Deletion in the ResultData object consists of setting the currentMonth object to superseded
      // with the rsult that it is not visible to operational system.
      // 
      // If none of the properties are selected then delete the user.
      // 
      if ( Report.ReportTitle == String.Empty )
      {
        if ( Report.Guid != Guid.Empty )
        {
          this.LogValue ( " Deleting Report." );

          iReturn = this._dalReportTemplates.deleteItem ( Report );

          this.LogClass ( this._dalReportTemplates.Log );
        }
        return iReturn;
      }

      // 
      // If the object UID = 0 it is new selection to add a record
      // 
      if ( Report.Guid == Guid.Empty )
      {
        this.LogValue( " Adding Report." );

        iReturn = this._dalReportTemplates.addReport ( Report );
        this.LogClass ( this._dalReportTemplates.Log );

        return iReturn;
      }
      this.LogValue( " Updating Report." );

      iReturn = this._dalReportTemplates.updateReport ( Report );

      this.LogClass ( this._dalReportTemplates.Log );
      return iReturn;

    } // Close saveReport method
    #endregion


  }//END EvReportTemplates

}//END namespace Evado.Bll.Digital
