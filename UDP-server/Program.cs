using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static string GetLocalIPAddress() {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList) {
            if (ip.AddressFamily == AddressFamily.InterNetwork) {
                return ip.ToString();
            }
        }
        throw new Exception("Local IP Address Not Found!");
    }
    
    static void Main(string[] args)
    {
        const string message = "Hello client";
        const int port = 5588;
        UdpClient udpServer = null;

        try
        {
            udpServer = new UdpClient(port);
            Console.WriteLine($"Server started. IP Address: {GetLocalIPAddress()}, Port: {port}");

            while (true)
            {
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] receiveBytes = udpServer.Receive(ref clientEndPoint);
                string data = Encoding.ASCII.GetString(receiveBytes);

                if (data.ToUpper().Equals("GET"))
                {
                    byte[] sendBytes = Encoding.ASCII.GetBytes(message);
                    udpServer.Send(sendBytes, sendBytes.Length, clientEndPoint);
                    Console.WriteLine("Sent to client: " + message);
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