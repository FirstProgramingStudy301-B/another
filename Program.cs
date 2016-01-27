using System;
using System.Threading;

namespace Async.Net3
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var oldAPM = new AsyncNet11APM ();
			//moust old APM
			oldAPM.run();

			var oldNet20 = new AsyncNet20APM ();
			oldNet20.run ();

			var win7Net35 = new AsyncNet35APM ();
			win7Net35.run ();

		}
	}
}
