using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Server
{ 
    static async Task Main()
    {
        const string message = "Hello UDP client ";
        const int port = 5588;
        UdpClient udpServer = null;
        Random rnd = new Random();

        try
        {
            udpServer = new UdpClient(port);
            Console.WriteLine($"Server started. IP Address: 192.168.56.101, Port: {port}");

            while (true)
            {
                UdpReceiveResult result = await udpServer.ReceiveAsync();
                byte[] receiveBytes = result.Buffer;
                IPEndPoint clientEndPoint = result.RemoteEndPoint;

                string data = Encoding.ASCII.GetString(receiveBytes);

                if (data.ToUpper().Equals("GET"))
                {
                    var newMessage = message + rnd.Next(1000000);
                    byte[] sendBytes = Encoding.ASCII.GetBytes(newMessage);
                    await udpServer.SendAsync(sendBytes, sendBytes.Length, clientEndPoint);
                    Console.WriteLine("Sent to client: " + newMessage);
                }
                else
                {
                    Console.WriteLine("Received zeros: " + data);
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