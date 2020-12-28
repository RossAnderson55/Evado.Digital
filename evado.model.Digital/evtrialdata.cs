/***************************************************************************************
 * <copyright file="ProjectData.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the data model for the Trial object .  
 * 
 *
 ****************************************************************************************/

using System;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace Evado.Model.Digital
{
  /// <summary>
  /// The data mode class for the Trial additional data object.
  /// </summary>
  [Serializable]
  public class EvTrialData  //EvTrialData
  {
    #region public Constants
    /// <summary>
    /// This constant defines a draft state of data dictionary
    /// </summary>
    public const string CONST_DATA_DICTIONARY_STATE_DRAFT = "CL_CD";

    /// <summary>
    /// This constant defines a reviewed state of data dictionary
    /// </summary>
    public const string CONST_DATA_DICTIONARY_STATE_REVIEWED = "CL_CR";

    /// <summary>
    /// This constant defines an issued state of data dictionary
    /// </summary>
    public const string CONST_DATA_DICTIONARY_STATE_ISSUED = "CL_CI";

    #endregion

    #region public members

    // ==================================================================================
    List<EvUserSignoff> _Signoffs = new List<EvUserSignoff> ( );
    /// <summary>
    /// This property contains a user signoff list of the trial.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public List<EvUserSignoff> Signoffs
    {
      get
      { return _Signoffs; }
      set
      { _Signoffs = value; }
    }

    // ==================================================================================
    private String _CoordinatingSiteId = String.Empty;
    /// <summary>
    /// This property contains a coordinating site identifier of the trial.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public String CoordinatingSiteId
    {
      get { return this._CoordinatingSiteId; }
      set { this._CoordinatingSiteId = value; }
    }

    // ==================================================================================
    private bool _CoordinatingSiteChanged = false;
    /// <summary>
    /// This property flags if the coordinating site has been changed.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public bool CoordinatingSiteChanged
    {
      get { return this._CoordinatingSiteChanged; }
      set { this._CoordinatingSiteChanged = value; }
    }

    // ==================================================================================
    private bool _SponsorChanged = false;
    /// <summary>
    /// This property indicates if the sponsor has been changed.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public bool SponsorChanged
    {
      get { return this._SponsorChanged; }
      set { this._SponsorChanged = value; }
    }

    // ==================================================================================
    private String _TrialSites = String.Empty;
    /// <summary>
    /// This property contains trial sites of the trial
    /// </summary>
    // ----------------------------------------------------------------------------------
    public String TrialSites
    {
      get
      {
        return _TrialSites;
      }
      set
      {
        _TrialSites = value;
      }
    }

    // ==================================================================================
    private Boolean _EnableEdc = false;
    /// <summary>
    /// This property indicates whether a trial data has electronic data enabled
    /// </summary>
    // ----------------------------------------------------------------------------------
    public Boolean EnableEdc
    {
      get
      {
        return this._EnableEdc;
      }
      set
      {
        this._EnableEdc = value;
      }
    }

    private Boolean _EnableElectronicConsent = false;
    // ==================================================================================
    /// <summary>
    /// This property indicates whether a project has electronic consent is enabled.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public Boolean EnableInformedConsent
    {
      get
      {
        return this._EnableElectronicConsent;
      }
      set
      {
        this._EnableElectronicConsent = value;
      }
    }

    private Boolean _EnableAuxiliarySubjectData = false;
    // ==================================================================================
    /// <summary>
    /// This property indicates whether a project has auxiliary is enabled.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public Boolean EnableAuxiliarySubjectData
    {
      get
      {
        return this._EnableAuxiliarySubjectData;
      }
      set
      {
        this._EnableAuxiliarySubjectData = value;
      }
    }

    private Boolean _EnablePatientData = false;
    // ==================================================================================
    /// <summary>
    /// This property indicates whether a project has patient data is enabled.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public Boolean EnablePatientData
    {
      get
      {
        return this._EnablePatientData;
      }
      set
      {
        this._EnablePatientData = value;
      }
    }

    private Boolean _EnablePatientRecordedObservations = false;
    // ==================================================================================
    /// <summary>
    /// This property indicates whether a project has patient recorded observations is enabled.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public Boolean EnablePatientRecordedObservations
    {
      get
      {
        return this._EnablePatientRecordedObservations;
      }
      set
      {
        this._EnablePatientRecordedObservations = value;

        //
        // If patient recorded observations is enabled then patient data needs to be enabed
        //
        if ( this._EnablePatientRecordedObservations == true )
        {
          this._EnablePatientData = true;
        }
      }
    }

    // ==================================================================================
    private Boolean _EnableCtms = false;
    /// <summary>
    /// This property indicates whether a trial data has using clinical trial management system enabled
    /// </summary>
    // ----------------------------------------------------------------------------------
    public Boolean EnableCtms
    {
      get
      {
        return this._EnableCtms;
      }
      set
      {
        this._EnableCtms = value;
      }
    }

    private Boolean _OnEdit_HideFieldAnnotation = true;
    // ==================================================================================
    /// <summary>
    /// This property indicates whether a form records display annotations on editing.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public Boolean OnEdit_HideFieldAnnotation
    {
      get
      {
        return this._OnEdit_HideFieldAnnotation;
      }
      set
      {
        this._OnEdit_HideFieldAnnotation = value;
      }
    }

    private Boolean _EnableNightlyDataPointUpdate = false;
    // ==================================================================================
    /// <summary>
    /// This property indicates whether a project is performing nightly data point updates.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public Boolean EnableNightlyDataPointUpdate
    {
      get { return this._EnableNightlyDataPointUpdate; }
      set { this._EnableNightlyDataPointUpdate = value; }
    }
    private Boolean _EnableProtocolExceptions = false;
    // ==================================================================================
    /// <summary>
    /// This property indicates the project is tracking protocol exceptions
    /// </summary>
    // ----------------------------------------------------------------------------------
    public Boolean EnableProtocolExceptions
    {
      get
      {
        return this._EnableProtocolExceptions;
      }
      set
      {
        this._EnableProtocolExceptions = value;
      }
    }

    private Boolean _EnableProtocolVariations = false;
    // ==================================================================================
    /// <summary>
    /// This property indicates whether a project has tracking protocol variations enabled.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public Boolean EnableProtocolVariations
    {
      get
      {
        return this._EnableProtocolVariations;
      }
      set
      {
        this._EnableProtocolVariations = value;
      }
    }

    // =====================================================================================
    private bool _EnableBinaryDataCollection = false;
    /// <summary>
    /// This property indicates binary data is being collected.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public bool EnableBinaryData
    {
      get
      {
        return _EnableBinaryDataCollection;
      }
      set
      {
        _EnableBinaryDataCollection = value;
      }
    }

    // ==================================================================================
    private Boolean _EnableAeAlerts = false;
    /// <summary>
    /// This property indicates whether a trial data is generating AE alerts
    /// </summary>
    // ----------------------------------------------------------------------------------
    public Boolean EnableAeAlerts
    {
      get { return this._EnableAeAlerts; }
      set { this._EnableAeAlerts = value; }
    }

    // ==================================================================================
    private Boolean _EnableSaeAlerts = false;
    /// <summary>
    /// This property indicates whether a trial data is generating SAE alerts
    /// </summary>
    // ----------------------------------------------------------------------------------
    public Boolean EnableSaeAlerts
    {
      get { return this._EnableSaeAlerts; }
      set { this._EnableSaeAlerts = value; }
    }

    // ==================================================================================
    private String _EmailAlert_Sae_AddressList = String.Empty;
    /// <summary>
    /// This property contains an email alert serious adverse event address list of the trial
    /// </summary>
    public String EmailAlert_Sae_AddressList
    {
      get
      {
        return _EmailAlert_Sae_AddressList;
      }
      set
      {
        _EmailAlert_Sae_AddressList = value;
      }
    }

    // ==================================================================================
    private String _EmailAlert_Ae_AddressList = String.Empty;
    /// <summary>
    /// This property contains an email alert AE address list of the trial.
    /// </summary>
    // ----------------------------------------------------------------------------------
    public String EmailAlert_Ae_AddressList
    {
      get
      {
        return _EmailAlert_Ae_AddressList;
      }
      set
      {
        _EmailAlert_Ae_AddressList = value;
      }
    }

    private String _EmailAlert_Ae_HtmlBody = String.Empty;
    // ==================================================================================
    /// <summary>
    /// This property contains an email alert html body of the trial. 
    /// </summary>
    // ----------------------------------------------------------------------------------
    public String EmailAlert_Ae_HtmlBody
    {
      get
      {
        return _EmailAlert_Ae_HtmlBody;
      }
      set
      {
        _EmailAlert_Ae_HtmlBody = value;
      }
    }

    private String _EmailAlert_Sae_HtmlBody = String.Empty;

    // ==================================================================================
    /// <summary>
    /// This property contains an email alert html body of the trial. 
    /// </summary>
    // ----------------------------------------------------------------------------------
    public String EmailAlert_Sae_HtmlBody
    {
      get
      {
        return _EmailAlert_Sae_HtmlBody;
      }
      set
      {
        _EmailAlert_Sae_HtmlBody = value;
      }
    }

    private String _EmailAlert_Ae_FailureText = String.Empty;
    /// <summary>
    /// This property contains an email alert failure text of the trial.
    /// </summary>
    public String EmailAlert_Ae_FailureText
    {
      get
      {
        return _EmailAlert_Ae_FailureText;
      }
      set
      {
        _EmailAlert_Ae_FailureText = value;
      }
    }
    private String _EmailAlert_Sae_FailureText = String.Empty;
    /// <summary>
    /// This property contains an email alert failure text of the trial.
    /// </summary>
    public String EmailAlert_Sae_FailureText
    {
      get
      {
        return _EmailAlert_Sae_FailureText;
      }
      set
      {
        _EmailAlert_Sae_FailureText = value;
      }
    }

    private EdApplicationSettings.ProjectBillingEventOptions _BillingEvent = EdApplicationSettings.ProjectBillingEventOptions.Null;
    /// <summary>
    /// This property contains the enumeration value of TrialBillingEventOptions 
    /// setting for the trial setting when the billing event will occur.
    /// </summary>
    public EdApplicationSettings.ProjectBillingEventOptions BillingEvent
    {
      get
      {
        return this._BillingEvent;
      }
      set
      {
        this._BillingEvent = value;
      }
    }

    private float _EstablishmentCost = 0;
    /// <summary>
    /// This property contains the establishment costs of setting up the trial.
    /// </summary>
    public float EstablishmentCost
    {
      get
      {
        return this._EstablishmentCost;
      }
      set
      {
        this._EstablishmentCost = value;
      }
    }

    private int _SitePaymentFinalLock = 0;
    /// <summary>
    /// This property contains the dollar value of the site payment when the trial is locked.
    /// </summary>
    public int SitePaymentFinalLock
    {
      get
      {
        return this._SitePaymentFinalLock;
      }
      set
      {
        this._SitePaymentFinalLock = value;
      }
    }

    private int _SitePaymentCompleteVisit = 0;
    /// <summary>
    /// This property contains the dollar value of the site payment when the visit is completed.
    /// </summary>
    public int SitePaymentCompleteVisit
    {
      get
      {
        return this._SitePaymentCompleteVisit;
      }
      set
      {
        this._SitePaymentCompleteVisit = value;
      }
    }

    private int _SitePaymentMonitoredVisit = 0;
    /// <summary>
    /// This property contains the dollar value of the site payment when the visit is monitored.
    /// </summary>
    public int SitePaymentMonitoredVisit
    {
      get
      {
        return this._SitePaymentMonitoredVisit;
      }
      set
      {
        this._SitePaymentMonitoredVisit = value;
      }
    }

    private int _SitePaymentCleanData = 0;
    /// <summary>
    /// This property contains the dollar value of the site payment when the visit is clean data.
    /// </summary>
    public int SitePaymentCleanData
    {
      get
      {
        return this._SitePaymentCleanData;
      }
      set
      {
        this._SitePaymentCleanData = value;
      }
    }
    private float _BudgetCostTotal = 0;
    /// <summary>
    /// This property contains the total budget cost for the trial.
    /// </summary>
    public float BudgetCostTotal
    {
      get
      {
        return this._BudgetCostTotal;
      }
      set
      {
        this._BudgetCostTotal = value;
      }
    }

    private float _BudgetPriceTotal = 0;
    /// <summary>
    /// This property contains the total budget price for the trial.
    /// </summary>
    public float BudgetPriceTotal
    {
      get
      {
        return this._BudgetPriceTotal;
      }
      set
      {
        this._BudgetPriceTotal = value;
      }
    }

    private float _InvoicedTotal = 0;
    /// <summary>
    /// This property contains the total invoiced value for the trial.
    /// </summary>
    public float InvoicedTotal
    {
      get
      {
        return this._InvoicedTotal;
      }
      set
      {
        this._InvoicedTotal = value;
      }
    }

    private float _DefaultMargin = 0;
    /// <summary>
    /// This property contains the trial profit margin to applied to all site costs and expenses.
    /// </summary>
    public float DefaultMargin
    {
      get
      {
        return this._DefaultMargin;
      }
      set
      {
        this._DefaultMargin = value;
      }
    }

    // =====================================================================================
    private string _AeRegulatoryReportXsl = "xForm.xsl";
    /// <summary>
    /// The property contains an AE regulatory report xsl of the trial. 
    /// </summary>
    // -------------------------------------------------------------------------------------
    public string AeRegulatoryReportXsl
    {
      get
      {
        return this._AeRegulatoryReportXsl.Replace ( @"xsl\", String.Empty );
      }
      set
      {
        this._AeRegulatoryReportXsl = value;
      }
    }

    // =====================================================================================
    private string _SaeRegulatoryReportXsl = "xForm.xsl";
    /// <summary>
    /// This property contains a serious adverse event regulatory report template of the trial.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public string SaeRegulatoryReportXsl
    {
      get
      {
        return this._SaeRegulatoryReportXsl.Replace ( @"xsl\", String.Empty );
      }
      set
      {
        this._SaeRegulatoryReportXsl = value;
      }
    }

    // =====================================================================================
    private string _CcmRegulatoryReportXsl = "xForm.xsl";
    /// <summary>
    /// This property contains the CCM regulatory report template of the trial.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public string CcmRegulatoryReportXsl
    {
      get
      {
        return this._CcmRegulatoryReportXsl.Replace ( @"xsl\", String.Empty ); ;
      }
      set
      {
        this._CcmRegulatoryReportXsl = value;
      }
    }


    // =====================================================================================
    private bool _SetInitialVisitStateToAttended = false;
    /// <summary>
    /// This property indicates if the Visit status is to be set to Attended when the visit is first scheduled.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public bool SetInitialVisitStateToAttended
    {
      get
      {
        return _SetInitialVisitStateToAttended;
      }
      set
      {
        _SetInitialVisitStateToAttended = value;
      }
    }

    // =====================================================================================
    private bool _OfflineDataCollectionEnabled = false;
    /// <summary>
    /// This property indicates whether the trial is enabled offline data collection. 
    /// </summary>
    // -------------------------------------------------------------------------------------
    public bool OfflineDataCollectionEnabled
    {
      get
      {
        return _OfflineDataCollectionEnabled;
      }
      set
      {
        _OfflineDataCollectionEnabled = value;
      }
    }

    /// <summary>
    /// This property indicates whether all visits are selected for offline operation. 
    /// </summary>
    public bool OfflineVisitSelection_AllScheduled = false;

    /// <summary>
    /// This property indicates whether the next visit is selected for offline operation.
    /// and over sets all scheduled visits to false.
    /// </summary>
    public bool OfflineVisitSelection_NextScheduled = true;

    /// <summary>
    /// This property indicates whether attended visits are selected for offline operation.
    /// </summary>
    public bool OfflineVisitSelection_Attended = true;

    /// <summary>
    /// This property indicates whether completed visits are selected for offline operation.
    /// </summary>
    public bool OfflineVisitSelection_Completed = false;

    /// <summary>
    /// This property indicates whether monitored visits are selected for offline operation.
    /// </summary>
    public bool OfflineVisitSelection_Monitored = false;

    /// <summary>
    /// This property indicates whether draft records are selected for offline operation.
    /// </summary>
    public bool OfflineRecordSelection_Draft = true;

    /// <summary>
    /// This property indicates whether submitted records are selected for offline operation.
    /// </summary>
    public bool OfflineRecordSelection_Submitted = true;

    /// <summary>
    /// This property indicates whether source data verifeid records are selected for offline operation.
    /// </summary>
    public bool OfflineRecordSelection_Source = false;

    #endregion

    #region Public methods

    #endregion

  }//END EvTrialData class

}//END namespace Evado.Trial.Model 
