using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evado.Model.Integration
{
  /// <summary>
  /// This model class defines the Web Service Query structure.
  /// </summary>
  [Serializable]
  public class EiColumnParameters
  {
    // ===================================================================================
    /// <summary>
    /// This method initialises the class
    /// </summary>
    // -----------------------------------------------------------------------------------
    public EiColumnParameters ( )
    {
    }

    // ===================================================================================
    /// <summary>
    /// This initialisation method initialises the class 
    /// and initialises the values.
    /// </summary>
    /// <param name="Datatype"></param>
    /// <param name="EvadoFieldId"></param>
    // -----------------------------------------------------------------------------------
    public EiColumnParameters (
      EiDataTypes Datatype,
      String EvadoFieldId )
    {
      this._DataType = Datatype;
      this.Name = EvadoFieldId;
      this.EvadoFieldId = EvadoFieldId;
    }

    // ===================================================================================
    /// <summary>
    /// This initialisation method initialises the class 
    /// and initialises the values.
    /// </summary>
    /// <param name="Datatype"></param>
    /// <param name="EvadoFieldId"></param>
    // -----------------------------------------------------------------------------------
    public EiColumnParameters (
      EiDataTypes Datatype,
      object EvadoFieldId )
    {
      this._DataType = Datatype;
      this.Name = EvadoFieldId.ToString();
      this.EvadoFieldId = EvadoFieldId.ToString();
    }

    // ===================================================================================
    /// <summary>
    /// This initialisation method initialises the class 
    /// and initialises the values.
    /// </summary>
    /// <param name="Datatype"></param>
    /// <param name="EvadoFieldId"></param>
    /// <param name="Index">True: column is the data index</param>
    // -----------------------------------------------------------------------------------
    public EiColumnParameters (
      EiDataTypes Datatype,
      String EvadoFieldId,
      bool Index)
    {
      this._DataType = Datatype;
      this.Name = EvadoFieldId;
      this.EvadoFieldId = EvadoFieldId;
      this.Index = Index;
    }

    // ===================================================================================
    /// <summary>
    /// This initialisation method initialises the class 
    /// and initialises the values.
    /// </summary>
    /// <param name="Datatype"> Date Type enumerated vlaue</param>
    /// <param name="EvadoFieldId">object Evado Field identifier</param>
    /// <param name="Index">Bool: True column is the index</param>
    // -----------------------------------------------------------------------------------
    public EiColumnParameters (
      EiDataTypes Datatype,
      object EvadoFieldId,
      bool Index )
    {
      this._DataType = Datatype;
      this.Name = EvadoFieldId.ToString ( );
      this.EvadoFieldId = EvadoFieldId.ToString ( );
      this.Index = Index;
    }

    // ===================================================================================
    /// <summary>
    /// This initialisation method initialises the class 
    /// and initialises the values.
    /// </summary>
    /// <param name="Datatype"></param>
    /// <param name="Name"></param>
    /// <param name="EvadoFieldId"></param>
    // -----------------------------------------------------------------------------------
    public EiColumnParameters (
      EiDataTypes Datatype,
      String Name,
      String EvadoFieldId )
    {
      this._DataType = Datatype;
      this.Name = Name;
      this.EvadoFieldId = EvadoFieldId;
    }

    // ===================================================================================
    /// <summary>
    /// This initialisation method initialises the class 
    /// and initialises the values.
    /// </summary>
    /// <param name="Datatype"> Date Type enumerated value</param>
    /// <param name="Name">String: column name</param>
    /// <param name="EvadoFieldId">object Evado Field identifier</param>
    // -----------------------------------------------------------------------------------
    public EiColumnParameters (
      EiDataTypes Datatype,
      String Name,
      object EvadoFieldId )
    {
      this._DataType = Datatype;
      this.Name = Name;
      this.EvadoFieldId = EvadoFieldId.ToString();
    }

    // ===================================================================================
    /// <summary>
    /// This initialisation method initialises the class 
    /// and initialises the values.
    /// </summary>
    /// <param name="Datatype"> Date Type enumerated vlaue</param>
    /// <param name="Name">String: column name</param>
    /// <param name="EvadoFieldId">String Evado Field identifier</param>
    /// <param name="Index">Bool: True column is the index</param>
    /// <param name="MetaData">String: column metadata</param>
    // -----------------------------------------------------------------------------------
    public EiColumnParameters (
      EiDataTypes Datatype,
      String Name,
      String EvadoFieldId,
      bool Index,
      String MetaData )
    {
      this._DataType = Datatype;
      this.Name = Name;
      this.EvadoFieldId = EvadoFieldId;
      this.Index = Index;
    }

    // ===================================================================================
    /// <summary>
    /// This initialisation method initialises the class 
    /// and initialises the values.
    /// </summary>
    /// <param name="Datatype"> Date Type enumerated vlaue</param>
    /// <param name="Name">String: column name</param>
    /// <param name="EvadoFieldId">object Evado Field identifier</param>
    /// <param name="Index">Bool: True column is the index</param>
    /// <param name="MetaData">String: column metadata</param>
    // -----------------------------------------------------------------------------------
    public EiColumnParameters (
      EiDataTypes Datatype,
      String Name,
      object EvadoFieldId,
      bool Index )
    {
      this._DataType = Datatype;
      this.Name = Name;
      this.EvadoFieldId = EvadoFieldId.ToString ( );
      this.Index = Index;
    }

    EiDataTypes _DataType = EiDataTypes.Text;

    /// <summary>
    /// This property defines the column data QueryType for data validation
    /// </summary>
    public EiDataTypes DataType { get; set; }
    /// <summary>
    /// This property defines the column name (external identifier)
    /// </summary>
    public String Name { get; set; }

    /// <summary>
    /// This property defines the columns Evado field identifier.
    /// </summary>
    public String EvadoFieldId { get; set; }

    /// <summary>
    /// This property indicates that the column is an Evado index identifier.
    /// </summary>
    public bool Index { get; set; }

  }//END EvWebServiceQuery

}//END Namespace Evado.Model.Integration
