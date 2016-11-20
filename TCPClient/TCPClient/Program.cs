using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Client Cl = new Client("192.168.4.1",55555);

            byte[] data = {0xff, 0x6f, 0x0b, 0x32, 0x07, 0x9c, 0xfe, 0x08, 0x38, 0xfe, 0x02 };
            //char[] data = { (char)0x7f, (char)0xef, (char)0xff };



            while (true)
            {
                Cl.Send(data);
                Console.WriteLine(Cl.read());
                System.Threading.Thread.Sleep(500);
            }
            //Cl.Recive();
            //Console.WriteLine(i);
        }
    }
}
