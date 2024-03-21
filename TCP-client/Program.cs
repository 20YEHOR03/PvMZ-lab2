using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main(string[] args)
    {
        const int port = 6013;
        const string ip = "192.168.0.101";

        try
        {
            while (true)
            {
                Console.WriteLine("Enter command (GET to receive message from server, QUIT to exit):");
                string command = Console.ReadLine();

                if (command.ToUpper() == "QUIT")
                    break;
                if (command.ToUpper() == "GET")
                {
                    using (TcpClient tcpClient = new TcpClient(ip, port))
                    {
                        NetworkStream stream = tcpClient.GetStream();

                        byte[] receiveBuffer = new byte[1024];
                        int bytesRead = stream.Read(receiveBuffer, 0, receiveBuffer.Length);
                        string receivedData = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);

                        Console.WriteLine($"Server response: {receivedData}");

                        int length = receivedData.Length;
                        Console.WriteLine("Message length: " + length);
                        string zeros = new string('0', length);
                        byte[] zeroData = Encoding.ASCII.GetBytes(zeros);
                        stream.Write(zeroData, 0, zeroData.Length);
                        Console.WriteLine("Sent zeros: " + zeros);

                        stream.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Unknown command. Please enter GET or QUIT.");
                }
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine($"SocketException: {e}");
        }
    }
}