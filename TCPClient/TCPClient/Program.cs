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

            byte[] data = { 0xff, 0x67, 0x05, 0xfe, 0x63 };
            //char[] data = { (char)0x7f, (char)0xef, (char)0xff };

            Cl.Send(data);
            System.Threading.Thread.Sleep(500);

            byte[] data1 = { 0xff, 0x6b, 0x09, 0x01, 0x03, 0x64, 0x64, 0xc8, 0x57 };

            

            while (true)
            {
                Cl.Send(data1);
                String s;
                /*s = Cl.read();
                
                if (s != null)
                {
                    Console.WriteLine(s);
                }*/
                
                System.Threading.Thread.Sleep(500);
            }
            //Cl.Recive();
            //Console.WriteLine(i);
        }
    }
}
