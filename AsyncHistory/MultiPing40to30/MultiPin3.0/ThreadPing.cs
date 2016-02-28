using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace MultiPin3._0
{
    class ThreadPing
    {
        IPAddress _IPaddress;
        bool _Completed;
        Ping _Pin;
        string _ReplyStat;
        public ManualResetEvent PinResetEvent;
        Action<string, string> _ShowResuletAction;
        private void Init(Action<string,string> Act)
        {
            _ShowResuletAction = Act;
            _ReplyStat = string.Empty;
            
            _Completed = false;
            _Pin = new Ping();
            _Pin.PingCompleted += _Pin_PingCompleted;
            this.PinResetEvent = new ManualResetEvent(false);
        }
        public ThreadPing(string ip,Action<string,string> act)
        {
            if (!IPAddress.TryParse(ip, out _IPaddress))
            {
                IPAddress.TryParse("0.0.0.0",out _IPaddress);
            }
            Init(act);
        }
        public ThreadPing(IPAddress ip ,Action<string,string> act)
        {
            _IPaddress = ip;
            Init(act);
        }
        public void Start()
        {
            _Pin.SendAsync(_IPaddress, null);
            this.PinResetEvent.Reset();
        }
        public string ReplyStat
        {
            get { return _ReplyStat; }
            
        }
        public bool Completed
        {
            get { return _Completed; }
        }
        public void Cancel()
        {
            if(_Pin != null)
            {
                _Pin.SendAsyncCancel();
            }
        }
        
        private void _Pin_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                _ReplyStat = e.Error.Message;
            }else if(e.Cancelled )
            {
                _ReplyStat = "User Canceled";
            }else { 
            
            _ReplyStat = e.Reply.Status == IPStatus.Success ? "OK" : "NG";
            }
            
            _Completed = true;
            //Console.WriteLine("{0}: {1}",_IPaddress,this.ReplyStat);
            _ShowResuletAction(_IPaddress.ToString(), this.ReplyStat);
            this.PinResetEvent.Set();
            
        }
    }
}
