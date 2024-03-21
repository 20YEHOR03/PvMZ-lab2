using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static void Main()
    {
        const string message = "Hello TCP client ";
        const int port = 6013;
        TcpListener? tcpListener = null;
        Random rnd = new Random();

        try
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            Console.WriteLine($"Server started. IP Address: 192.168.56.101, Port: {port}");

            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();

                NetworkStream stream = tcpClient.GetStream();
                
                var newMessage = message + rnd.Next(1000000);
                byte[] sendBytes = Encoding.ASCII.GetBytes(newMessage);
                stream.Write(sendBytes, 0, sendBytes.Length);
                Console.WriteLine("Sent to client: " + newMessage);

                byte[] zeroData = new byte[256];
                int bytesRead = stream.Read(zeroData, 0, zeroData.Length);
                string zerosReceived = Encoding.ASCII.GetString(zeroData, 0, bytesRead);
                Console.WriteLine("Received zeros from client: " + zerosReceived);
                
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