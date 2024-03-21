using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static void Main(string[] args)
    {
        const string message = "Hello client";
        const int port = 5000;
        UdpClient udpServer = null;

        try
        {
            udpServer = new UdpClient(port);
            Console.WriteLine($"Server started. IP Address: {Dns.GetHostAddresses(Dns.GetHostName())[0]}, Port: {port}");

            while (true)
            {
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] receiveBytes = udpServer.Receive(ref clientEndPoint);
                string data = Encoding.ASCII.GetString(receiveBytes);

                if (data.Equals("GET"))
                {
                    byte[] sendBytes = Encoding.ASCII.GetBytes(message);
                    udpServer.Send(sendBytes, sendBytes.Length, clientEndPoint);
                }
                else
                {
                    Console.WriteLine($"Received zeros: {data}");
                }
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine($"SocketException: {e}");
        }
        finally
        {
            udpServer?.Close();
        }
    }
}