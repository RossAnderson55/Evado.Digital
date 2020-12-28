/***************************************************************************************
 * <copyright file="BLL\clinical\EvReportDataProviderBase.cs" company="EVADO HOLDING PTY. LTD.">
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
 *  This class contains the EvCaseReportForms business object.
 *
 ****************************************************************************************/
 using System;
using System.Collections.Generic;
using System.Text;
using Evado.Model;
using Evado.Model.Digital;

namespace Evado.Bll.Clinical
{
  /// <summary>
  /// This class should be inherited by all of the classes that want to provide ResultData for the reports.
  /// Andres Castano July 08 2010
  /// </summary>
  class EvReportDataProviderBase
  {
    /// <summary>
    /// This method should be overriden by all of the classes that want to provide ResultData for the reports.
    /// </summary>
    /// <param name="Report"></param>
    /// <returns></returns>
    public virtual EvReport getReport(EvReport Report)
    {
      return Report;
    }//END getReport class

  }//END EvReportDataProviderBase class

}//END namespace Evado.Bll.Clinical
