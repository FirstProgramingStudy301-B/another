using System;
using System.Threading;

class GeneralTest{
  static void Main(){
    AsyncDelegate asyncDlg = new AsyncDelegate();
    asyncDlg.Run();
  }
}

class AsyncDelegate{
  
  delegate string ShowDelegate(string str);
  //非同期で実行するメソッドをデリゲートに格納する。
  ShowDelegate ayncDlgt;
  public AsyncDelegate(){
    ayncDlgt = new ShowDelegate(AsyncWaitingShow);

  }

  public void Run(){
    //ShowDelegate ayncDlgt = new ShowDelegate(AsyncWaitingShow);
    //Create CallBack Delegate   Signature   void CallBack(IAsyncResult arg)
    AsyncCallback callBack = new AsyncCallback(AsyncMethodCompleted);
    //BeginInvokeでThreadPoolを使用して非同期で実行されます。
    //引数は後ろから考える。
    //最後尾コールバックの引数
    //最後から次の引数: コールバックデリゲートを使用する。
    //残りは非同期メソッドの引数
    IAsyncResult ar = ayncDlgt.BeginInvoke("AsyncMessage",
        callBack,"CallBackState");
    //少し待ってみる
    //ar.AsyncWaitHandle.WaitOne(500);
    //MainThreadAction
    for(int i = 0 ; i< 100; i++){
      Thread.Sleep(50);
      Console.Write("HOG ");
    }
    //別スレッドの処理を待つ
    ar.AsyncWaitHandle.WaitOne();

    Console.WriteLine("-------------End of MainThread!!----------");
    Console.ReadKey();

  }
  
  //非同期で実行完了後、このメッソッドが実施される。
  void AsyncMethodCompleted(IAsyncResult res){
    string stat = (string) res.AsyncState;
    Console.WriteLine("AsyncState: " + stat);
    
    //非同期メソッドの戻り値を取得する。
    string resStr = ayncDlgt.EndInvoke(res);
    Console.WriteLine("AsyncMethodResult: " + resStr);
  }
  //別スレッドで実行される
  string AsyncWaitingShow(string msg){
    for(int i = 0; i < 10; i++){
      Thread.Sleep(500);
      Console.WriteLine(msg);
    }
    return "Async Completed";
  }
}
