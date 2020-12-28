using System;

public class ServerPageScript
{

  static public bool OnOpenPage ( Evado.Model.eClinical.EvForm Form )
  {
    Form.ScriptMessage = "Server Script: OnOpenPage method executed. \r\n\r\n" + Form.ScriptMessage;
    //
    // Return true indicates successfully completion.
    //
    return true;

  }//END onUpdate script method

  static public bool onUpdatePage ( Evado.Model.eClinical.EvForm Form )
  {
    Form.ScriptMessage = "Server Script: OnOpenPage method executed. \r\n\r\n" + Form.ScriptMessage;
    //
    // Return true indicates successfully completion.
    //
    return true;

  }//END onUpdate script method

}//END Script class

