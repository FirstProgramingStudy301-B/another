using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;

namespace Net3._5AsyncConsole
{
    class Program
    {
        //基本ThreadPoolで処理する関数　引数を一指定できる。
        static void BasicThreadMethod(object stat)
        {
            var tmpObj = (object[])stat;
            string arg = string.Empty;
            for (int l = 0; l < tmpObj.Length; l++)
            {
                arg += tmpObj[l].ToString();
            }
            for (int i = 0; i < 150; i++)
            {
                Thread.Sleep(10);
                Console.Write(" {0}" ,arg);
            }
            int workThread, compThread;
            ThreadPool.GetAvailableThreads(out workThread,out compThread);
            Console.WriteLine("Working: {0},Completed: {1}",workThread,compThread);
        }
        static void Main(string[] args)
        {
            //ThreadPoolの基本2.0?
            WaitCallback waitCaller = new WaitCallback(BasicThreadMethod);
            //ThreadPoolに登録
            ThreadPool.QueueUserWorkItem(waitCaller, new object[] { "X" });
            ThreadPool.QueueUserWorkItem(waitCaller, new object[] { "*" });
            //object配列を渡すと複数の引数を渡せる。
            ThreadPool.QueueUserWorkItem(waitCaller, new object[] { "I", "-" });

            int workThread, compThread;
            ThreadPool.GetAvailableThreads(out workThread, out compThread);
            Console.WriteLine("Working: {0},Completed: {1}", workThread, compThread);
            Console.ReadKey();


            //ThreadPoolを使用しています。
            //基本的にEndInvokeを呼びスレッドを閉じなくてはならない。
            //デリゲートを使用した方法
            BlockingCollBack();
            Console.ReadKey();
            //ラムダ式を使用した方法

        }
        //EndInvokeによるブロッキング
        static void BlockingEndInvoke()
        {
            // The asynchronous method puts the thread id here.
            int threadId;

            // Create an instance of the test class.
            AsyncDemo ad = new AsyncDemo();

            // Create the delegate.
            AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

            // Initiate the asychronous call.
            // デリゲートを同じ引き数＋コールバック関数＋コールバック引数となります。
            IAsyncResult result = caller.BeginInvoke(3000,
                out threadId, null, null);

            Thread.Sleep(0);
            Console.WriteLine("Main thread {0} does some work.",
                Thread.CurrentThread.ManagedThreadId);

            // Call EndInvoke to wait for the asynchronous call to complete,
            // and to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, result);

            Console.WriteLine("The call executed on thread {0}, with return value \"{1}\".",
                threadId, returnValue);
        }
        //WaitHandle による非同期呼び出しの待機
        static void BlockingWaitHandle()
        {
            // スレッドID保存用の変数を宣言
            int threadId;

            //AsyncDemoクラスをインスタンス化
            AsyncDemo ad = new AsyncDemo();

            //AsyncDemoのデリゲート型を使用してデリゲートを作成
            AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

            // デリゲートのBeginInvokeを使用してThreadPoolで処理を開始
            // 引数#1はデリゲートの引数１引数#2はデリゲートの第二引数
            // #3,コールバック関数を利用する場合、#4はコールバック関数の引数
            IAsyncResult result = caller.BeginInvoke(3000,
                out threadId, null, null);

            Thread.Sleep(0);
            Console.WriteLine("Main thread {0} does some work.",
                Thread.CurrentThread.ManagedThreadId);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            // Perform additional processing here.
            // Call EndInvoke to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            Console.WriteLine("The call executed on thread {0}, with return value \"{1}\".",
                threadId, returnValue);
        }
        //非同期呼び出し完了のポーリングによる待機
        static void BlockingIsCompleted()
        {
            // The asynchronous method puts the thread id here.
            int threadId;

            // Create an instance of the test class.
            AsyncDemo ad = new AsyncDemo();

            // Create the delegate.
            AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

            // Initiate the asychronous call.
            IAsyncResult result = caller.BeginInvoke(3000,
                out threadId, null, null);

            // Poll while simulating work.ここでポーリング確認
            while (result.IsCompleted == false)
            {
                Thread.Sleep(250);
                Console.Write(".");
            }

            // Call EndInvoke to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, result);

            Console.WriteLine("\nThe call executed on thread {0}, with return value \"{1}\".",
                threadId, returnValue);
        }
        //非同期呼び出し完了じのコールバックによる待機
        static void BlockingCollBack()
        {
            // Create an instance of the test class.
            AsyncDemo ad = new AsyncDemo();

            // Create the delegate.
            AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

            // The threadId parameter of TestMethod is an out parameter, so
            // its input value is never used by TestMethod. Therefore, a dummy
            // variable can be passed to the BeginInvoke call. If the threadId
            // parameter were a ref parameter, it would have to be a class-
            // level field so that it could be passed to both BeginInvoke and 
            // EndInvoke.
            int dummy = 0;

            // Initiate the asynchronous call, passing three seconds (3000 ms)
            // for the callDuration parameter of TestMethod; a dummy variable 
            // for the out parameter (threadId); the callback delegate; and
            // state information that can be retrieved by the callback method.
            // In this case, the state information is a string that can be used
            // to format a console message.
            IAsyncResult result = caller.BeginInvoke(3000,
                out dummy,
                new AsyncCallback(CallbackMethod),
                "The call executed on thread {0}, with return value \"{1}\".");

            Console.WriteLine("The main thread {0} continues to execute...",
                Thread.CurrentThread.ManagedThreadId);

            // mainスレッドを待機させる3秒以上待機させても問題ない。
            //Thread.Sleep(4000);
            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            Console.WriteLine("The main thread ends.");
        }
        // The callback method must have the same signature as the
        // AsyncCallback delegate.
        static void CallbackMethod(IAsyncResult ar)
        {
            // Retrieve the delegate.
            AsyncResult result = (AsyncResult)ar;
            AsyncMethodCaller caller = (AsyncMethodCaller)result.AsyncDelegate;

            // Retrieve the format string that was passed as state 
            // information.
            string formatString = (string)ar.AsyncState;

            // Define a variable to receive the value of the out parameter.
            // If the parameter were ref rather than out then it would have to
            // be a class-level field so it could also be passed to BeginInvoke.
            int threadId = 0;

            // Call EndInvoke to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, ar);

            // Use the format string to format the output message.
            Console.WriteLine(formatString, threadId, returnValue);
        }

    }
}

