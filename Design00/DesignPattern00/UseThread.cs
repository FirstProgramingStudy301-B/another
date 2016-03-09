using System;
using System.Threading;
namespace DesignPattern00
{
	public class UseThread
	{
		bool _ThreadStop = false;
		readonly Gate _gate;
		readonly string _myName;
		readonly string _myAddress;
		public UseThread(Gate gate,string name ,string address)
		{
			this._gate = gate;
			this._myAddress = address;
			this._myName = name;

		}
		public void Run(){
			Console.WriteLine (_myName + " BEGIN");
			ThreadPool.QueueUserWorkItem (_ => {
				while (true)
				{
					_gate.pass (_myName, _myAddress);
					if(this._ThreadStop) break;
				}		
			});
		}
		public void Stop()
		{
			this._ThreadStop = true;
		}
	}
}

