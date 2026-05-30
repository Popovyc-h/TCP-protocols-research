using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client;

internal class Program
{
    private static void ReceiveMessages(Socket socket)
    {
        var buffer = new byte[1024];

        while (true)
        {
            int receive = socket.Receive(buffer);
            string message = Encoding.UTF8.GetString(buffer, 0, receive);
            Console.WriteLine(message);
        }
    }
    
    private static void SendMessages(Socket socket)
    {
        while (true)
        {
            Console.Write("\nВи: ");
            string message = Console.ReadLine();

            var sendBytes = Encoding.UTF8.GetBytes(message);

            socket.Send(sendBytes);
        }
    }

    static void Main(string[] args)
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var endpoint = new IPEndPoint(IPAddress.Loopback, 8888);

        socket.Connect(endpoint);
        
        Console.WriteLine($"Підключено до Chat Server ({endpoint.Address}:{endpoint.Port})");
        Console.Write($"Введіть ваше ім'я: ");
        string name = Console.ReadLine();

        var nameBytes = Encoding.UTF8.GetBytes(name);
        socket.Send(nameBytes);
        Console.WriteLine("Ви приєдналися до чату!");

        var receiveThread = new Thread(() => ReceiveMessages(socket));
        var sendThread = new Thread(() => SendMessages(socket));
        receiveThread.Start();
        sendThread.Start();
        receiveThread.Join();
    }
}
