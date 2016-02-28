using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace MultiPing
{
    class AsyncBiginDNS
    {
        static ManualResetEvent allDone;
        //public Run()
        public void Run()
        {
            allDone = new ManualResetEvent(false);
            // Create an instance of the PinRequestState class.
            PinRequestState myRequestState = new PinRequestState();

            // Begin an asynchronous request for information like host name, IP addresses, or 
            // aliases for specified the specified URI.
            IAsyncResult asyncResult = Dns.BeginGetHostEntry("www.contiioso.com", new AsyncCallback(RespCallback), myRequestState);

            // Wait until asynchronous call completes.
            allDone.WaitOne();
            //asyncResult.AsyncWaitHandle.WaitOne();
            if (myRequestState.host == null) return;
            if (asyncResult.IsCompleted) Console.WriteLine("Get IPs!!");
            Console.WriteLine("Host name : " + myRequestState.host.HostName);
            Console.WriteLine("\nIP address list : ");
            for (int index = 0; index < myRequestState.host.AddressList.Length; index++)
            {
                Console.WriteLine(myRequestState.host.AddressList[index]);
            }
            Console.WriteLine("\nAliases : ");
            for (int index = 0; index < myRequestState.host.Aliases.Length; index++)
            {
                Console.WriteLine(myRequestState.host.Aliases[index]);
            }
        }
         // private CallBack
        void RespCallback(IAsyncResult ar)
        {

            try
            {
                // Convert the IAsyncResult object to a PinRequestState object.
                PinRequestState tempRequestState = (PinRequestState)ar.AsyncState;
                // End the asynchronous request.
                tempRequestState.host = Dns.EndGetHostEntry(ar);
                //allDone.Set();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }
            finally
            {
                if (allDone != null) allDone.Set();

            }
        }
    }
}
