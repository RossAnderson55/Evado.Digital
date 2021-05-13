/***************************************************************************************
 * <copyright file="BLL\AssemblyAttributes.cs" company="EVADO HOLDING PTY. LTD.">
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
 * Description: 
 *  This class contains the EvCommonFormFields business object.
 *
 ****************************************************************************************/

using System;
using System.Collections; 
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Evado.Digital.Bll
{
	/// <summary>
  /// Summary description for AssemblyAttributes class.
	/// </summary>
	public class AssemblyAttributes
  {
    #region Class variables. 

    private string _Version = String.Empty;
    private string _FullVersion = String.Empty;
    private string _Title = String.Empty;
		private string _Copyright = String.Empty;
		private string _Trademark = String.Empty;
		private string _Product = String.Empty;
		private string _Company = String.Empty;
		private string _Description = String.Empty;
    #endregion

    /// <summary>
    /// The class initialisation method.
    /// </summary>
    public AssemblyAttributes ( )
		{
			char SEPARATOR = Convert.ToChar(",");
		
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
				
			string [] FullName = assembly.FullName.Split( SEPARATOR );
				
			for ( int i = 0 ; i < FullName.Length; i++ )
			{
				if ( FullName[i].LastIndexOf("Version=") > 0 )
				{
					int length =  FullName[i].LastIndexOf("=");
					string stVersion = FullName[i].Substring( length+1 ) + "\r\n";
					
					Version version = new Version( stVersion );
					
					_Version = version.Major +"." + version.Minor +"." + version.Build ;
          _FullVersion = version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
					
				}
			}
				
			object[] customAttributes = assembly.GetCustomAttributes(true);
				
			for ( int j = 0; j < customAttributes.Length; j++ )
			{

        System.Type Type = customAttributes[ j ].GetType();
					
        if ( Type.Name == "AssemblyTitleAttribute" )
        {
          _Title = ((AssemblyTitleAttribute)customAttributes[j]).Title;
        }

        if ( Type.Name == "AssemblyCopyrightAttribute" )
        {
          _Copyright = ((AssemblyCopyrightAttribute)customAttributes[j]).Copyright;
        }

        if ( Type.Name == "AssemblyProductAttribute" )
        {
          _Product = ((AssemblyProductAttribute)customAttributes[j]).Product;
        }
        if ( Type.Name == "AssemblyCompanyAttribute" )
        {
          _Company = ((AssemblyCompanyAttribute)customAttributes[j]).Company;
        }
        if ( Type.Name == "AssemblyDescriptionAttribute" )
        {
          _Description = ((AssemblyDescriptionAttribute)customAttributes[j]).Description;
        }
        if ( Type.Name == "AssemblyTrademarkAttribute" )
        {
          _Trademark = ((AssemblyTrademarkAttribute)customAttributes[j]).Trademark;
        }
			}//END For loop
			
		}// END WebAttributes class constructor;

    #region Class Property
    /// <summary>
    /// This class contains a Company attribute
    /// </summary>
    public string Company 
		{
			get { return _Company; }
		}

    /// <summary>
    /// This class contains a Copyright attribute
    /// </summary>
		public string Copyright 
		{
			get { return _Copyright; }
		}

    /// <summary>
    /// This class contains a Description attribute
    /// </summary>
		public string Description 
		{
			get { return _Description; }
		}

    /// <summary>
    /// This class contains a Product attribute
    /// </summary>
		public string Product 
		{
			get { return _Product; }
		}

    /// <summary>
    /// This class contains a Trademark attribute
    /// </summary>
		public string Trademark 
		{
			get { return _Trademark; }
		}

    /// <summary>
    /// This class contains a Title attribute
    /// </summary>
		public string Title 
		{
			get { return _Title; }
		}

    /// <summary>
    /// This class contains a Version attribute
    /// </summary>
		public string Version 
		{
			get { return _Version; }
    }

    /// <summary>
    /// This class contains a FullVersion attribute
    /// </summary>
    public string FullVersion
    {
      get { return _FullVersion; }
    }
    #endregion

    /// <summary>
    /// This class displays the attributes. 
    /// </summary>
    /// <returns>string: a display attribute string.</returns>
    public string DisplayAttributes ( )
    {
      string strAttributes = "Company:\t" + this._Company + "\r\n"
        + "Product:\t" + this._Product + "</td></tr>"
        + "Title:\t" + this._Title + "\r\n"
        + "Description:\t" + this._Description + "\r\n"
        + "Trademark:\t" + this._Trademark + "\r\n"
        + "Version:\t" + this._Version + "\r\n"
        + "Full Version:\t" + this._FullVersion + "\r\n";

      return strAttributes;
    }//END DisplayAttributes class

	}//END AssemblyAttributes Class
	
}  //END Version namespace

