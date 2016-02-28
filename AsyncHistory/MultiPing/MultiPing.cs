using System;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

class BeginDNS{
  class RequestState/*{{{*/
  {
    public IPHostEntry host;
    public RequestState()
    {
        host = null;
    }
  }
  static ManualResetEvent allDone;
  //public Run()
  public void Run()
  {/*{{{*/
    allDone = new ManualResetEvent(false);
    // Create an instance of the RequestState class.
    RequestState myRequestState = new RequestState();

    // Begin an asynchronous request for information like host name, IP addresses, or 
    // aliases for specified the specified URI.
    IAsyncResult asyncResult = Dns.BeginGetHostEntry("www.contiioso.com", new AsyncCallback(RespCallback), myRequestState );

    // Wait until asynchronous call completes.
    allDone.WaitOne();
    //asyncResult.AsyncWaitHandle.WaitOne();
    if(myRequestState.host == null) return;
    if(asyncResult.IsCompleted) Console.WriteLine("Get IPs!!");
    Console.WriteLine("Host name : " + myRequestState.host.HostName);
    Console.WriteLine("\nIP address list : ");
    for(int index=0; index < myRequestState.host.AddressList.Length; index++)
    {
      Console.WriteLine(myRequestState.host.AddressList[index]);
    }
    Console.WriteLine("\nAliases : ");
    for(int index=0; index < myRequestState.host.Aliases.Length; index++)
    {
      Console.WriteLine(myRequestState.host.Aliases[index]);
    }
  }/*}}}*/
  // private CallBack
  void RespCallback(IAsyncResult ar)
  {/*{{{*/
    
    try 
    {
      // Convert the IAsyncResult object to a RequestState object.
      RequestState tempRequestState = (RequestState)ar.AsyncState;
      // End the asynchronous request.
      tempRequestState.host = Dns.EndGetHostEntry(ar);
      //allDone.Set();
    }
    catch(ArgumentNullException e) 
    {
      Console.WriteLine("ArgumentNullException caught!!!");
      Console.WriteLine("Source : " + e.Source);
      Console.WriteLine("Message : " + e.Message);
    }   
    catch(Exception e)
    {
      Console.WriteLine("Exception caught!!!");
      Console.WriteLine("Source : " + e.Source);
      Console.WriteLine("Message : " + e.Message);
    }
    finally{
      if(allDone != null) allDone.Set();
      
    }
  }/*}}}*/
}/*}}}*/
class TaskPing
{
  public async  void Run()
  {
    var scop = "172.29.111.";
    //var scop = "216.239.32.";
    Console.WriteLine(scop + 81);
    var tRes = await taskPing(scop + "81");
    Console.WriteLine(tRes);
    var ress = Enumerable.Range(0,254).Select(async i => await taskPing(scop+i));
    foreach(var itm in ress.ToList())
    {
      Console.WriteLine(itm.Result.ToString());
    }
    Console.ReadKey();
  }
  async Task<string> taskPing(string ip)
  {
    IPAddress ipAdr = null;
    var ipchk = IPAddress.TryParse(ip,out ipAdr);
    if(ipchk == false) return string.Format("{0}: Can not Convert IPAddress",ip);
    string res = string.Empty;
    try{
      var reply = await (new Ping()).SendPingAsync(ipAdr,1000);
      res = reply.Status == IPStatus.Success ? "OK":"NG";
    }catch(Exception ex){
      return string.Format("{0}: {1}",ip,ex.Message);
    }
    return string.Format("{0}: {1}",ip,res);
  }
}
class MultiPings
{
  public static void Main(string[] args)
  {
    //var beginDNS = new BeginDNS();
    //beginDNS.Run();

    var tskDNS = new TaskPing();
    tskDNS.Run();
  }
}
