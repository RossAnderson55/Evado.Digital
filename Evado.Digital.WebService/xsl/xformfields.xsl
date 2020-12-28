<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="3.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:fo="http://www.w3.org/1999/XSL/Format"  >

  <xsl:include href="./xCommonFields.xsl" />


  <!--
*******************************************************************************
*                COPYRIGHT (C) EVADO HOLDING Pty. Ltd.  2000 - 2016
*
*                            ALL RIGHTS RESERVED
*
*******************************************************************************
-->

  <!-- 
  
  =================================  Set form sections    ================================ 
  
  -->
  <xsl:template name="SetSection">

    <xsl:variable name="LastPos" select=" number( position())-1 " />
    <xsl:variable name="LastSection" select="../EvFormField[number($LastPos)]/Design/Section" />

    <xsl:if test ="string-length( Design/Section) > 0">
      <xsl:if test ="string($LastSection) != string( Design/Section)">
        <tr>
          <td class='evFsFormSection black' colspan='2'>
            <xsl:value-of select="Design/Section"/>
          </td>
        </tr>
      </xsl:if>
    </xsl:if>

  </xsl:template>


  <!-- 
  
  =================================  Form Fields    ================================ 
  
  -->
  <xsl:template name="FormField">
    
    <xsl:if test="TypeId='Text' or TypeId=''">
      <xsl:call-template name="TextField">
      </xsl:call-template>
    </xsl:if>
    
    <xsl:if test="TypeId='Computed_Field'">
      <xsl:call-template name="ComputedField">
      </xsl:call-template>
    </xsl:if>
    <xsl:if test="TypeId='Numeric'">
      <xsl:call-template name="NumericField">
      </xsl:call-template>
    </xsl:if>
    
    <xsl:if test="TypeId='Date'">
      <xsl:call-template name="DateField">
      </xsl:call-template>
    </xsl:if>
    
    <xsl:if test="TypeId='Time'">
      <xsl:call-template name="TimeField">
      </xsl:call-template>
    </xsl:if>
    
    <xsl:if test="TypeId='Yes_No'">
      <xsl:call-template name="YesNoField">
      </xsl:call-template>
    </xsl:if>
    
    <xsl:if test="TypeId='Free_Text'">
      <xsl:call-template name="FreeTextField">
      </xsl:call-template>
    </xsl:if>
    
    <xsl:if test="TypeId='Selection_List' or TypeId='External_Selection_List'">
      <xsl:call-template name="SelectionListField">
      </xsl:call-template>
    </xsl:if>
    
    <xsl:if test="TypeId='Check_Button_List'">
      <xsl:call-template name="CheckBoxListField">
      </xsl:call-template>
    </xsl:if>
    
    <xsl:if test="TypeId='Radio_Button_List'">
      <xsl:call-template name="RadioButtonListField">
      </xsl:call-template>
    </xsl:if>
    
    <xsl:if test="TypeId='Table' or TypeId='Matrix' or TypeId='Subject_Demographics' or TypeId='Medication_Summary' ">
      <xsl:call-template name="TableField">
      </xsl:call-template>
    </xsl:if>
    
    <tr>
      <td colspan="2">
        <hr />
      </td>
    </tr>
  </xsl:template>
  <!-- 
	
	template for Text field 
	
	-->
  <xsl:template name="TextField">
    <tr>
      <td class="evFormPromptColumn">
        <xsl:call-template name="FieldPrompt">
        </xsl:call-template>
      </td>
      <td class="evFormFieldColumn">
        <xsl:value-of select="ItemValue" />
      </td>
    </tr>
  </xsl:template>
  <!-- 
	
	template for Computer field 
	
	-->
  <xsl:template name="ComputedField">
    <tr>
      <td class="evFormPromptColumn">
        <xsl:call-template name="FieldPrompt">
        </xsl:call-template>
      </td>
      <td class="evFormFieldColumn">
        <xsl:value-of select="ItemValue" />
      </td>
    </tr>
  </xsl:template>
  <!-- 
	
	template for Numeric field 
	
	-->
  <xsl:template name="NumericField">

    <xsl:variable name ="fsClass" >
      <xsl:choose>
        <xsl:when test ="ValidationError = 'V'">
          <xsl:text>evFsFormFieldError</xsl:text>
        </xsl:when>
        <xsl:when test ="ValidationError = 'A'">
          <xsl:text>evFsFormFieldAlert</xsl:text>
        </xsl:when>
        <xsl:when test ="ValidationError = 'LNA'">
          <xsl:text>evFsFormFieldRange</xsl:text>
        </xsl:when>
        <xsl:when test ="ValidationError = 'S'">
          <xsl:text>evFsFormFieldSafety</xsl:text>
        </xsl:when>
        <xsl:when test ="State = 'Queried'">
          <xsl:text>css_FieldSetFieldQuery</xsl:text>
        </xsl:when>
        <xsl:otherwise>
          <xsl:text>evFsFormField</xsl:text>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <tr  class="{$fsClass}" >
      <td class="evFormPromptColumn" >
        <xsl:call-template name="FieldPrompt">
        </xsl:call-template>
      </td>
      <td class="evFormFieldColumn" >
        <xsl:value-of select="ItemValue" />

        <xsl:if test="Design/UnitScaling !='' and Design/UnitScaling !='1'">
          x10<span class='SuperScript'>
            <xsl:value-of select="Design/UnitScaling" />
          </span>
        </xsl:if>
        <xsl:text> </xsl:text>
        <xsl:value-of select="Design/Unit" />
      </td>
    </tr>
  </xsl:template>
  <!-- 
	
	template for Date field 
	
	-->
  <xsl:template name="DateField">
    <tr>
      <td class="evFormPromptColumn">
        <xsl:call-template name="FieldPrompt">
        </xsl:call-template>
      </td>
      <td class="evFormFieldColumn">
        <xsl:value-of select="ItemDate" />
      </td>
    </tr>
  </xsl:template>
  <!-- 
	
	template for Time field 
	
	-->
  <xsl:template name="TimeField">
    <tr>
      <td class="evFormPromptColumn">
        <xsl:call-template name="FieldPrompt">
        </xsl:call-template>
      </td>
      <td class="evFormFieldColumn">
        <xsl:value-of select="ItemDate" />
      </td>
    </tr>
  </xsl:template>
  <!-- 
	
	template for Yes/ No field 
	
	-->
  <xsl:template name="YesNoField">
    <tr>
      <td class="evFormPromptColumn">
        <xsl:call-template name="FieldPrompt">
        </xsl:call-template>
      </td>
      <td class="evFormFieldColumn">
        <xsl:value-of select="ItemValue" />
      </td>
    </tr>
  </xsl:template>
  <!-- 
	
template for free text field 	
	-->
  <xsl:template name="FreeTextField">
    <tr>
      <td class="evFormPromptColumn" colspan="2">
        <xsl:call-template name="FieldPrompt">
        </xsl:call-template>
      </td>
    </tr>
    <tr>
      <td class="evFormFieldColumn" colspan="2">
        <xsl:for-each select="ItemFreeText/string">
          <p class="FieldDisp" style="margin:10pt;">
            <xsl:value-of select="." />
          </p>
        </xsl:for-each>
      </td>
    </tr>
  </xsl:template>
  <!-- 
	
	============================== Selection List Field Template ===============================
	
	-->
  <xsl:template name="SelectionListField">
    <tr>
      <td class="evFormPromptColumn">
        <xsl:call-template name="FieldPrompt">
        </xsl:call-template>
      </td>

      <td class="evFormFieldColumn">
        <xsl:value-of select="ItemValue" />
      </td>
    </tr>
  </xsl:template>
  <!-- 
	
	============================== Radio Button List Field Template ===============================
	
	-->
  <xsl:template name="RadioButtonListField">
    <xsl:variable name="value" select="ItemValue" />
    <tr>
      <td class="evFormPromptColumn">
        <xsl:call-template name="FieldPrompt">
        </xsl:call-template>
      </td>
      <td class="evFormFieldColumn">
        <ul>
          <xsl:for-each select="Design/OptionList/EvOption">
            <li>
              <xsl:if test="Value=$value">
                <strong>
                  <xsl:value-of select="Description" />
                </strong>
              </xsl:if>
              <xsl:if test="Value!=$value">
                <xsl:value-of select="Description" />
              </xsl:if>
            </li>
          </xsl:for-each>
        </ul>
      </td>
    </tr>
  </xsl:template>

  <!-- 
	
	============================== Check Button List Field Template ===============================
	
	-->
  <xsl:template name="CheckBoxListField">
    <xsl:variable name="value" select="ItemValue" />
    <tr>
      <td class="evFormPromptColumn">
        <xsl:call-template name="FieldPrompt">
        </xsl:call-template>
      </td>
      <td class="evFormFieldColumn">
        <ul>
          <xsl:for-each select="Design/OptionList/EvOption">
            <xsl:if test="string-length(Value) > 0">
              <li>
                <xsl:if test="contains($value, Value )">
                  <strong>
                    <xsl:value-of select="Description" />
                  </strong>
                </xsl:if>
                <xsl:if test="Value!=$value">
                  <xsl:value-of select="Description" />
                </xsl:if>
              </li>
            </xsl:if>
          </xsl:for-each>
        </ul>
      </td>
    </tr>
  </xsl:template>
  <!-- 
	
	============================== Table  Field Template ===============================
	
	-->
  <xsl:template name="TableField">
    <xsl:variable name="sColumnCount" select="Table/ColumnCount" />
    <xsl:variable name="fieldId" select="FieldId" />
    <tr>
      <td class="evFormPromptColumn">
        <xsl:call-template name="FieldPrompt">
          <xsl:with-param name="FormDisplayState">
            <xsl:value-of select="$FormDisplayState" />
          </xsl:with-param>
        </xsl:call-template>
      </td>
      <td>
        <span class="BlankLine">.</span>
      </td>
    </tr>
    <tr>
      <td colspan="2">
        <table class="evFormDataTable" cellspacing='1px'  >
          <tr>
            <td class="TableHeader" style="Width:30px;">Row</td>
            <xsl:for-each select="Table/Header/EvFormFieldTableColumnHeader">
              <xsl:if test="string-length( Text ) > 0 ">
                <td class="TableHeader" style="width:{Width}%;" >

                  <xsl:choose>
                    <xsl:when test="position() = '1'">
                      <xsl:attribute name="style">
                        <xsl:text></xsl:text>
                      </xsl:attribute>
                    </xsl:when>
                    <xsl:when test="sColumnCount = '1'">
                      <xsl:attribute name="style">
                        <xsl:text>width:100%;</xsl:text>
                      </xsl:attribute>
                    </xsl:when>
                    <xsl:when test="sColumnCount = '2'">
                      <xsl:attribute name="style">
                        <xsl:text>width:45%;</xsl:text>
                      </xsl:attribute>
                    </xsl:when>
                    <xsl:when test="contains( Width, 'pt') or contains( Width, 'px')">
                      <xsl:attribute name="style">
                        <xsl:text>width:</xsl:text>
                        <xsl:value-of select="Width" />
                        <xsl:text>;</xsl:text>
                      </xsl:attribute>
                    </xsl:when>
                    <xsl:otherwise>
                    </xsl:otherwise>
                  </xsl:choose>

                  <xsl:if test="TypeId = 'D' or TypeId = 'N' or TypeId = 'Y'">
                    <xsl:attribute name="align">Center</xsl:attribute>
                  </xsl:if>
                  <strong>
                    <xsl:value-of select="Text" />
                  </strong>
                  <xsl:if test="TypeId = 'DT'">
                    <br />
                    <span class="Smaller_Italics">dd MMM YYYY</span>
                  </xsl:if>
                </td>
              </xsl:if>
            </xsl:for-each>
          </tr>
          <xsl:for-each select="Table/Rows/EvFormFieldTableRow">
            <xsl:variable name="row" select="position()" />
            <tr>
              <td>
                <xsl:value-of select="position()" />
              </td>
              <xsl:for-each select="Column/string">
                <xsl:variable name="column" select="position()" />
                <xsl:variable name="text" select="../../../../Header/EvFormFieldTableColumnHeader[$column]/Text" />
                <xsl:if test ="string-length( $text ) > 0 " >
                  <td  class="Smaller" >
                    <xsl:text> </xsl:text>
                    <xsl:value-of select="." />
                  </td>
                </xsl:if>
              </xsl:for-each>
            </tr>
          </xsl:for-each>
        </table>
      </td>
    </tr>
  </xsl:template>

  <!-- 
		
		Template for displaying the Field prompt content  
		
		-->
  <xsl:template name="FieldPrompt">
    <p Class="FieldPrompt">
      <strong>
        <xsl:value-of select="Design/Subject" />
      </strong>
      <xsl:if test= "Design/Mandatory = 'true' ">
        <span class="Mandatory"> *</span>
      </xsl:if>
    </p>

    <xsl:if test= "string-length( Design/InstructionPar/string[1]) > 0">
      <p>
        <xsl:for-each select="Design/InstructionPar/string">
          <xsl:if test="string-length(.) > 0">
            <xsl:value-of select="." />
            <br/>
          </xsl:if>
        </xsl:for-each>
      </p>
    </xsl:if>
    <xsl:if test= "not(Design/Reference = '') ">
      <p>
        <a href="{Design/Reference}" Target="_Blank">Reference</a>
      </p>
    </xsl:if>
    <xsl:if test="TypeId != 'Computed_Field'">
      <xsl:if test= "string-length( CommentList/EvFormRecordComment[1]) > 0 " >
        <p>
          <strong>Annotation:</strong>
          <br/>
          <xsl:for-each select="CommentList/EvFormRecordComment">
            <xsl:if test="string-length(Content) > 0">
              <xsl:value-of select="Content" /> by
              <xsl:value-of select="UserCommonName" /> on
              <xsl:value-of select="CommentDate" />
              <br/>
            </xsl:if>
          </xsl:for-each>
        </p>
      </xsl:if>
    </xsl:if>

  </xsl:template>

  <xsl:template match="text()"></xsl:template>
</xsl:stylesheet>
