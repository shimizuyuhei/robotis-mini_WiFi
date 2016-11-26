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

            byte[] data1 = { 0xff, 0x6b, 0x09, 0x01, 0x03, 0x64, 0x00, 0x64, 0x9f };

            byte[] data2 = { 0xff, 0x6b, 0x09, 0x01, 0x03, 0x64, 0x64, 0xc8, 0x57 };

            Cl.Send(data1);
            Cl.Send(data2);
            while (true)
            {
                
                byte[] s;
                s = Cl.read();

                if (s != null)
                {
                    for (int i = 0; s[i] != '\n'; i++)
                    {
                        Console.WriteLine(s[i]);
                    }
                }
                
                System.Threading.Thread.Sleep(500);
            }
            //Cl.Recive();
            //Console.WriteLine(i);
        }
    }
}
