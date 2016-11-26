using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*ネットワーク宣言*/
using System.IO;
using System.Net;
using System.Net.Sockets;

/**********************************************/
/* WROOM-02とのTCPソケット通信用クラス        */
/*                                            */
/* WROOM-02をアクセスポイントとサーバーとして */
/* 動作させてC#側のクライアントプログラムで   */
/* ソケット通信を行う                         */
/*                                            */
/* WROOM-02仕様　　　　　　　　　　　　　　　 */
/* IPアドレス:192.168.4.1                     */
/* ポート番号:55555                           */
/* SSID:wroom                                 */
/* PASSWORD:robotismini                       */
/*                                            */
/**********************************************/

namespace TCPClient
{
    class Client
    {
        TcpClient client;
        /*クラス全体でストリームライターが使えるようにする*/
        private StreamWriter sw;
        private StreamReader sr;

        public Client(string ipAddress, int portNum)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), portNum);

            client = new TcpClient();

            /*サーバーに接続できたかの判定*/
            try
            {
                client.Connect(ep);     //接続の開始　Connect(繋ぐ)
                Console.WriteLine("接続された");	//接続された時の表示
                NetworkStream ns = client.GetStream();
                sr = new StreamReader(ns);  //読み込み
                sw = new StreamWriter(ns);   //文字コードを指定して送信
                sw.AutoFlush = true;    //一行書き込んだら送信する

                /*マルチスレットを立ち上げる*/
                Task.Factory.StartNew(() => Recive());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);   //接続できなかった場合の処理
            }
        }

        /*データの送信*/
        public void Send(byte[] data)
        {
            char[] num;

            if (sw != null)
            {
                num = Conversion(data);
                //Console.WriteLine(num);
                sw.Write(num);
            }
        }

        /*送信するbyte列の分解*/
        private char[] Conversion(byte[] data)
        {
            char[] num = new char[data.Length*2];
            int i,j;

            for (i = 0, j = 0; i < data.Length; i++, j += 2)
            {
                num[j] = (char)((data[i] >> 4)+0x10);
                num[j + 1] = (char)((data[i] & 0x0f) + 0x20);
            }

            return num;
        }

        //string str = string.Empty;
        byte[] ReadData = new byte[1024];
        Boolean readflg = false;
        byte dataH;
        byte dataL;

        /*受信処理*/
        private void Recive()
        {
            int cnt = 0;
            byte str;

            do
            {
                str = (byte)sr.Read();
                if (str == 0)
                {
                    break;
                }

                SetData(cnt, str);
                if (str != '\n')
                {
                    cnt++;
                }
                else
                {
                    if(cnt != 0)
                    {
                        ReadData[cnt] = str;
                    }
                    cnt = 0;
                    readflg = true;
                }
            } while (true);
        }

        private void SetData(int cnt,byte data)
        {

            if ((data & 0x10) == 0x10)
            {
                dataH = data;
            }
            else if ((data & 0x20) == 0x20)
            {
                dataL = data;
            }

            if (dataH != 0 && dataL != 0)
            {
                ReadData[cnt] = (byte)((dataH << 4) | (dataL & 0x0f));
                dataH = 0;
                dataL = 0;
            }
        }

        /*データの受信*/
        public byte[] read()
        {
            if(readflg)
            {
                readflg = false;
                return ReadData;
            }
            /*受信がなければnullを返す*/
            return null;
        }

        /*ソケットのクローズ*/
        public void ClientClose()
        {
            try
            {
               client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);	//閉じることができなかった時の表示
            }
        }
    }
}
