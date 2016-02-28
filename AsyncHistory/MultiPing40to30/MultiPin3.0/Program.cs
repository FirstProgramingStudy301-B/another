using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;

namespace MultiPin3._0
{
    class Program
    {
        static ManualResetEvent mainWait = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            #region CreatClosure１ラムダ編
            //クロージャーを使用して出力されるIPをカウントしています。下記のStaticMethodでもOK
            //Func<int,Action<string,string>> CreateShowResult = x =>
            //{
            //    var callCount = x;
            //    return (ip, stat) =>
            //    {
            //        Console.WriteLine("{0:D3} IP{1,15}: {2}",
            //            ++callCount, ip, stat);
            //    };
            //};
            #endregion
            
            var ShowConsoleResult = CreateShowResult(0);

            var ipAddreses = Enumerable.Range(1, 254).Select(i => "172.29.111." + i);

            var pinResArray = ipAddreses
                .Select(ip => new ThreadPing(ip, ShowConsoleResult))
                .ToArray();

            foreach (var item in pinResArray)
            {
                item.Start();
            }
            #region Thread監視及びMainThreadへの同期１
            //ThreadPool.QueueUserWorkItem(_ => 
            //{
            //    int CompletedCount = 0;
            //    while (CompletedCount != pinResArray.Length)
            //    {
            //        Thread.Sleep(10);
            //        CompletedCount = 0;
            //        foreach (var item in pinResArray)
            //        {
            //            if (item.Completed)
            //            {
            //                CompletedCount++;
            //            }
            //        }
            //        //Console.WriteLine("----------  {0}  ---------",CompletedCount);
            //    }
            //    mainWait.Set();
            //});
            //mainWait.WaitOne();
            #endregion

            #region ThreadsWaitting MainThreadへの同期２
            foreach(var item in pinResArray)
            {
                item.PinResetEvent.WaitOne();
            }
            #endregion
            Console.WriteLine("--- ThreadFinished{0}",pinResArray.Length);
            Console.ReadKey();
        }
        #region CreateClosure２メソッド編
        //クロージャーを使用して出力されるIPをカウントしています。上記のClosureMethodでもOK
        static Action<string, string> CreateShowResult(int initNum)
        {
            int callCount = initNum;
            return (ip, stat) =>
            {
                Console.WriteLine("{0:D3}IP{1,15}: {2}",
                    ++callCount, ip, stat);
            };
        }
        #endregion
    }
}
