using System;
using System.Collections.Generic;
using System.Threading;

namespace DesignPattern00
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			
			Console.WriteLine ("Any Key To Exit");
			var threads = new List<UseThread> ();
			var Gate = new Gate ();
			threads.Add (new UseThread (Gate, "Alice", "Alaska"));
			threads.Add (new UseThread (Gate, "Bobby", "Brazil"));
			threads.Add (new UseThread (Gate, "Chris", "Canada"));

			foreach (var thread in threads) {
				thread.Run ();
			}
			Console.ReadKey ();
			foreach (var thread in threads) {
				thread.Stop ();
			}

		}
	}
}
