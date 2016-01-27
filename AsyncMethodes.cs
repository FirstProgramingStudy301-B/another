using System;
using System.Threading;

namespace Async.Net3
{
	//Asynchronous Programing Model (APM)
	public class AsyncProgMethod
	{
		protected const int THREADWAITCOUNT = 30;
		protected const int THREADWAITTIME = 100;
	}
	public class AsyncNet35APM : AsyncProgMethod
	{
		string Name = "Net35";
		public void run()
		{
			Func<string,int,string> thrMethod = (name, id) => {
				for (int i = 0; i < THREADWAITCOUNT; i++) {
					Thread.Sleep (THREADWAITTIME);
					Console.Write (".");
				}
				Console.WriteLine (".");
				return string.Format ("{0:D3}: {1}"+Environment.NewLine, id, DateTime.Now);
			};
			var thrStat = thrMethod.BeginInvoke ("Bar", 300,
				((ar) => {
					var result = thrMethod.EndInvoke(ar);
					var dTime = (DateTime)ar.AsyncState;
					Console.WriteLine ("StartTime: {0}, EndTime: {1}",dTime, result);
				}), DateTime.Now);
			Console.WriteLine ("Start Threading on {0}.",Name);
			thrStat.AsyncWaitHandle.WaitOne ();
			Console.WriteLine ("Finish Thread on {0}.",Name);
		}
	}
	#region anonymous method in .NetFramework2.0 of One Method.
	public class AsyncNet20APM : AsyncProgMethod
	{
		string Name = "Net20";
		delegate string dlgGetIDName(string name,int id);
		public void run()
		{
			//inner Decliment. Threading Method.
			dlgGetIDName thrMethod = delegate(string name, int id) {
				for (int i = 0; i < THREADWAITCOUNT; i++) {
					Thread.Sleep (THREADWAITTIME);
					Console.Write (".");
				}
				Console.WriteLine (".");
				return string.Format ("{0:D3}: {1}"+Environment.NewLine, id, DateTime.Now);
			};
			//inner CallBack Method.
			AsyncCallback thrCaller = delegate(IAsyncResult Array) {
				var result = thrMethod.EndInvoke(Array);
				var dTime = (DateTime)Array.AsyncState;
				Console.WriteLine ("StartTime: {0}, EndTime: {1}",dTime, result);
			};
			IAsyncResult thrState = thrMethod.BeginInvoke ("hoge", 200, thrCaller, DateTime.Now);
			Console.WriteLine ("Start Threading on {0}.",Name);

			thrState.AsyncWaitHandle.WaitOne ();
			Console.WriteLine ("Finish Thread on {0}.",Name);
		}
	}
	#endregion
	//Requierd Big Class.
	public class AsyncNet11APM : AsyncProgMethod
	{
		delegate string dlgGetIDName(string name,int id);
		dlgGetIDName getIdNameDalegate;
		//Waiting Thread Method
		string GetIdName(string name,int id){
			for (int i = 0; i < THREADWAITCOUNT; i++) {
				Thread.Sleep (THREADWAITTIME);
				Console.Write(".");
			}
			Console.WriteLine (".");
			return string.Format ("{0:D3}: {1}"+Environment.NewLine, id, DateTime.Now);
		}
		//Ather 2 method out Declier.
		public void run()
		{
			string Name = "Net11";
			var i = 100;
			//.Net1.1 Asynchronous Programing Model (APM)
			// Create Delegate
			getIdNameDalegate = new dlgGetIDName(GetIdName);
			Console.WriteLine ("Start Threading on " + Name);								
			IAsyncResult thStat = 
				getIdNameDalegate.BeginInvoke ("hoge", i, //GetIdName Arguments 
					//AsyncCallbak Delegate    CallBack Argument ar.AsyncState
					new AsyncCallback (thCallBack), DateTime.Now);
			//Thread Statment Property. Finish Thread Blocking.
			thStat.AsyncWaitHandle.WaitOne ();
			Console.WriteLine ("Finish Threading on " + Name);
			//Console.ReadKey ();
		}
		//AsyncCallback Method
		void thCallBack(IAsyncResult ar)
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

