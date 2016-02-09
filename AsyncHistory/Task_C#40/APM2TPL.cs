using System;
using System.Threading.Tasks;
using System.Net;

class asyncmethodPattern
{
  static void Main(string[] args)
  {
    //01Basic FromAsync
    BasicFromAsync01();

    BasicFromAsync02();

    BasicFromAsync03();

    //この後の学習の予定はタスクのスケジュール
    // ContinueWith
    // ContinueWhenAll
    // ContinueWhenAny    Task.Factory.ContinueWhenAllで作成できる。
    Console.ReadKey();
  }
  //01Basic FromAsync
  static void BasicFromAsync01()/*{{{*/
  {
    Task<IPAddress[]> task = Task<IPAddress[]>.Factory.FromAsync(
      Dns.BeginGetHostAddresses, Dns.EndGetHostAddresses,
      "www.microsoft.com", null); 
    
    var ips = task.Result as IPAddress[];
    foreach(var ip in ips) Console.WriteLine(ip);
  }/*}}}*/
  //02 AMP2TPL
  static void BasicFromAsync02()
  {
    var tsk = GetHostAddressesAsTask("www.microsoft.com");
    foreach(var ip in tsk.Result) Console.WriteLine(ip);
  }
  //03 EAP2TPL
  static void BasicFromAsync03()
  {
    var uri = new Uri("http://www.microsoft.com");
    var tsk = DownloadStringAsTask(uri);
    Console.WriteLine(tsk.Result);
  }
  //02sub AMP2TPL Use TaskCompletionSource.
  static Task<IPAddress[]> GetHostAddressesAsTask(/*{{{*/
    string hostNameOrAddress) {
    var tcs = new TaskCompletionSource<IPAddress[]>();
    Dns.BeginGetHostAddresses(hostNameOrAddress, iar => {
      try { 
        tcs.SetResult(Dns.EndGetHostAddresses(iar)); }
      catch(Exception exc) { tcs.SetException(exc); }
    }, null);
    return tcs.Task;
  }/*}}}*/
  //03sub EAP2TPl Event-based Async Pattern
  static Task<string> DownloadStringAsTask(Uri address) {/*{{{*/
    TaskCompletionSource<string> tcs = 
      new TaskCompletionSource<string>();
    WebClient client = new WebClient();
    client.DownloadStringCompleted += (sender, args) => {
      if (args.Error != null) tcs.SetException(args.Error);
      else if (args.Cancelled) tcs.SetCanceled();
      else tcs.SetResult(args.Result);
    };
    client.DownloadStringAsync(address);
    return tcs.Task;
  }/*}}}*/
}
