using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("Local IP Address Not Found!");
    }

    static void Main()
    {
        const string message = "Hello client";
        const int port = 6013;
        TcpListener? tcpListener = null;

        try
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            Console.WriteLine($"Server started. IP Address: {GetLocalIPAddress()}, Port: {port}");

            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();

                NetworkStream stream = tcpClient.GetStream();

                byte[] receiveBuffer = new byte[1024];
                int bytesRead = stream.Read(receiveBuffer, 0, receiveBuffer.Length);
                string data = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);

                if (data.ToUpper().Equals("GET"))
                {
                    byte[] sendBytes = Encoding.ASCII.GetBytes(message);
                    stream.Write(sendBytes, 0, sendBytes.Length);
                    Console.WriteLine("Sent to client: " + message);
        
                    byte[] zeroData = new byte[256];
                    bytesRead = stream.Read(zeroData, 0, zeroData.Length);
                    string zerosReceived = Encoding.ASCII.GetString(zeroData, 0, bytesRead);
                    Console.WriteLine("Received zeros from client: " + zerosReceived);
                }
                else
                {
                    Console.WriteLine($"Received unknown command: {data}");
                }

                stream.Close();
                tcpClient.Close();
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine($"SocketException: {e}");
        }
        finally
        {
            tcpListener?.Stop();
        }
    }
}