<?xml version="1.0" encoding="ISO-8859-1" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  >
  <!--
*******************************************************************************
*                COPYRIGHT (C) EVADO HOLDING Pty. Ltd.  2000 - 2010
*
*                            ALL RIGHTS RESERVED
*
*******************************************************************************
-->
  <!-- Includes 
  -->
  <xsl:include href="./xFormHeader.xsl" />
  <xsl:include href="./xFormFields.xsl" />
  <xsl:output method="html" omit-xml-declaration="yes" indent="yes" encoding="ISO-8859-1" />
  <!--

  
    ====================== CLINICAL REPORT FORM - Header SECTION ================= 
     
  -->
  <xsl:template match="EvForm">

    <table id="RR_MainTable">
      <xsl:call-template name="CIOMS_FormHeader">
      </xsl:call-template>

      <xsl:call-template name="CIOMS_ReactionInformation">
      </xsl:call-template>

      <xsl:call-template name="CIOMS_DrugInformation">
      </xsl:call-template>

      <xsl:call-template name="CIOMS_ConcomitantMedication">
      </xsl:call-template>

      <xsl:call-template name="CIOMS_DrugManufacturer">
      </xsl:call-template>
    </table>
    <!--
    -->
  </xsl:template>
  <!--
  
  ===================================  Form Header Template ===============================
  
   -->
  <xsl:template name="CIOMS_FormHeader">

    <tr>
      <td class="RR_Section" style="text-align:right; padding-right:5px;font-size:8pt;" >
        CIOMS FORM
      </td>
    </tr>
    <tr>
      <td>
        <table>
          <tr>
            <td class="RR_border" style="vertical-align: middle;text-align:center">
              SUSPECT ADVERSE REACTION REPORT
            </td>
            <td  class="RR_Width_50">
              <table class="RR_Table">
                <tr style="height:22pt;">
                  <td  class="RR_border" colspan="15"  style="color:white;">.</td>
                </tr>
                <tr style="height:22pt;">
                  <td class="RR_border" colspan="15" style="color:white;">.</td>
                </tr>
                <tr style="height:22pt;">
                  <td class="RR_border" style="width:200px;color:white;">
                    .
                  </td>
                  <td class="RR_border" style="width:30px;color:white;">
                    .
                  </td>
                  <td class="RR_border" style="width:30px;color:white;">
                    .
                  </td>
                  <td class="RR_border" style="width:30px;color:white;">
                    .
                  </td>
                  <td class="RR_border" style="width:30px;color:white;">
                    .
                  </td>
                  <td class="RR_border" style="width:30px;color:white;">
                    .
                  </td>
                  <td class="RR_border" style="width:30px;color:white;">
                    .
                  </td>
                  <td class="RR_border" style="width:30px;color:white;">
                    .
                  </td>
                  <td class="RR_border" style="width:30px;color:white;">
                    .
                  </td>
                  <td class="RR_border" style="width:30px;color:white;">
                    .
                  </td>
                  <td class="RR_border" style="width:30px;color:white;">
                    .
                  </td>
                  <td class="RR_border" style="width:30px;color:white;">
                    .
                  </td>
                  <td class="RR_border" style="width:30px;color:white;">
                    .
                  </td>
                  <td class="RR_border" style="width:30px;color:white;">
                    .
                  </td>
                  <td class="RR_border" style="width:30px;color:white;">
                    .
                  </td>
                </tr>
              </table>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </xsl:template>

  <!--
  
  ===================================  Reaction Information Template ===============================
  
   -->
  <xsl:template name="CIOMS_ReactionInformation">
    <tr>
      <td class="RR_Section" style="text-align:left" >
        I. REACTION INFORMATION
      </td>
    </tr>
    <tr>
      <td>
        <table class="RR_Table">
          <tr>
            <td class="RR_Width_70 RR_border" >
              <table class="RR_Table">
                <tr>
                  <td  class="RR_border" style="text-align:center;">
                    1. PATIENT INITIALS<br/>
                    (first,last)<br/><br/>

                  </td>
                  <td  class="RR_border" style="text-align:center;">
                    1a. COUNTRY<br/><br/>
                    <xsl:value-of select="RecordContent/Country"/>

                  </td>
                  <td  class="RR_border" style="text-align:center;">
                    2. DATE OF BIRTH<br/><br/>
                    <xsl:value-of select="Subject/stDateOfBirth"/>
                  </td>
                  <td  class="RR_border" style="text-align:center;">
                    2a. AGE<br/><br/>
                    <xsl:value-of select="//Subject/Age"/>
                  </td>
                  <td  class="RR_border" style="text-align:center;">
                    3. SEX<br/><br/>
                    <xsl:value-of select="//Subject/Sex"/>
                  </td>
                  <td  class="RR_border" style="text-align:center;">
                    4-6. REACTION ONSET<br/><br/>
                    <xsl:value-of select="//stStartDate"/>
                  </td>
                </tr>
              </table>
              <p class="RR_Content">7 + 13 DESCRIBE REACTION(S) (including relevant tests/lab data)</p>
              <xsl:call-template name="getFieldValue">
                <xsl:with-param name="FieldId">
                  <xsl:text>DESC</xsl:text>
                </xsl:with-param>
              </xsl:call-template>

            </td>
            <td class=" RR_border" >
              <p class="RR_Content">
                8-12 CHECK ALL APPROPRIATE<br/> TO ADVERSE REACTION
              </p>

              <xsl:call-template name="getFieldChecklistValue">
                <xsl:with-param name="FieldId">
                  <xsl:text>OC</xsl:text>
                </xsl:with-param>
              </xsl:call-template>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </xsl:template>

  <!--
  
  ===================================  Drug Information Template ===============================
  
   -->
  <xsl:template name="CIOMS_DrugInformation">
    <tr>
      <td class="RR_Section"  style="text-align:left" >
        II. SUSPECT DRUG(S) INFORMATION
      </td>
    </tr>
    <tr>
      <td colspan="2">
        <!--
        -->
        <table>
          <tr>
            <td class="RR_Width_70 RR_border " colspan="2">
              <p class="RR_Content">14. SUSPECT DRUG(S) (include generic name)</p>

              <xsl:call-template name="getFieldValue">
                <xsl:with-param name="FieldId">
                  <xsl:text>SUSDRUG</xsl:text>
                </xsl:with-param>
              </xsl:call-template>

            </td>
            <td class=" RR_border" >
              <table class="RR_Table" >
                <tr>
                  <td>
                    <p class="RR_Content">20. DID REACTION ABATE AFTER STOPPING DRUG?</p>

                    <xsl:call-template name="getFieldChecklistValue">
                      <xsl:with-param name="FieldId">
                        <xsl:text>DRAAS</xsl:text>
                      </xsl:with-param>
                    </xsl:call-template>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td class="RR_Width_35 RR_border ">
              <p class="RR_Content">15. DAILY DOSE(S)</p>

              <xsl:call-template name="getFieldValue">
                <xsl:with-param name="FieldId">
                  <xsl:text>DAILYDOSE</xsl:text>
                </xsl:with-param>
              </xsl:call-template>

            </td>
            <td class="RR_Width_35 RR_border ">
              <p class="RR_Content">16. ROUTE(S) OF ADMINISTRATION</p>

              <xsl:call-template name="getFieldValue">
                <xsl:with-param name="FieldId">
                  <xsl:text>ROUTE</xsl:text>
                </xsl:with-param>
              </xsl:call-template>

            </td>
            <td class=" RR_border" >
              <table class="RR_Table" >
                <tr>
                  <td>
                    <p class="RR_Content">21. DID REACTION REAPPEAR AFTER REINTRODUCTION?</p>

                    <xsl:call-template name="getFieldChecklistValue">
                      <xsl:with-param name="FieldId">
                        <xsl:text>DRRAR</xsl:text>
                      </xsl:with-param>
                    </xsl:call-template>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </xsl:template>

  <!--
  
  ===================================  Concomitant MedicationTemplate ===============================
  
   -->
  <xsl:template name="CIOMS_ConcomitantMedication">
    <tr>
      <td class="RR_Section" style="text-align:left"  >
        III. CONCOMITANT DRUGS AND HISTORY
      </td>
    </tr>
    <tr>
      <td >
        <table class="RR_Table">
          <tr>
            <td class=" RR_border ">
              <p class="RR_Content">22. CONCOMITANT DRUG(S) AND DATES OF ADMINISTRATION (exclude those used to treat reaction)</p>
              <xsl:call-template name="getFieldValue">
                <xsl:with-param name="FieldId">
                  <xsl:text>ConMeds</xsl:text>
                </xsl:with-param>
              </xsl:call-template>
              <p class="RR_blnk">.</p>

            </td>
          </tr>
        </table>
      </td>
    </tr>
    <tr>
      <td >
        <table class="RR_Table">
          <tr>
            <td class="RR_border ">
              <p class="RR_Content">23. OTHER RELEVANT HISTORY (e.g. diagnostics, allergics, pregnancy with last month of period, etc.)</p>

              <xsl:call-template name="getFieldValue">
                <xsl:with-param name="FieldId">
                  <xsl:text>OtherHist</xsl:text>
                </xsl:with-param>
              </xsl:call-template>
              <p class="RR_blnk"> </p>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </xsl:template>

  <!--
  
  ===================================  Drug Manufacturer Template ===============================
  
   -->
  <xsl:template name="CIOMS_DrugManufacturer">
    <tr>
      <td class="RR_Section" style="text-align:left"  >
        IV. MANUFACTURER INFORMATION
      </td>
    </tr>
    <tr>
      <td>
        <table class="RR_Table">
          <tr>
            <td class="RR_Width_50 " >
              <table class="RR_Table">
                <tr>
                  <td class="RR_border" colspan="2">
                    <p class="RR_Content">24a. NAME AND ADDRESS OF MANUFACTURER</p>

                    <xsl:call-template name="getFieldValue">
                      <xsl:with-param name="FieldId">
                        <xsl:text>ManAddress</xsl:text>
                      </xsl:with-param>
                    </xsl:call-template>
                    <p class="RR_blnk"> </p>
                  </td>
                </tr>
                <tr>
                  <td  class="RR_Width_50 RR_border" >
                    <p class="RR_blnk">.</p>
                  </td>
                  <td  class="RR_border" >
                    <p class="RR_Content">24b. MFR CONTROL NO.</p>

                    <xsl:call-template name="getFieldValue">
                      <xsl:with-param name="FieldId">
                        <xsl:text>MfrConNo</xsl:text>
                      </xsl:with-param>
                    </xsl:call-template>
                    <p class="RR_blnk"> </p>
                  </td>
                </tr>
                <tr>
                  <td  class="RR_border" >
                    <p class="RR_Content">24c. DATE RECEIVED BY MANUFACTURER</p>

                    <xsl:call-template name="getFieldValue">
                      <xsl:with-param name="FieldId">
                        <xsl:text>OtherHist</xsl:text>
                      </xsl:with-param>
                    </xsl:call-template>
                    <p class="RR_blnk"> </p>
                  </td>
                  <td  class="RR_border">
                    <p class="RR_Content">24d. REPORT SOURCE</p>

                    <xsl:call-template name="getFieldValue">
                      <xsl:with-param name="FieldId">
                        <xsl:text>RptSource</xsl:text>
                      </xsl:with-param>
                    </xsl:call-template>
                    <p class="RR_blnk"> </p>
                  </td>
                </tr>
                <tr>
                  <td  class="RR_border">
                    <p class="RR_Content">DATE OF THIS REPORT</p>

                    <p class="RR_Content">
                      <xsl:value-of select="stRecordDate" />
                    </p>
                    <p class="RR_blnk"> </p>
                  </td>
                  <td  class="RR_border" >
                    <p class="RR_Content">25a. REPORT TYPE</p>

                    <xsl:call-template name="getFieldValue">
                      <xsl:with-param name="FieldId">
                        <xsl:text>RptType</xsl:text>
                      </xsl:with-param>
                    </xsl:call-template>
                    <p class="RR_blnk"> </p>
                  </td>
                </tr>
              </table>
            </td>
            <td class=" RR_border" >
              <p class="RR_blnk">.</p>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </xsl:template>

  <!--
  
  ===================================  get field value Templates ===============================
  
   -->

  <xsl:template name="getFieldValue">
    <xsl:param name="FieldId"></xsl:param>

    <xsl:for-each select="Fields/EvFormField">
      <xsl:if test="FieldId = $FieldId">
        <p class="RR_Content">
          <xsl:if test="string-length(ItemValue) > 0 ">
            <xsl:value-of select="ItemValue" />
          </xsl:if>
          <xsl:if test="string-length(ItemText) > 0 ">

            <xsl:for-each select="ItemFreeText/string">
              <xsl:value-of select="." />
              <br/>
            </xsl:for-each>
          </xsl:if>
        </p>
      </xsl:if>
    </xsl:for-each>

  </xsl:template>
  <!--
  
  The field chcklist display template.
  
  -->
  <xsl:template name="getFieldChecklistValue">
    <xsl:param name="FieldId"></xsl:param>

    <xsl:for-each select="Fields/EvFormField">
      <xsl:if test="FieldId = $FieldId">
        <xsl:variable name="value" select="ItemValue" />

        <xsl:if test="Design/OptionList/EvOption/OptionId">
          <table class="RR_Table" style="text-align: left;">
            <xsl:for-each select="Design/OptionList/EvOption">
              <xsl:if test="string-length(.)">
                <tr>
                  <td style="width:20px;">
                    <input type="Checkbox" Disabled="disabled" class="RR_field"  Value="{.}" >
                      <xsl:if test="contains($value, OptionId )">
                        <xsl:attribute name="checked">checked</xsl:attribute>
                      </xsl:if>
                    </input>
                  </td>
                  <td class="RR_Content" style="text-align:left;">
                    <xsl:text> </xsl:text>
                    <xsl:value-of select="Option" />
                  </td>
                </tr>
              </xsl:if>
            </xsl:for-each>
          </table>
        </xsl:if>
        <!-- 
      -->
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

  <!--
  
  ===================================  Form Header Template ===============================
  
   -->
  <!--

  -->
  <xsl:template match="text()"></xsl:template>

</xsl:stylesheet>
