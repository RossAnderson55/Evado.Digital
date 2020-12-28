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
  <!-- Includes 
  -->
  <xsl:include href="./xFormHeader.xsl" />
  <xsl:include href="./xFormFields.xsl" />
  <!--

  
    ====================== CLINICAL REPORT FORM - Header SECTION ================= 
     
  -->
  <xsl:template name="Subject">

    <fieldset class="evFsFormField no-border">
      <table class="evFormTable" >

        <xsl:call-template name="SubjectFields">
          <xsl:with-param name="FormTypeId">
            <xsl:value-of select="Design/TypeId" />
          </xsl:with-param>
          <xsl:with-param name="FormDisplayState">
            <xsl:value-of select="FormDisplayState" />
          </xsl:with-param>
        </xsl:call-template>

        <xsl:for-each select="Fields/EvFormField">

          <xsl:call-template name="SetSection" >
          </xsl:call-template>

          <xsl:call-template name="FormField" >
            <xsl:with-param name="FormTypeId">
              <xsl:value-of select="TypeId" />
            </xsl:with-param>
          </xsl:call-template>

        </xsl:for-each>

        <xsl:call-template name="FormFooter">
          <xsl:with-param name="FormTypeId">
            <xsl:value-of select="Design/TypeId" />
          </xsl:with-param>
        </xsl:call-template>

      </table>
    </fieldset>
    
  </xsl:template>

  <xsl:template name="EvFormRecord">
    <xsl:param name="RecordType"></xsl:param>

    <p class="BlankSpace">.</p>
    <div class="Page_Break_Before"></div>

    <xsl:choose>
      <xsl:when test ="position() = 1 and string-length($RecordType) > 0">
        <h2 class="Left" style="margin-left:10px">
          <xsl:value-of select="$RecordType" /></h2>
      </xsl:when>
      <xsl:otherwise>
        <p class="BlankSpace">.</p>
      </xsl:otherwise>
    </xsl:choose>

    <xsl:call-template name="FormHeader">
    </xsl:call-template>

    <fieldset class="evFsFormField no-border">
      <table class="evFormTable" >

        <xsl:call-template name="CommonFormField">
          <xsl:with-param name="FormTypeId">
            <xsl:value-of select="Design/TypeId" />
          </xsl:with-param>
          <xsl:with-param name="FormDisplayState">
            <xsl:value-of select="FormDisplayState" />
          </xsl:with-param>
        </xsl:call-template>


        <xsl:for-each select="Fields/EvFormField">

          <xsl:call-template name="SetSection" >
          </xsl:call-template>

          <xsl:call-template name="FormField" >
            <xsl:with-param name="FormTypeId">
              <xsl:value-of select="TypeId" />
            </xsl:with-param>
          </xsl:call-template>

        </xsl:for-each>
        <xsl:call-template name="FormFooter">
          <xsl:with-param name="FormTypeId">
            <xsl:value-of select="Design/TypeId" />
          </xsl:with-param>
        </xsl:call-template>

      </table>
    </fieldset>
  </xsl:template>

  <xsl:template match="text()"></xsl:template>

</xsl:stylesheet>
