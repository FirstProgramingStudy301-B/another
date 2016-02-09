using System;
using System.Threading.Tasks;

class Program{
  const int COUNT = 1000;
  //01 Thradding Task
  static void BasicTask01(){/*{{{*/
    var tsk = Task.Factory.StartNew(()=>
    {
      for(int i = 0; i < COUNT; i++) Console.Write('A');
    });
    // Main Thread Task
    for(int i = 0; i < COUNT; i++) Console.Write('B');
    tsk.Wait();
  }/*}}}*/
  //02 Threading ContinueTask
  static void BasicTask02(){/*{{{*/
    Action<Task>  notify = t => 
    {
      Console.WriteLine();
      Console.WriteLine("FinishedTask{0}",t.Id);
    };
    var cntTsk01 = Task.Factory.StartNew(() =>
    {
      for(int i = 0; i < COUNT; i++) Console.Write('A');
    });
    cntTsk01.ContinueWith(notify);

    var cntTsk02 = Task.Factory.StartNew(() =>
    {
      for(int i = 0; i < COUNT; i++) Console.Write('C');
    });
    cntTsk02.ContinueWith(notify);
    for(int i = 0; i<COUNT; i++) Console.Write('B');

    /*cntTsk02.Wait();
    cntTsk01.Wait();
    */
    Task.WaitAny(cntTsk01,cntTsk02);
  }/*}}}*/

  static void Main(string[] args){
    //01 Thradding Task
    //BasicTask01();

    //02 ContinueTask
    BasicTask02();

  }
}
