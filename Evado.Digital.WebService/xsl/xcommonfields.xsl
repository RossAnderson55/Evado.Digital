<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">

  <!--
*******************************************************************************
*                COPYRIGHT (C) EVADO HOLDING Pty. Ltd.  2000 - 2016
*
*                            ALL RIGHTS RESERVED
*
*******************************************************************************
-->
  <!-- 
  
  =================================  Subject Form Fields    ================================ 
  
  -->
  <xsl:template name="SubjectFields">

    <xsl:param name="FormTypeId"></xsl:param>

    <xsl:if test ="$FormTypeId = 'Subject_Record'">
      <tr>
        <td class='evFsFormSection black' colspan='2'>
          Standard Demographics Fields
        </td>
      </tr>
      <tr>
        <td class="evFormPromptColumn">
          <p Class="FieldPrompt">
            <Strong>Subject Id:</Strong>
          </p>
        </td>
        <td class="evFormFieldColumn">
          <xsl:value-of select="SubjectId" />

          <xsl:if test ="string-length( ParticipantId ) > 0" >
            <xsl:text>  </xsl:text> ParticipantId: <xsl:value-of select="ParticipantId" />
          </xsl:if>
        </td>
      </tr>
      <tr>
        <td colspan="2">
          <hr />
        </td>
      </tr>
      <xsl:if test ="contains(Design/HiddenFields,'ScreeningId')=false" >
        <tr>
          <td class="evFormPromptColumn">
            <p Class="FieldPrompt">
              <Strong>Trial Arm:</Strong>
            </p>
          </td>
          <td class="evFormFieldColumn">
            <xsl:value-of select="Arm/Name" />
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
      </xsl:if>
      <xsl:if test ="contains(Design/HiddenFields,'ScreeningId')=false" >
        <tr>
          <td class="evFormPromptColumn">
            <xsl:call-template name="StaticFieldPrompt">
              <xsl:with-param name="FieldPrompt">
                <xsl:text>Screening Id:</xsl:text>
              </xsl:with-param>
              <xsl:with-param name="FieldMandatory">
                <xsl:text>false</xsl:text>
              </xsl:with-param>
            </xsl:call-template>

            <xsl:if test= "string-length(./RecordContent/ScreeningId/CommentList/EvFormRecordComment[1]) > 0 " >
              <p>
                <strong>Annotation:</strong>
                <br/>
                <xsl:for-each select="//RecordContent/ScreeningId/CommentList/EvFormRecordComment">
                  <xsl:if test="string-length(Content) > 0">
                    <xsl:value-of select="Content" /> by
                    <xsl:value-of select="UserCommonName" /> on
                    <xsl:value-of select="CommentDate" />
                    <br/>
                  </xsl:if>
                </xsl:for-each>
              </p>
            </xsl:if>

          </td>
          <td class="evFormFieldColumn">
            <xsl:value-of select="ScreeningId" />
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
      </xsl:if>

      <xsl:if test ="contains( //Design/HiddenFields, 'SponsorId')=false" >
        <tr>
          <td class="evFormPromptColumn">
            <xsl:call-template name="StaticFieldPrompt">
              <xsl:with-param name="FieldPrompt">
                <xsl:text>Sponsor Id:</xsl:text>
              </xsl:with-param>
              <xsl:with-param name="FieldMandatory">
                <xsl:text>false</xsl:text>
              </xsl:with-param>
            </xsl:call-template>

            <xsl:if test= "string-length(./RecordContent/SponsorId/CommentList/EvFormRecordComment[1]) > 0 " >
              <p>
                <strong>Annotation:</strong>
                <br/>
                <xsl:for-each select="//RecordContent/SponsorId/CommentList/EvFormRecordComment">
                  <xsl:if test="string-length(Content) > 0">
                    <xsl:value-of select="Content" /> by
                    <xsl:value-of select="UserCommonName" /> on
                    <xsl:value-of select="CommentDate" />
                    <br/>
                  </xsl:if>
                </xsl:for-each>
              </p>
            </xsl:if>

          </td>
          <td class="evFormFieldColumn">
            <xsl:value-of select="SponsorId" />
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
      </xsl:if>
      <xsl:if test ="contains( //Design/HiddenFields, 'ExternalId') = false " >
        <tr>
          <td class="evFormPromptColumn">
            <xsl:call-template name="StaticFieldPrompt">
              <xsl:with-param name="FieldPrompt">
                <xsl:text>External Id:</xsl:text>
              </xsl:with-param>
              <xsl:with-param name="FieldMandatory">
                <xsl:text>false</xsl:text>
              </xsl:with-param>
            </xsl:call-template>

            <xsl:if test= "string-length(./RecordContent/ExternalId/CommentList/EvFormRecordComment[1]) > 0 " >
              <p>
                <strong>Annotation:</strong>
                <br/>
                <xsl:for-each select="//RecordContent/ExternalId/CommentList/EvFormRecordComment">
                  <xsl:if test="string-length(Content) > 0">
                    <xsl:value-of select="Content" /> by
                    <xsl:value-of select="UserCommonName" /> on
                    <xsl:value-of select="CommentDate" />
                    <br/>
                  </xsl:if>
                </xsl:for-each>
              </p>
            </xsl:if>

          </td>
          <td class="evFormFieldColumn">
            <xsl:value-of select="ExternalId" />
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
      </xsl:if>
      <xsl:if test ="contains( //Design/HiddenFields, 'RandomisedId') = false " >
        <tr>
          <td class="evFormPromptColumn">
            <xsl:call-template name="StaticFieldPrompt">
              <xsl:with-param name="FieldPrompt">
                <xsl:text>Randomised Id:</xsl:text>
              </xsl:with-param>
              <xsl:with-param name="FieldMandatory">
                <xsl:text>false</xsl:text>
              </xsl:with-param>
            </xsl:call-template>

            <xsl:if test= "string-length(./RecordContent/RandomisedId/CommentList/EvFormRecordComment[1]) > 0 " >
              <p>
                <strong>Annotation:</strong>
                <br/>
                <xsl:for-each select="//RecordContent/RandomisedId/CommentList/EvFormRecordComment">
                  <xsl:if test="string-length(Content) > 0">
                    <xsl:value-of select="Content" /> by
                    <xsl:value-of select="UserCommonName" /> on
                    <xsl:value-of select="CommentDate" />
                    <br/>
                  </xsl:if>
                </xsl:for-each>
              </p>
            </xsl:if>

          </td>
          <td class="evFormFieldColumn">
            <xsl:value-of select="RandomisedId" />
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
      </xsl:if>
      <xsl:if test ="contains( //Design/HiddenFields, 'NickName') = false " >
        <tr>
          <td class="evFormPromptColumn">
            <xsl:call-template name="StaticFieldPrompt">
              <xsl:with-param name="FieldPrompt">
                <xsl:text>Nick name:</xsl:text>
              </xsl:with-param>
              <xsl:with-param name="FieldMandatory">
                <xsl:text>false</xsl:text>
              </xsl:with-param>
            </xsl:call-template>

            <xsl:if test= "string-length(./RecordContent/NickName/CommentList/EvFormRecordComment[1]) > 0 " >
              <p>
                <strong>Annotation:</strong>
                <br/>
                <xsl:for-each select="//RecordContent/NickName/CommentList/EvFormRecordComment">
                  <xsl:if test="string-length(Content) > 0">
                    <xsl:value-of select="Content" /> by
                    <xsl:value-of select="UserCommonName" /> on
                    <xsl:value-of select="CommentDate" />
                    <br/>
                  </xsl:if>
                </xsl:for-each>
              </p>
            </xsl:if>
          </td>
          <td class="evFormFieldColumn">
            <xsl:value-of select="NickName" />
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
      </xsl:if>
      <tr>
        <td class="evFormPromptColumn">
          <xsl:call-template name="StaticFieldPrompt">
            <xsl:with-param name="FieldPrompt">
              <xsl:text>Sex:</xsl:text>
            </xsl:with-param>
            <xsl:with-param name="FieldMandatory">
              <xsl:text>true</xsl:text>
            </xsl:with-param>
          </xsl:call-template>

          <xsl:if test= "string-length(./RecordContent/Sex/CommentList/EvFormRecordComment[1]) > 0 " >
            <p>
              <strong>Annotation:</strong>
              <br/>
              <xsl:for-each select="//RecordContent/Sex/CommentList/EvFormRecordComment">
                <xsl:if test="string-length(Content) > 0">
                  <xsl:value-of select="Content" /> by
                  <xsl:value-of select="UserCommonName" /> on
                  <xsl:value-of select="CommentDate" />
                  <br/>
                </xsl:if>
              </xsl:for-each>
            </p>
          </xsl:if>
        </td>
        <td class="evFormFieldColumn">
          <xsl:value-of select="SexDesc" />
        </td>
      </tr>
      <tr>
        <td colspan="2">
          <hr />
        </td>
      </tr>
      <tr>
        <td class="evFormPromptColumn">
          <xsl:call-template name="StaticFieldPrompt">
            <xsl:with-param name="FieldPrompt">
              <xsl:text>Birth Date:</xsl:text>
            </xsl:with-param>
            <xsl:with-param name="FieldMandatory">
              <xsl:text>true</xsl:text>
            </xsl:with-param>
          </xsl:call-template>

          <xsl:if test= "string-length(./RecordContent/DateOfBirth/CommentList/EvFormRecordComment[1]) > 0 " >
            <p>
              <strong>Annotation:</strong>
              <br/>
              <xsl:for-each select="//RecordContent/DateOfBirth/CommentList/EvFormRecordComment">
                <xsl:if test="string-length(Content) > 0">
                  <xsl:value-of select="Content" /> by
                  <xsl:value-of select="UserCommonName" /> on
                  <xsl:value-of select="CommentDate" />
                  <br/>
                </xsl:if>
              </xsl:for-each>
            </p>
          </xsl:if>
        </td>
        <td class="evFormFieldColumn">
          <xsl:value-of select="stDateOfBirth" />
        </td>
      </tr>
      <tr>
        <td colspan="2">
          <hr />
        </td>
      </tr>
      <tr>
        <td class="evFormPromptColumn">
          <xsl:call-template name="StaticFieldPrompt">
            <xsl:with-param name="FieldPrompt">
              <xsl:text>Age:</xsl:text>
            </xsl:with-param>
            <xsl:with-param name="FieldMandatory">
              <xsl:text>false</xsl:text>
            </xsl:with-param>
          </xsl:call-template>
        </td>
        <td class="evFormFieldColumn">
          <xsl:value-of select="Age" /> (yrs)
        </td>
      </tr>

      <xsl:if test ="contains( //Design/HiddenFields, 'Height') = false " >
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
        <tr>
          <td class="evFormPromptColumn">
            <xsl:call-template name="StaticFieldPrompt">
              <xsl:with-param name="FieldPrompt">
                <xsl:text>Height:</xsl:text>
              </xsl:with-param>
              <xsl:with-param name="FieldMandatory">
                <xsl:text>false</xsl:text>
              </xsl:with-param>
            </xsl:call-template>

            <xsl:if test= "string-length(./RecordContent/Height/CommentList/EvFormRecordComment[1]) > 0 " >
              <p>
                <strong>Annotation:</strong>
                <br/>
                <xsl:for-each select="//RecordContent/Height/CommentList/EvFormRecordComment">
                  <xsl:if test="string-length(Content) > 0">
                    <xsl:value-of select="Content" /> by
                    <xsl:value-of select="UserCommonName" /> on
                    <xsl:value-of select="CommentDate" />
                    <br/>
                  </xsl:if>
                </xsl:for-each>
              </p>
            </xsl:if>
          </td>
          <td class="evFormFieldColumn">
            <xsl:value-of select="Height" /> (cm)
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
      </xsl:if>

      <xsl:if test ="contains( //Design/HiddenFields, 'Weight') = false " >
        <tr>
          <td class="evFormPromptColumn">
            <xsl:call-template name="StaticFieldPrompt">
              <xsl:with-param name="FieldPrompt">
                <xsl:text>Weight:</xsl:text>
              </xsl:with-param>
              <xsl:with-param name="FieldMandatory">
                <xsl:text>false</xsl:text>
              </xsl:with-param>
            </xsl:call-template>

            <xsl:if test= "string-length(./RecordContent/Weight/CommentList/EvFormRecordComment[1]) > 0 " >
              <p>
                <strong>Annotation:</strong>
                <br/>
                <xsl:for-each select="//RecordContent/Weight/CommentList/EvFormRecordComment">
                  <xsl:if test="string-length(Content) > 0">
                    <xsl:value-of select="Content" /> by
                    <xsl:value-of select="UserCommonName" /> on
                    <xsl:value-of select="CommentDate" />
                    <br/>
                  </xsl:if>
                </xsl:for-each>
              </p>
            </xsl:if>
          </td>
          <td class="evFormFieldColumn">
            <xsl:value-of select="Weight" /> (Kg)
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
      </xsl:if>

      <xsl:if test ="contains( //Design/HiddenFields, 'BMI') = false " >
        <tr>
          <td class="evFormPromptColumn">
            <xsl:call-template name="StaticFieldPrompt">
              <xsl:with-param name="FieldPrompt">
                <xsl:text>BMI:</xsl:text>
              </xsl:with-param>
              <xsl:with-param name="FieldMandatory">
                <xsl:text>false</xsl:text>
              </xsl:with-param>
            </xsl:call-template>
          </td>
          <td class="evFormFieldColumn">
            <xsl:value-of select="stBmi" />
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
      </xsl:if>

      <xsl:if test ="contains( //Design/HiddenFields, 'History') = false " >
        <tr>
          <td class="evFormPromptColumn" colspan="2">
            <xsl:call-template name="StaticFieldPrompt">
              <xsl:with-param name="FieldPrompt">
                <xsl:text>Medical History:</xsl:text>
              </xsl:with-param>
              <xsl:with-param name="FieldMandatory">
                <xsl:text>false</xsl:text>
              </xsl:with-param>
            </xsl:call-template>

            <xsl:if test= "string-length(./RecordContent/History/CommentList/EvFormRecordComment[1]) > 0 " >
              <p>
                <strong>Annotation:</strong>
                <br/>
                <xsl:for-each select="//RecordContent/History/CommentList/EvFormRecordComment">
                  <xsl:if test="string-length(Content) > 0">
                    <xsl:value-of select="Content" /> by
                    <xsl:value-of select="UserCommonName" /> on
                    <xsl:value-of select="CommentDate" />
                    <br/>
                  </xsl:if>
                </xsl:for-each>
              </p>
            </xsl:if>
          </td>
        </tr>
        <tr>
          <td class="evFormFieldColumn" colspan="2">
            <xsl:for-each select="HistoryPara/string">
              <p class="FieldDisp">
                <xsl:value-of select="." />
              </p>
            </xsl:for-each>
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
      </xsl:if>

      <xsl:if test ="string-length( Design/Categories/string) > 0 " >
        <tr>
          <td class="evFormPromptColumn">
            <xsl:call-template name="StaticFieldPrompt">
              <xsl:with-param name="FieldPrompt">
                <xsl:text>Non clinical categories:</xsl:text>
              </xsl:with-param>
              <xsl:with-param name="FieldMandatory">
                <xsl:text>false</xsl:text>
              </xsl:with-param>
            </xsl:call-template>

            <xsl:if test= "string-length(./RecordContent/Categories/CommentList/EvFormRecordComment[1]) > 0 " >
              <p>
                <strong>Annotation:</strong>
                <br/>
                <xsl:for-each select="//RecordContent/Categories/CommentList/EvFormRecordComment">
                  <xsl:if test="string-length(Content) > 0">
                    <xsl:value-of select="Content" /> by
                    <xsl:value-of select="UserCommonName" /> on
                    <xsl:value-of select="CommentDate" />
                    <br/>
                  </xsl:if>
                </xsl:for-each>
              </p>
            </xsl:if>
          </td>
          <td class="evFormFieldColumn">
            <xsl:variable name="sCategories" select="Categories" />
            <ul >
              <xsl:for-each select="Design/Categories/string">
                <li>
                  <xsl:text> </xsl:text>
                  <xsl:if test="contains($sCategories, . )">
                    <strong>
                      <xsl:value-of select="." />
                    </strong>
                  </xsl:if>
                  <xsl:if test="contains($sCategories, . ) = false">
                    <xsl:text> </xsl:text>
                    <xsl:value-of select="." />
                  </xsl:if>
                </li>
              </xsl:for-each>
            </ul>
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
      </xsl:if>

      <xsl:if test ="string-length( Design/Diseases/string) > 0 " >
        <tr>
          <td class="evFormPromptColumn">
            <xsl:call-template name="StaticFieldPrompt">
              <xsl:with-param name="FieldPrompt">
                <xsl:text>Disease history:</xsl:text>
              </xsl:with-param>
              <xsl:with-param name="FieldMandatory">
                <xsl:text>false</xsl:text>
              </xsl:with-param>
            </xsl:call-template>

            <xsl:if test= "string-length(./RecordContent/Diseases/CommentList/EvFormRecordComment[1]) > 0 " >
              <p>
                <strong>Annotation:</strong>
                <br/>
                <xsl:for-each select="//RecordContent/Diseases/CommentList/EvFormRecordComment">
                  <xsl:if test="string-length(Content) > 0">
                    <xsl:value-of select="Content" /> by
                    <xsl:value-of select="UserCommonName" /> on
                    <xsl:value-of select="CommentDate" />
                    <br/>
                  </xsl:if>
                </xsl:for-each>
              </p>
            </xsl:if>
          </td>
          <td class="evFormFieldColumn">
            <xsl:variable name="sDiseases" select="Diseases" />
            <ul>
              <xsl:for-each select="Design/Diseases/string">
                <li>
                  <xsl:text> </xsl:text>
                  <xsl:if test="contains($sDiseases, . )">
                    <strong>
                      <xsl:value-of select="." />
                    </strong>
                  </xsl:if>
                  <xsl:if test="contains($sDiseases, . ) = false">
                    <xsl:text> </xsl:text>
                    <xsl:value-of select="." />
                  </xsl:if>
                </li>
              </xsl:for-each>
            </ul>
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
      </xsl:if>

      <xsl:if test ="string-length( Design/TrialDiseases/string) > 0 " >
        <tr>
          <td class="evFormPromptColumn">
            <xsl:call-template name="StaticFieldPrompt">
              <xsl:with-param name="FieldPrompt">
                <xsl:text>Trial specific disease history:</xsl:text>
              </xsl:with-param>
              <xsl:with-param name="FieldMandatory">
                <xsl:text>false</xsl:text>
              </xsl:with-param>
            </xsl:call-template>

            <xsl:if test= "string-length(./RecordContent/TrialDiseases/CommentList/EvFormRecordComment[1]) > 0 " >
              <p>
                <strong>Annotation:</strong>
                <br/>
                <xsl:for-each select="//RecordContent/TrialDiseases/CommentList/EvFormRecordComment">
                  <xsl:if test="string-length(Content) > 0">
                    <xsl:value-of select="Content" /> by
                    <xsl:value-of select="UserCommonName" /> on
                    <xsl:value-of select="CommentDate" />
                    <br/>
                  </xsl:if>
                </xsl:for-each>
              </p>
            </xsl:if>
          </td>
          <td class="evFormFieldColumn">
            <xsl:variable name="sDiseases" select="Diseases" />
            <ul>
              <xsl:for-each select="Design/TrialDiseases/string">
                <li>
                  <xsl:text> </xsl:text>
                  <xsl:if test="contains($sDiseases, . )">
                    <strong>
                      <xsl:value-of select="." />
                    </strong>
                  </xsl:if>
                  <xsl:if test="contains($sDiseases, . ) = false">
                    <xsl:text> </xsl:text>
                    <xsl:value-of select="." />
                  </xsl:if>
                </li>
              </xsl:for-each>
            </ul>
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
      </xsl:if>

      <tr>
        <td class="evFormPromptColumn">
          <xsl:call-template name="StaticFieldPrompt">
            <xsl:with-param name="FieldPrompt">
              <xsl:text>Consent Date:</xsl:text>
            </xsl:with-param>
            <xsl:with-param name="FieldMandatory">
              <xsl:text>true</xsl:text>
            </xsl:with-param>
          </xsl:call-template>

          <xsl:if test= "string-length(./RecordContent/ConsentDate/CommentList/EvFormRecordComment[1]) > 0 " >
            <p>
              <strong>Annotation:</strong>
              <br/>
              <xsl:for-each select="//RecordContent/ConsentDate/CommentList/EvFormRecordComment">
                <xsl:if test="string-length(Content) > 0">
                  <xsl:value-of select="Content" /> by
                  <xsl:value-of select="UserCommonName" /> on
                  <xsl:value-of select="CommentDate" />
                  <br/>
                </xsl:if>
              </xsl:for-each>
            </p>
          </xsl:if>
        </td>
        <td class="evFormFieldColumn">
          <xsl:value-of select="stConsentDate" />
        </td>
      </tr>
      <tr>
        <td colspan="2">
          <hr />
        </td>
      </tr>

      <tr>
        <td class="evFormPromptColumn">
          <p Class="FieldPrompt">
            <Strong>Trial Site:</Strong>
          </p>
        </td>
        <td class="evFormFieldColumn">
          <xsl:value-of select="OrgId" />
        </td>
      </tr>
      <tr>
        <td colspan="2">
          <hr />
        </td>
      </tr>

      <tr>
        <td class="evFormPromptColumn">
          <p Class="FieldPrompt">
            <Strong>Subject Status:</Strong>
          </p>
        </td>
        <td class="evFormFieldColumn">
          <xsl:value-of select="SubjectState" />
        </td>
      </tr>
      <tr>
        <td colspan="2">
          <hr />
        </td>
      </tr>

      <xsl:if test= "string-length(stLastVisitDate) > 0 " >
        <tr>
          <td class="evFormPromptColumn">
            <p Class="FieldPrompt">
              <Strong>Last Visit Date:</Strong>
            </p>
          </td>
          <td class="evFormFieldColumn">
            <xsl:value-of select="stLastVisitDate" />
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <hr />
          </td>
        </tr>
      </xsl:if>

    </xsl:if>
  </xsl:template>
  <!-- 
  
  =================================  Common Form Fields    ================================ 
  
  -->
  <xsl:template name="CommonFormField">

    <xsl:param name="FormTypeId"></xsl:param>

    <xsl:if test ="$FormTypeId = 'Adverse_Event_Report' or $FormTypeId = 'Serious_Adverse_Event_Report' or $FormTypeId = 'Concomitant_Medication' or $FormTypeId = 'Dose_Limit_Toxicity'">

      <tr>
        <td class='evFsFormSection black' colspan='2'>
          Standard Fields
        </td>
      </tr>
      <tr>
        <td class="evFormPromptColumn">
          <xsl:call-template name="StaticFieldPrompt">
            <xsl:with-param name="FieldPrompt">
              <xsl:if test ="$FormTypeId = 'Concomitant_Medication'">
                <xsl:text>Medication:</xsl:text>
              </xsl:if>
              <xsl:if test ="$FormTypeId != 'Concomitant_Medication'">
                <xsl:text>Event:</xsl:text>
              </xsl:if>
            </xsl:with-param>
            <xsl:with-param name="FieldMandatory">
              <xsl:text>true</xsl:text>
            </xsl:with-param>
          </xsl:call-template>

          <xsl:if test= "string-length(//RecordContent/RecordSubject/CommentList/EvFormRecordComment[1]) > 0 " >
            <p>
              <strong>Annotation:</strong>
              <br/>
              <xsl:for-each select="//RecordContent/RecordSubject/CommentList/EvFormRecordComment">
                <xsl:if test="string-length(Content) > 0">
                  <xsl:value-of select="Content" /> by
                  <xsl:value-of select="UserCommonName" /> on
                  <xsl:value-of select="CommentDate" />
                  <br/>
                </xsl:if>
              </xsl:for-each>
            </p>
          </xsl:if>

        </td>
        <td class='evFormFieldColumn'>
          <p>
            <xsl:value-of select="RecordSubject" />
          </p>
        </td>
      </tr>
      <tr>
        <td colspan="2">
          <hr />
        </td>
      </tr>

      <tr>
        <td class="evFormPromptColumn">
          <xsl:call-template name="StaticFieldPrompt">
            <xsl:with-param name="FieldPrompt">
              <xsl:if test ="$FormTypeId = 'Concomitant_Medication'">
                <xsl:text>Commencent date:</xsl:text>
              </xsl:if>
              <xsl:if test ="$FormTypeId != 'Concomitant_Medication'">
                <xsl:text>Onset date:</xsl:text>
              </xsl:if>
            </xsl:with-param>
            <xsl:with-param name="FieldMandatory">
              <xsl:text>true</xsl:text>
            </xsl:with-param>
          </xsl:call-template>

          <xsl:if test= "string-length(//RecordContent/StartDate/CommentList/EvFormRecordComment[1]) > 0 " >
            <p>
              <strong>Annotation:</strong>
              <br/>
              <xsl:for-each select="//RecordContent/StartDate/CommentList/EvFormRecordComment">
                <xsl:if test="string-length(Content) > 0">
                  <xsl:value-of select="Content" /> by
                  <xsl:value-of select="UserCommonName" /> on
                  <xsl:value-of select="CommentDate" />
                  <br/>
                </xsl:if>
              </xsl:for-each>
            </p>
          </xsl:if>
        </td>
        <td class="evFormFieldColumn">
          <xsl:value-of select="stStartDate" />
        </td>
      </tr>
      <tr>
        <td colspan="2">
          <hr />
        </td>
      </tr>

      <tr>
        <td class="evFormPromptColumn">
          <xsl:call-template name="StaticFieldPrompt">
            <xsl:with-param name="FieldPrompt">
              <xsl:if test ="$FormTypeId = 'Concomitant_Medication'">
                <xsl:text>Completion date:</xsl:text>
              </xsl:if>
              <xsl:if test ="$FormTypeId != 'Concomitant_Medication'">
                <xsl:text>Resolved date:</xsl:text>
              </xsl:if>
            </xsl:with-param>
            <xsl:with-param name="FieldMandatory">
              <xsl:text>false</xsl:text>
            </xsl:with-param>
          </xsl:call-template>

          <xsl:if test= "string-length(//RecordContent/FinishDate/CommentList/EvFormRecordComment[1]) > 0 " >
            <p>
              <strong>Annotation:</strong>
              <br/>
              <xsl:for-each select="//RecordContent/FinishDate/CommentList/EvFormRecordComment">
                <xsl:if test="string-length(Content) > 0">
                  <xsl:value-of select="Content" /> by
                  <xsl:value-of select="UserCommonName" /> on
                  <xsl:value-of select="CommentDate" />
                  <br/>
                </xsl:if>
              </xsl:for-each>
            </p>
          </xsl:if>
        </td>
        <td class="evFormFieldColumn">
          <xsl:value-of select="stFinishDate" />
        </td>
      </tr>
      <tr>
        <td colspan="2">
          <hr />
        </td>
      </tr>

      <xsl:if test ="$FormTypeId = 'Concomitant_Medication'">
        <xsl:if test ="string-length( AeSelectionList) > 0 ">
          <tr>
            <td class="evFormPromptColumn">
              <xsl:call-template name="StaticFieldPrompt">
                <xsl:with-param name="FieldPrompt">
                  <xsl:text>Referenced AE:</xsl:text>
                </xsl:with-param>
                <xsl:with-param name="FieldMandatory">
                  <xsl:text>false</xsl:text>
                </xsl:with-param>
              </xsl:call-template>

              <xsl:if test= "string-length(//RecordContent/ReferenceId/CommentList/EvFormRecordComment[1]) > 0 " >
                <p>
                  <strong>Annotation:</strong>
                  <br/>
                  <xsl:for-each select="//RecordContent/ReferenceId/CommentList/EvFormRecordComment">
                    <xsl:if test="string-length(Content) > 0">
                      <xsl:value-of select="Content" /> by
                      <xsl:value-of select="UserCommonName" /> on
                      <xsl:value-of select="CommentDate" />
                      <br/>
                    </xsl:if>
                  </xsl:for-each>
                </p>
              </xsl:if>
            </td>
            <td class="evFormFieldColumn">
              <xsl:variable name="referenceId" select="ReferenceId" />
              <xsl:for-each select="AeSelectionList/SelectionOption">
                <xsl:if test="Value = $referenceId">
                  <xsl:value-of select="Description" />
                </xsl:if>
              </xsl:for-each>
            </td>
          </tr>
          <tr>
            <td colspan="2">
              <hr />
            </td>
          </tr>
        </xsl:if>
      </xsl:if>
    </xsl:if>

  </xsl:template>

  <!-- 
		
		Template for displaying the Field prompt content  
		
		-->
  <xsl:template name="StaticFieldPrompt">
    <xsl:param name="FieldPrompt" />
    <xsl:param name="FieldMandatory" />

    <p Class="FieldPrompt">
      <strong>
        <xsl:value-of select="$FieldPrompt" />
      </strong>

      <xsl:if test= "$FieldMandatory = 'true' ">
        <span class="Mandatory"> *</span>
      </xsl:if>
    </p>

  </xsl:template>

  <xsl:template match="text()"></xsl:template>
</xsl:stylesheet>