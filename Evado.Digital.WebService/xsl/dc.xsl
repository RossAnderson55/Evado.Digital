<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  >
  <!--
xmlns:fo="http://www.w3.org/1999/XSL/Format"

*******************************************************************************
*                COPYRIGHT (C) EVADO HOLDING PTY. LTD.  2002 - 2015
*
*                            ALL RIGHTS RESERVED
*
*******************************************************************************

-->
<!-- Includes 
  -->
<xsl:output method="html" omit-xml-declaration="yes" indent="yes" encoding="ISO-8859-1" />
<!--
  
    ====================== CLINICAL REPORT FORM - HeadER SECTION ================= 
     
  -->
  <xsl:template match="ArrayOfEvDataChange">
    <xsl:for-each select="EvDataChange" >
      <xsl:call-template name="DataChange"></xsl:call-template>
    </xsl:for-each>
  </xsl:template>
  <!--  

 ============================   Call Trial Template        =========================
 
-->

  <!--
  
  =====================  TrialSubjectRecords Template =======================
  
  <xsl:template name="DataChanges">
    <xsl:for-each select="EvDataChange" >
      <xsl:call-template name="DataChange"></xsl:call-template>
    </xsl:for-each>
  </xsl:template>

   -->
  <!--
  
  =====================  TrialSubjectRecords Template =======================
  
   -->

  <xsl:template name="DataChange">

    <fieldset class="Fields">
      <legend>
        Change: <xsl:value-of select="position()" />
      </legend>
      <table>
        <xsl:if test="string-length( TrialId )">
          <tr>
            <td class="Prompt Width_20" >TrialId:</td>
            <td>
              <xsl:value-of select="TrialId" />
            </td>
          </tr>
        </xsl:if>
        <xsl:if test="string-length( SubjectId )">
          <tr>
            <td class="Prompt" >SubjectId:</td>
            <td>
              <xsl:value-of select="SubjectId" />
            </td>
          </tr>
        </xsl:if>
        <xsl:if test="string-length( RecordId )">
          <tr>
            <td class="Prompt" align="right">RecordId:</td>
            <td>
              <xsl:value-of select="RecordId" />
            </td>
          </tr>
        </xsl:if>
      </table>
      <table class="View_Header" style="width:98%" >
        <tr class="View_Header">
          <th style="width:50px;">Item</th>
          <th>ItemId</th>
          <th style="width:400px;">Initial Value</th>
          <th style="width:400px;">New Value</th>
        </tr>
        <xsl:for-each select="Items/EvDataChangeItem" >
          <xsl:if test ="ItemId != '' ">
            <tr class="View_Item">
              <td style="text-align:center;">
                <xsl:value-of select="position()" />
              </td>
              <td>
                <xsl:value-of select="ItemId" />
                <xsl:if test="ItemId = ''">
                  Null Data
                </xsl:if>
              </td>
              <td>
                <xsl:value-of select="InitialValue" />
                <xsl:if test="InitialValue = ''">
                  Null Data
                </xsl:if>
              </td>
              <td>
                <xsl:value-of select="NewValue" />
                <xsl:if test="NewValue = ''">
                  Null Data
                </xsl:if>
              </td>
            </tr>
          </xsl:if>
        </xsl:for-each>
      </table>
      <table>
        <tr>
          <td class="Prompt wIDTH_20" >UserId:</td>
          <td>
            <xsl:value-of select="UserId" />
          </td>
        </tr>
        <tr>
          <td class="Prompt" >DateStamp:</td>
          <td>
            <xsl:value-of select="stDateStamp" />
          </td>
        </tr>
      </table>
    </fieldset>
  </xsl:template>

  <!--
  <xsl:template match="text()"></xsl:template>
  
  -->
</xsl:stylesheet>
