using System;
using System.Threading;

namespace Async.Net3
{
	class MainClass
	{
		delegate string dlgGetIDName(string name,int id);
		static dlgGetIDName getIdNameDalegate;
		//Waiting Thread Method
		static string GetIdName(string name,int id){
			for (int i = 0; i < 100; i++) {
				Thread.Sleep (100);
				Console.Write(".");
			}
			Console.WriteLine (".");
			return string.Format ("{0:D3}: {1}", id, DateTime.Now);
		}
		public static void Main (string[] args)
		{
			var i = 100;
			//.Net1.1 Asynchronous Programing Model (APM)
			// Create Delegate

			getIdNameDalegate = new dlgGetIDName(GetIdName);
			Console.WriteLine ("Start Threading.");								
			IAsyncResult thStat = 
				getIdNameDalegate.BeginInvoke ("hoge", i, //GetIdName Arguments 
					//AsyncCallbak Delegate    CallBack Argument ar.AsyncState
				new AsyncCallback (thCallBack), DateTime.Now);
			//Thread Statment Property. Finish Thread Blocking.
			thStat.AsyncWaitHandle.WaitOne ();
			Console.WriteLine ("Finish Threading.");
			//Console.ReadKey ();
		}
		//AsyncCallback Method
		static void thCallBack(IAsyncResult ar)
		{
			//GetIdName method Resule is string
			var result = getIdNameDalegate.EndInvoke (ar);
			//BeginInvoke method last a argument Castting.
			var datetime = (DateTime)ar.AsyncState;
			Console.WriteLine ("StartTime: {0}, EndTime: {1}",
				datetime, result);
		}
	}
}
