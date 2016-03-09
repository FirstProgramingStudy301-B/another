using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleStdInOut
{
	public class ChildConsole
	{
		ManualResetEvent ReadResetEvent = new ManualResetEvent (false);
		ManualResetEvent InputResetEvent = new ManualResetEvent (false);
		System.IO.StreamWriter stdInputStream = null;
		System.IO.StreamReader stdOutStream = null;
		System.IO.StreamReader stdErrorStream = null;
		Process _process = null;
		bool isExited = false;
		private  Action<string> ShowCommandResult;
		public ChildConsole (Action<string> showCmdRes)
		{
			ShowCommandResult = showCmdRes ?? (_ => {} );
			_process = new Process ();
			_process.StartInfo.FileName = Environment.CurrentDirectory;
			_process.StartInfo.FileName = "/bin/sh";
			//Console.WriteLine (process.StartInfo.FileName);
			_process.StartInfo.UseShellExecute = false;
			_process.StartInfo.RedirectStandardOutput = true;
			_process.StartInfo.RedirectStandardInput = true;
			_process.StartInfo.RedirectStandardError = true;
			_process.StartInfo.CreateNoWindow = true;
			_process.StartInfo.Arguments = "";
		}
		void InitStreams(){
			this.stdInputStream = _process.StandardInput;
			this.stdInputStream.AutoFlush = true;
			this.stdOutStream = _process.StandardOutput;
			this.stdErrorStream = _process.StandardError;
		}
		public void Start(){
			_process.Start ();
			InitStreams ();
			ThreadPool.QueueUserWorkItem (_ => {
				while (true) {
					ReadResetEvent.WaitOne();
					ReadResetEvent.Reset();
					ShowCommandResult(stdOutStream.ReadLine());
					//Thread.Sleep(300);
					stdOutStream.InitializeLifetimeService();
					while (stdOutStream.Peek() > 0) {
						var res = stdOutStream.ReadLine ();
						ShowCommandResult(res);
					}
					InputResetEvent.Set();
					if(isExited) break;
				}
				_process.WaitForExit ();
				_process.Close ();

			});
		}
		public void InputCommand(string cmd){
			try {
				Console.WriteLine ("----  StartCommand: {0}   ---",cmd);
				if (cmd != string.Empty)stdInputStream.WriteLine (cmd);
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}finally{
				cmd = cmd.ToUpper ();
				if (cmd == "EXIT")isExited = true;
				ReadResetEvent.Set ();
				InputResetEvent.WaitOne ();
				InputResetEvent.Reset ();
				Console.WriteLine ("----  End of InputCommand   ---");
			}

		}
	}
}

