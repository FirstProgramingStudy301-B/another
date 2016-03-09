using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleStdInOut
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string recy = string.Empty;
			var childConsole = new ChildConsole (strLine =>{
				//Console.WriteLine(new String('-',80));
				//Console.WriteLine (strLine);
				recy += strLine + Environment.NewLine;
				//Console.WriteLine(new String('=',80));
			});

			childConsole.Start ();
			recy = string.Empty;
			childConsole.InputCommand ("pwd");
			Console.WriteLine ("<<<<"+recy+ ">>>>");
			recy = string.Empty;
			childConsole.InputCommand ("ls -la");
			Console.WriteLine ("<<<<"+recy+ ">>>>");
			recy = string.Empty;
			childConsole.InputCommand ("ps -aef");
			Console.WriteLine ("<<<<"+recy+ ">>>>");
			recy = string.Empty;
			childConsole.InputCommand ("exit");
			Console.WriteLine ("<<<<"+recy+ ">>>>");

			Console.ReadKey ();
		}

	}
}
