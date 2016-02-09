using System;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;

class program{
  static void Main(string[] args){

  Task<string> page1Task = DownloadStringAsTask(
    new Uri("http://www.microsoft.com"));
  Task<string> page2Task = DownloadStringAsTask(
    new Uri("http://www.msdn.com"));

  Task<int> count1Task = page1Task.ContinueWith(t => CountParagraphs(t.Result));
  Task<int> count2Task = page2Task.ContinueWith(t => CountParagraphs(t.Result));

  Task.Factory.ContinueWhenAll( new[] { count1Task, count2Task },
    tasks => {
      Console.WriteLine( "<P> tags on microsoft.com: {0}", count1Task.Result);
      Console.WriteLine( "<P> tags on msdn.com: {0}", count2Task.Result);
    });
        
  Console.ReadKey();
  }
  // Task要の定義
  static Task<string> DownloadStringAsTask(Uri address)
  {
      TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
      WebClient client = new WebClient();
      client.DownloadStringCompleted += (sender, args) =>
      {
          if (args.Error != null) tcs.SetException(args.Error);
          else if (args.Cancelled) tcs.SetCanceled();
          else tcs.SetResult(args.Result);
      };
      client.DownloadStringAsync(address);
      return tcs.Task;
  }

  static int CountParagraphs(string s)
  {

      Console.WriteLine(new string('-',60));
      Console.WriteLine(s);
      Console.WriteLine(new string('-',60));
      return Regex.Matches(s, "<p").Count;
  }
}
