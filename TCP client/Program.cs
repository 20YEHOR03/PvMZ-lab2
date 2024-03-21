using System.Net.Sockets;
using System.Text;

const string ip = "127.0.0.1";
const int port = 6000;

try {
    TcpClient client = new TcpClient(ip, port);
    Console.WriteLine("Connected to server...");
    
    NetworkStream stream = client.GetStream();
    
    byte[] data = new byte[256];
    StringBuilder response = new StringBuilder();
    int bytes = stream.Read(data, 0, data.Length);
    response.Append(Encoding.ASCII.GetString(data, 0, bytes));
    Console.WriteLine("Received from server: " + response);
    
    int length = response.Length;
    Console.WriteLine("Message length: " + length);
    string zeros = new string('0', length);
    byte[] zeroData = Encoding.ASCII.GetBytes(zeros);
    stream.Write(zeroData, 0, zeroData.Length);
    Console.WriteLine("Zeros: " + zeros);
    
    stream.Close();
    client.Close();
} 
catch (Exception e) {
    Console.WriteLine("Error: " + e.Message);
}