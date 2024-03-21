using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main(string[] args)
    {
        const int port = 5588;
        const string ip = "192.168.56.101";

        try
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(ip, port);

            while (true)
            {
                Console.WriteLine("Enter command (GET to receive message from server, QUIT to exit):");
                string command = Console.ReadLine();

                if (command.ToUpper() == "QUIT")
                    break;
                if (command.ToUpper() == "GET")
                {
                    byte[] sendBytes = Encoding.ASCII.GetBytes(command);
                    udpClient.Send(sendBytes, sendBytes.Length);

                    IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receiveBytes = udpClient.Receive(ref serverEndPoint);
                    string receivedData = Encoding.ASCII.GetString(receiveBytes);

                    Console.WriteLine($"Server response: {receivedData}");
                
                    int length = receivedData.Length;
                    Console.WriteLine("Message length: " + length);
                    string zeros = new string('0', length);
                    byte[] zeroData = Encoding.ASCII.GetBytes(zeros);
                    udpClient.Send(zeroData, zeroData.Length);
                    Console.WriteLine("Sent zeros: " + zeros);
                }
                else
                {
                    Console.WriteLine($"Received unknown command: {command}");
                }
            }
            udpClient.Close();
        }
        catch (SocketException e)
        {
            Console.WriteLine($"SocketException: {e}");
        }
    }
}