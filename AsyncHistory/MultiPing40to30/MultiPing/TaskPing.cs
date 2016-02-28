using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace MultiPing
{
    class TaskPing
    {
        public void Run()
        {
            var scop = "172.29.111.";
            //var scop = "216.239.32.";
            Console.WriteLine(scop + 81);
            //var tRes = await taskPing(scop + "81");
            //Console.WriteLine(await tRes);
            var ress = Enumerable.Range(0, 254).AsParallel().Select(async i => await taskPing(scop + i));
            foreach (var itm in ress)
            {
                Console.WriteLine(itm.Result.ToString());
            }
            Console.ReadKey();
        }
        async Task<string> taskPing(string ip)
        {
            IPAddress ipAdr = null;
            var ipchk = IPAddress.TryParse(ip, out ipAdr);
            if (ipchk == false) return string.Format("{0}: Can not Convert IPAddress", ip);
            string res = string.Empty;
            try
            {
                var reply = await (new Ping()).SendPingAsync(ipAdr, 1000);
                res = reply.Status == IPStatus.Success ? "OK" : "NG";
            }
            catch (Exception ex)
            {
                return string.Format("{0}: {1}", ip, ex.Message);
            }
            string[] ippar = ip.Split(new char[] { '.' });
            return string.Format("{0,3}.{1,3}.{2,3}.{3,3}: {4}", ippar[0],ippar[1],ippar[2],ippar[3], res);
        }
    }
}
