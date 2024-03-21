using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener server = null;
const int port = 6000;
const string ip = "127.0.0.1";

try {
    server = new TcpListener(IPAddress.Parse(ip), port);
    server.Start();
    string ipAddress = GetLocalIPAddress();
    Console.WriteLine("Server started at IP: " + ipAddress);

    while (true) {
        Console.WriteLine("Waiting for a client to connect...");
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Client connected...");
        
        Thread clientThread = new Thread(HandleClient);
        clientThread.Start(client);
    }
} 
catch (Exception e) {
    Console.WriteLine("Error: " + e.Message);
    server?.Stop();
}

static string GetLocalIPAddress() {
    var host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (var ip in host.AddressList) {
        if (ip.AddressFamily == AddressFamily.InterNetwork) {
            return ip.ToString();
        }
    }
    throw new Exception("Local IP Address Not Found!");
}

static void HandleClient(object obj) {
    TcpClient client = (TcpClient)obj;
    try {
        NetworkStream stream = client.GetStream();
        
        string message = "Hello from server!";
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);
        Console.WriteLine("Sent to client: " + message);
        
        byte[] zeroData = new byte[256];
        int bytesRead = stream.Read(zeroData, 0, zeroData.Length);
        string zerosReceived = Encoding.ASCII.GetString(zeroData, 0, bytesRead);
        Console.WriteLine("Received zeros from client: " + zerosReceived);
        
        stream.Close();
        client.Close();
    } 
    catch (Exception e) {
        Console.WriteLine("Error: " + e.Message);
        client.Close();
    }
}