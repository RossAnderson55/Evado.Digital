using System;
using System.Collections;
using System.Data;
using System.IO;

using Evado.Model;
using Evado.Bll;
using Evado.Model.eClinical;
using Evado.Bll.eClinical;

public class ServerPageScript
{

  static public bool onOpenPage ( EvForm Form )
  {
    Form.ScriptMessage = "Server Script: onLoadForm method executed. \r\n\r\n" + Form.ScriptMessage;
    ///
    /// Return true indicates successfully completion.
    ///
    return true;

  }//END onUpdate script method

  static public bool onUpdatePage ( EvForm Form )
  {
    Form.ScriptMessage = "Server Script: onPostBackForm method executed. \r\n\r\n" + Form.ScriptMessage;
    ///
    /// Return true indicates successfully completion.
    ///
    return true;

  }//END onUpdate script method

  static public bool onUpdateForm( EvForm Form )
  {
    Form.Comments = "Server Script: onUpdateForm method executed. \r\n\r\n" + Form.Comments;
    ///
    /// Return true indicates successfully completion.
    ///
    return true;

  }//END onUpdate script method

}//END Script class

