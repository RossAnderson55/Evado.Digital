/***************************************************************************************
 * <copyright file="Evado.Model.UniForm\EuArticle.cs" company="EVADO HOLDING PTY. LTD.">
 *     
 *      Copyright (c) 2013 - 2021 EVADO HOLDING PTY. LTD..  All rights reserved.
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
 *  This class contains the EuArticle data object.
 *
 ****************************************************************************************/
using System;
using Newtonsoft.Json;

namespace Evado.Model.UniForm
{
  /// <summary>
  /// This class defines the method parameter object structure.
  /// </summary>
  [Serializable]
  public class EuArticle
  {
    #region class initialisation methods

    //  =================================================================================
    /// <summary>
    /// This method initialises the class with parameter and value.
    /// </summary>
    //  ---------------------------------------------------------------------------------
    public EuArticle( )
    {
    }

    //  =================================================================================
    /// <summary>
    /// This method initialises the article class with parameter values.
    /// </summary>
    /// <param name="ArticleId">String: article identifier</param>
    /// <param name="Title">String: article title</param>
    /// <param name="Author">String: Author name</param>
    /// <param name="HeaderImageUrl">String:HeaderImageUrl</param>
    /// <param name="Body">String:article body </param>
    /// <param name="PublishDate">DateTime:article body </param>
    //  ---------------------------------------------------------------------------------
    public EuArticle (
      String ArticleId, 
      String Title, 
      String HeaderImageUrl,
      String Body,
      String Author, 
      DateTime PublishDate )
    {
      this.ArticleId = ArticleId;
      this.Title = Title;
      this.HeaderImageUrl = HeaderImageUrl;
      this.Body = Body;
      this.Author = Author;
      this.PublishDate = PublishDate;
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion

    #region Class PropertyList


    /// <summary>
    /// This property contains Article identifier, this will be same value as the parent
    /// Entity's EntityId
    /// </summary>
    [JsonProperty ( "aid" )]
    public String ArticleId { get; set; }

    /// <summary>
    /// This property contains the article title, this value will be the same as the 
    /// parent Entity's 'Title' field.
    /// </summary>
    [JsonProperty ( "t" )]
    public String Title { get; set; }

    /// <summary>
    /// This property contains name of the article's author, this value will be the same as the 
    /// parent Entity's 'AuthorName' property.
    /// </summary>
    [JsonProperty ( "an" )]
    public String Author { get; set; }

    /// <summary>
    /// This property contains absoluate URL to the header image, this value will be the same as the 
    /// parent Entity's 'HeaterImageUrl' field.
    /// Null value indicates that there is no image for this article.
    /// </summary>
    [JsonProperty ( "hiurl" )]
    public String HeaderImageUrl { get; set; }

    /// <summary>
    /// This Property contains the article body content, this value will be the same as the 
    /// parent Entity's 'Body' field. Using MarkDown document formatting.
    /// </summary>
    [JsonProperty ( "body" )]
    public String Body { get; set; }

    /// <summary>
    /// This Property contains the article publish date content, this value will be the same as the 
    /// parent Entity's Post/Submit date.
    /// </summary>
    [JsonProperty ( "bdt" )]
    public DateTime PublishDate { get; set; }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endregion
  } 
}//END namespace