#region Help:  Introduction to the Script Component
/* The Script Component allows you to perform virtually any operation that can be accomplished in
 * a .Net application within the context of an Integration Services data flow.
 *
 * Expand the other regions which have "Help" prefixes for examples of specific ways to use
 * Integration Services features within this script component. */
#endregion

#region Namespaces
using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
#endregion

/// <summary>
/// This is the class to which to add your code.  Do not change the name, attributes, or parent
/// of this class.
/// </summary>
[Microsoft.SqlServer.Dts.Pipeline.SSISScriptComponentEntryPointAttribute]
public class ScriptMain : UserComponent
{
    IDTSConnectionManager100 connMgr;
    SqlConnection sqlConn;
    SqlDataReader sqlReader;
    /// <summary>
    /// This method is called once, before rows begin to be processed in the data flow.
    ///
    /// You can remove this method if you don't need to do anything here.
    /// </summary>
    public override void AcquireConnections(object Transaction)
    {
        connMgr = this.Connections.Connection;
        sqlConn = (SqlConnection)connMgr.AcquireConnection(null);
    }
    public override void PreExecute()
    {
        base.PreExecute();
        var cmd = sqlConn.CreateCommand();
        cmd.CommandText = "SELECT data FROM naprawy";
        sqlReader = cmd.ExecuteReader();
        /*
         * Add your code here
         */
    }
    /// <summary>
    /// This method is called after all the rows have passed through this component.
    ///
    /// You can delete this method if you don't need to do anything here.
    /// </summary>
    public override void PostExecute()
    {
        base.PostExecute();
        sqlReader.Close();
        /*
         * Add your code here
         */
    }
    public override void CreateNewOutputRows()
    {
        var oldest = new DateTime(2100, 12, 31);
        var newest = new DateTime(1900, 1, 1);
        while (sqlReader.Read())
        {
            var current = sqlReader.GetDateTime(0);
            if (DateTime.Compare(oldest, current) > 0)
            {
                oldest = current;
            }
            if (DateTime.Compare(newest, current) < 0)
            {
                newest = current;
            }
        }
        for (var date = oldest; newest.CompareTo(date) >= 0; date = date.AddDays(1))
        {
            Output0Buffer.AddRow();
            Output0Buffer.Rok = date.Year;
            Output0Buffer.Miesiac = date.Month;
            Output0Buffer.Dzien = date.Day;
        }
        //Output0Buffer.AddRow();
        //Output0Buffer.Rocznik = oldest;
        /*
          Add rows by calling the AddRow method on the member variable named "<Output Name>Buffer".
          For example, call MyOutputBuffer.AddRow() if your output was named "MyOutput".
        */
    }
    public override void ReleaseConnections()
    {

        connMgr.ReleaseConnection(sqlConn);

    }

}
