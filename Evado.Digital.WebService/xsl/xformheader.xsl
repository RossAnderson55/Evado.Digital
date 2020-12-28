<?xml version="1.0" encoding="ISO-8859-1" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  >

  <!--
*******************************************************************************
*                COPYRIGHT (C) EVADO HOLDING Pty. Ltd.  2000 - 2016
*
*                            ALL RIGHTS RESERVED
*
*******************************************************************************
-->
  <!--
  ===================================  Form Header Template ===============================
   -->
  <xsl:template name="FormHeader">
    <table id="HeaderTable">
      <tr>
        <td class="LeftColumn" align="center">

          <xsl:if test="string-length( Trial/CroName )> 0">
            <p class='header2'>
              <xsl:value-of select="Trial/CroName" />
            </p>
          </xsl:if>
          <xsl:if test="string-length( Trial/Title) > 0">
            <p class='header3'>
              <xsl:value-of select="Trial/Title" />
            </p>
          </xsl:if>
          <xsl:if test="string-length( Trial/Sponsor )> 0">
            <p class='header3'>
              <xsl:value-of select="Trial/Sponsor" />
            </p>
          </xsl:if>
          <xsl:if test="string-length( Design/Title) > 0">
            <p class='header3'>
              <xsl:if test="string-length( Design/Reference ) > 0">
                <a href="Design/Reference" target="_blank">
                  <xsl:value-of select="Design/Title" />
                </a>
              </xsl:if>
              <xsl:if test="string-length( Design/Reference ) = 0">
                <xsl:value-of select="Design/Title" />
              </xsl:if>
            </p>
          </xsl:if>
          <xsl:if test="string-length( AuthoredBy ) > 0">
            <p class='para'>
              <b>Author Signoff: </b><xsl:value-of select="AuthoredBy" /> on <xsl:value-of select="stAuthoredDate" />
            </p>
          </xsl:if>
          <xsl:if test="string-length( ReviewedBy ) > 0">
            <p class='para'>
              <b>Review Signoff: </b><xsl:value-of select="ReviewedBy" /> on <xsl:value-of select="stReviewedDate" />
            </p>
          </xsl:if>
          <xsl:if test="string-length( ApprovedBy ) > 0">
            <p class='para'>
              <b>Approver Signoff: </b><xsl:value-of select="ApprovedBy" /> on <xsl:value-of select="stApprovalDate" />
            </p>
          </xsl:if>
          <xsl:if test="string-length( Monitor ) > 0">
            <p class='para'>
              <b>Monitor Signoff: </b><xsl:value-of select="Monitor" /> on <xsl:value-of select="stMonitorDate" />
            </p>
          </xsl:if>
        </td>
        <td class="RightColumn">
          <table >
            <tr>
              <td class="Prompt">Project Id:</td>
              <td>
                <xsl:value-of select="TrialId" />
              </td>
            </tr>
            <xsl:if test="string-length( SubjectId) > 0">
              <tr>
                <td class="Prompt">Subject Id:</td>
                <td>
                  <xsl:value-of select="SubjectId" />
                </td>
              </tr>
            </xsl:if>
            <xsl:if test="string-length( VisitId ) > 0">
              <tr>
                <td class="Prompt">Visit Id:</td>
                <td>
                  <xsl:value-of select="VisitId" />
                </td>
              </tr>
            </xsl:if>
            <xsl:if test="string-length( ReferenceId ) > 0 and Design/TypeId != 'Adverse_Event_Report'" >
              <tr>
                <td class="Prompt">Reference Id:</td>
                <td>
                  <xsl:value-of select="ReferenceId" />
                </td>
              </tr>
            </xsl:if>
            <xsl:if test="string-length( FormId ) > 0">
              <tr>
                <td class="Prompt">Form Id:</td>
                <td>
                  <xsl:value-of select="FormId" />
                </td>
              </tr>
            </xsl:if>
            <xsl:if test="string-length( RecordId ) > 0">
              <tr>
                <td class="Prompt">Record Id:</td>
                <td>
                  <xsl:value-of select="RecordId" />
                </td>
              </tr>
            </xsl:if>
            <xsl:if test="string-length( stRecordDate ) > 0">
              <tr>
                <td class="Prompt">Record Date:</td>
                <td>
                  <xsl:value-of select="stRecordDate" />
                </td>
              </tr>
            </xsl:if>
            <xsl:if test="string-length( StateDesc ) > 0">
              <tr>
                <td class="Prompt">Status:</td>
                <td>
                  <xsl:value-of select="StateDesc" />
                </td>
              </tr>
            </xsl:if>
          </table>
        </td>
      </tr>
      <tr>
        <td colspan="2"  >
          <xsl:if test="string-length( Design/Approval ) > 0">
            <p class='para' style="text-align:center;font-size:smaller;" >
              <xsl:value-of select="Design/Approval" />
            </p>
          </xsl:if>
          <xsl:if test="string-length( Design/Instructions ) > 0 " >
            <p class='para' style='text-align:center;'>
              <xsl:value-of select="Design/Instructions" />
            </p>
          </xsl:if>
          <xsl:if test="string-length( FormMessage ) > 0">
            <p class='para' style='text-align:center;font-weight: bold;'>
              <xsl:value-of select="FormMessage" />
            </p>
          </xsl:if>
        </td>
      </tr>
    </table>
  </xsl:template>

  <!-- 
  
  ==================================  Form Footer ================================== 
  
  -->


  <xsl:template name="FormFooter">
    <xsl:param name="FormTypeId"></xsl:param>

    <xsl:if test ="$FormTypeId = 'Subject_Record'">
      <tr>
        <td class='evFsFormSection black' colspan='2'>
          Standard Fields
        </td>
      </tr>
      <tr>
        <td class="EvFooterPrompt">
          Subject status:
        </td>
        <td>
          <xsl:value-of select="SubjectState" />
        </td>
      </tr>
      <tr>
        <td class="EvFooterPrompt">
          Trial Exit Date:
        </td>
        <td>
          <xsl:value-of select="stLastVisitDate" />
        </td>
      </tr>
      <xsl:if test="string-length( EarlyWithdrawal ) > 0">
        <tr>
          <td class="EvFooterPrompt">
            Early withdrawal:
          </td>
          <td>
            <xsl:value-of select="EarlyWithdrawal" />
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="string-length( SignoffStatement ) > 0">
        <tr>
          <td class="EvFooterPrompt">
            Signoff:
          </td>
          <td>
            <xsl:value-of select="SignoffStatement" />
          </td>
        </tr>
      </xsl:if>
    </xsl:if>

    <tr>
      <td class='evFsFormSection black' colspan='2'>
        Comments
      </td>
    </tr>
    <xsl:choose>
      <xsl:when test="CommentList/EvFormRecordComment">
      <xsl:for-each select="CommentList/EvFormRecordComment">
        <xsl:if test="string-length(Content) > 0">
          <tr>
            <td class='EvFooterPrompt'>
            </td>
            <td>
              <xsl:value-of select="Content" /> by
              <xsl:value-of select="UserCommonName" /> on
              <xsl:value-of select="CommentDate" />
            </td>
          </tr>
        </xsl:if>
      </xsl:for-each>
      <tr>
        <td colspan="2">
          <p>Not Comments</p>
        </td>
      </tr>
      </xsl:when>
      <xsl:otherwise>
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
      </xsl:otherwise>
      </xsl:choose>

    <tr>
      <td class='evFsFormSection black' colspan='2'>
        Signoffs
      </td>
    </tr>
    <xsl:choose>
      <xsl:when test="RecordContent/Signoffs/EvUserSignoff">
        <xsl:for-each select="RecordContent/Signoffs/EvUserSignoff">
          <xsl:if test="string-length(SignedOffBy) > 0">
            <tr>
              <td class='EvFooterPrompt'>
              </td>
              <td >
                <xsl:value-of select="stDescription" /> by
                <xsl:value-of select="SignedOffBy" /> on
                <xsl:value-of select="stSignOffDate" />
              </td>
            </tr>
          </xsl:if>
        </xsl:for-each>
      </xsl:when>
      <xsl:otherwise>
        <tr>
          <td colspan="2">
            <p>Not Signsffs</p>
          </td>
        </tr>
      </xsl:otherwise>
    </xsl:choose>
    <tr>
      <td colspan="2">
        <hr />
      </td>
    </tr>
  </xsl:template>
  <!--

  -->
  <xsl:template match="text()"></xsl:template>

</xsl:stylesheet>
