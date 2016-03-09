using System;

namespace DesignPattern00
{
	public class Gate
	{
		object lockObje = new object();
		int counter =0;
		string name = "Nobody";
		string address = "Nowhere";
		public void pass(string name,string address)
		{
			lock (lockObje) 
			{
				this.counter++;
				this.name = name;
				this.address = address;
				check ();
			}
		}
		public override string ToString ()
		{
			lock (lockObje) {
				return string.Format ("No. {0:d3}: {1,8}  {2,8}", counter, name, address);
			}
		}
		private void check()
		{
			if (name [0] != address [0]) {
				Console.WriteLine ("***** BROKEN ***** {0,22}", this.ToString ());
			}
			//Broken check out !!
//			} else {
//				Console.WriteLine("***** OK ***** {0,22}", this.ToString ());
//			}
		}
	}
}

 