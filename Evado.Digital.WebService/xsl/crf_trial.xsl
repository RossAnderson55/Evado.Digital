<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" 
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <!--
*******************************************************************************
*                COPYRIGHT (C) EVADO HOLDING Pty. Ltd.  2000 - 2016
*
*                            ALL RIGHTS RESERVED
*
*******************************************************************************
-->

  <xsl:include href="./crf_form.xsl" />
  <!--
  <xsl:include href="./crf_tr.xsl" />

  
  ============================  Clinical Trial Template =========================
  
   -->
  <xsl:template name="Trial">
    <h2 class="Left" style="margin-left:10px">Clinical Trial</h2>
    <fieldset class="Fields  no-border">
      <table>
        <xsl:if test="string-length( CroName )">
          <tr>
            <td class="Prompt Width_25">CRO:</td>
            <td>
              <xsl:value-of select="CroName" />
            </td>
          </tr>
        </xsl:if>
        <tr>
          <td  class="Prompt">Project Id:</td>
          <td>
            <xsl:value-of select="TrialId" />
          </td>
        </tr>
        <xsl:if test="string-length( Title )">
          <tr>
            <td class="Prompt">Title:</td>
            <td>
              <xsl:value-of select="Title" />
            </td>
          </tr>
        </xsl:if>
        <xsl:if test="string-length( Description/P )">
          <tr>
            <td class="Prompt">Description:</td>
            <td>
              <p >
                <xsl:for-each select="Description/P">
                  <xsl:value-of select="." />
                  <br/>
                </xsl:for-each>
              </p>
            </td>
          </tr>
        </xsl:if>
        <xsl:if test="string-length( Sponsor ) ">
          <tr>
            <td class="Prompt">Sponsor:</td>
            <td>
              <xsl:value-of select="Sponsor" />
            </td>
          </tr>
        </xsl:if>
        <xsl:if test="string-length( Subject) > 0">
          <tr>
            <td class="Prompt">Trial Subject:</td>
            <td>
              <xsl:value-of select="Subject" />
            </td>
          </tr>
        </xsl:if>
        <tr>
          <td  class="Prompt">Protocol Id:</td>
          <td>
            <xsl:value-of select="ProtocolId" />
          </td>
        </tr>
        <xsl:if test="string-length( Reference ) > 0">
          <tr>
            <td class="Prompt">Reference:</td>
            <td>
              <xsl:value-of select="Reference" />
            </td>
          </tr>
        </xsl:if>
        <xsl:if test="string-length( Initiated ) > 0">
          <tr>
            <td class="Prompt">Initiated: </td>
            <td>
              <xsl:value-of select="Initiated" />
            </td>
          </tr>
        </xsl:if>
        <xsl:if test="string-length( Started ) > 0">
          <tr>
            <td class="Prompt">Commenced: </td>
            <td>
              <xsl:value-of select="Started" />
            </td>
          </tr>
        </xsl:if>
        <xsl:if test="string-length( Closed ) > 0">
          <tr>
            <td class="Prompt">Completed: </td>
            <td>
              <xsl:value-of select="Closed" />
            </td>
          </tr>
        </xsl:if>
      </table>
    </fieldset>
  </xsl:template>
  
  <!--
  
  =====================  EvMilestone Template =======================
  
   -->
  <xsl:template name="EvMilestones">

    <xsl:variable name="visitId">
      <xsl:value-of select="VisitId" />
    </xsl:variable>
    
    <p class="BlankSpace">.</p>
    <div class="Page_Break_Before"></div>

    <xsl:choose>
      <xsl:when test ="position() = 1">
        <h2 class="Left" style="margin-left:10px">Subject Visits:</h2>
      </xsl:when>
      <xsl:otherwise>
        <p class="BlankSpace">.</p>
      </xsl:otherwise>
    </xsl:choose>


    <fieldset class="Fields  no-border">
      <table>
        <tr>
          <td class="Prompt Width_15">Title:</td>
          <td>
            <xsl:value-of select="Title" />
          </td>
        </tr>
        <tr>
          <td class="Prompt">VisitId:</td>
          <td>
            <xsl:value-of select="VisitId" />
          </td>
        </tr>
        <tr>
          <td class="Prompt">Date Time: </td>
          <td>
            <xsl:value-of select="stStartDate" />
          </td>
        </tr>
        <tr>
          <td class="Prompt">Status:</td>
          <td>
            <xsl:value-of select="State" />
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <xsl:if test="Activities/EvActivity">
              <p class='header3' style='Text-Align:left;'>
                Activities:
              </p>
              <table class="evFormDataTable" cellspacing='1px'>
                <tr>
                  <td class="TableHeader" >
                    ActivityId:
                  </td>
                  <td  class="TableHeader" >
                    Title:
                  </td>
                  <td  class="TableHeader" >
                    Completed By:
                  </td>
                  <td  class="TableHeader" >
                    Date:
                  </td>
                  <td class="TableHeader" >
                    Status:
                  </td>
                </tr>
                <xsl:for-each select="Activities/EvActivity" >
                  <tr>
                    <td>
                      <xsl:value-of select="ActivityId" />
                      <xsl:text> </xsl:text>
                    </td>
                    <td>
                      <xsl:value-of select="Title" />
                      <xsl:text> </xsl:text>
                    </td>
                    <td>
                      <xsl:value-of select="CompletedBy" />
                      <xsl:text> </xsl:text>
                    </td>
                    <td>
                      <xsl:value-of select="stCompletionDate" />
                      <xsl:text> </xsl:text>
                    </td>
                    <td>
                      <xsl:value-of select="StateDesc" />
                      <xsl:text> </xsl:text>
                    </td>
                  </tr>
                </xsl:for-each>
              </table>
            </xsl:if>
            <xsl:if test="Data/Signoffs/EvUserSignoff">
              <p class='header3' style='Text-Align:left;'>
                Visit Signoffs:
              </p>
              <table class="evFormDataTable" cellspacing='1px'>
                <tr>
                  <td class="TableHeader width_25">
                    Description:
                  </td>
                  <td class="TableHeader" >
                    Person:
                  </td>
                  <td class="TableHeader" >
                    Date:
                  </td>
                </tr>
                <xsl:for-each select="Data/Signoffs/EvUserSignoff" >
                  <tr>
                    <td>
                      <xsl:value-of select="stDescription" />
                    </td>
                    <td>
                      <xsl:value-of select="SignedOffBy" />
                    </td>
                    <td>
                      <xsl:value-of select="stSignOffDate" />
                    </td>
                  </tr>
                </xsl:for-each>
              </table>
            </xsl:if>
            <xsl:if test="Data/ProtocolViolations/EvProtocolViolation">
              <p class='header3' style='Text-Align:left;'>
                Protocol Violations:
              </p>
              <table class="evFormDataTable" cellspacing='1px'>
                <tr>
                  <td class="TableHeader" >
                    Type:
                  </td>
                  <td class="TableHeader" >
                    Comments:
                  </td>
                  <td class="TableHeader" >
                    Status:
                  </td>
                </tr>
                <xsl:for-each select="Data/ProtocolViolations/EvProtocolViolation" >
                  <tr>
                    <td>
                      <xsl:value-of select="stType" />
                      <xsl:text> </xsl:text>
                    </td>
                    <td>
                      <xsl:value-of select="CommentsXml" />
                      <xsl:text> </xsl:text>
                    </td>
                    <td>
                      <xsl:value-of select="StateDesc" />
                      <xsl:text> </xsl:text>
                    </td>
                  </tr>
                </xsl:for-each>
              </table>
            </xsl:if>
            <p align="left">
              <b>Comments:</b>
              <br/>
              <xsl:for-each select="Comments/P" >
                <xsl:value-of select="." />
                <br/>
              </xsl:for-each>
            </p>
          </td>
        </tr>
      </table>
    </fieldset>
    
    <!--
    Iterate through the record list displaying the records that are in the
    current visit.
    -->
    <xsl:for-each select="//CrfRecords/EvForm">
      
      <xsl:if test="VisitId = $visitId">
      <xsl:call-template name="EvFormRecord">
        <xsl:with-param name="RecordType">
          <xsl:text>Case Report Form:</xsl:text>
        </xsl:with-param>
        
      </xsl:call-template>
        
      </xsl:if>
      
    </xsl:for-each>
  </xsl:template>

  <!--
    ======================= OTHER TEMPLATES  ======================================
<xsl:template match="text()"></xsl:template>
  -->
</xsl:stylesheet>
