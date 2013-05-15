using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

public class EchoServer {
   
    public static int Port = 8080;
    
    static void Main() {
        var server = new EchoServer();
        new Thread(server.Start).Start();

        var client = new Client { Message = "hello world" };
        new Thread(client.Start).Start();

        var client2 = new Client { Message = "foo bar" };
        new Thread(client2.Start).Start();
    }

    public void Start() {
        var server = TcpListener.Create(Port);
        server.Start();
        while (true) {
            var i = 0;
            string data = null;
            var bytes = new byte[256];
            var client = server.AcceptTcpClient();
            var stream = client.GetStream();
            while((i = stream.Read(bytes, 0, bytes.Length)) != 0) 
            {   
                data = Encoding.ASCII.GetString(bytes, 0, i);
                data = data.ToUpper();
                var msg = Encoding.ASCII.GetBytes(data);
                stream.Write(msg, 0, msg.Length);
            }
        }
    }
}

public class Client {
    
    public string Message { get; set; }

    public void Start() {
        var client = new TcpClient("localhost", EchoServer.Port);
        var data = Encoding.ASCII.GetBytes(Message);
        var stream = client.GetStream();
        stream.Write(data, 0, data.Length);
        data = new byte[256];
        String responseData = String.Empty;
        var bytes = stream.Read(data, 0, data.Length);
        responseData = Encoding.ASCII.GetString(data, 0, bytes);
        Console.WriteLine("Echo: {0}", responseData);
        client.Close();
    }
}