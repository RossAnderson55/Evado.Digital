using Microsoft.SqlServer.Server;
using System.Data.SqlClient;
using System;

public class AiFunctions
{

  [SqlFunction ( DataAccess = DataAccessKind.Read )]
  public static int ByteLength ( byte [ ] SourceData )
  {
    return SourceData.Length;
  }


  [SqlFunction ( DataAccess = DataAccessKind.Read )]
  public static bool MustHave ( byte[] SourceData, byte[] MustHave)
  {
    byte [ ] resultData = new byte [ SourceData.Length ];
    bool match = false;

    for ( int i = 0; i < SourceData.Length && i < MustHave.Length; i++ )
    {
      uint answer = (uint) SourceData [ i ] & (uint) MustHave [ i ];
      resultData [ i ] = Convert.ToByte ( answer );

      if ( resultData [ i ] == MustHave [ i ] )
      {
        match = true;
      }
    }

    return match;
  }

  [SqlFunction ( DataAccess = DataAccessKind.Read )]
  public static byte [ ] CouldHave ( byte [ ] SourceData, byte [ ] CouldHave )
  {
    byte [ ] resultData = new byte [ SourceData.Length ];

    for ( int i = 0; i < SourceData.Length && i < CouldHave.Length; i++ )
    {
      uint answer = (uint) SourceData [ i ] & (uint) CouldHave [ i ];
      resultData [ i ] = Convert.ToByte ( answer );
    }

    return resultData;
  }
}