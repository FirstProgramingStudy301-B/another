using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Runable {
  static void Main(){
    var basic = new BasicAsyncAwait();
    basic.AsyncRun();
    Console.WriteLine("---   Start   ---");
    basic.SyncRun();
    Console.WriteLine("---   End   ---");

    Console.ReadKey();
  }
}
class BasicAsyncAwait{
  public async void AsyncRun(){
    var res = await WaitMethod(2000);
    Console.WriteLine("---   ASynMethod   ---");
    Console.WriteLine(res);
  }
  public void SyncRun(){
    // res is Task<int>
    var res = WaitMethod(5000);
    res.Wait();
    Console.WriteLine("---   SynMethod   ---");
    Console.WriteLine(res.Result);
  }
  async Task<int> WaitMethod(int sec){
    await Task.Delay(sec);
    return sec;
  }
}
