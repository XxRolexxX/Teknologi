using System.Net;
using System.Net.Sockets;
using System.Text;

public class Program
{
    static byte[] buffer {  get; set; }
    static Socket Sck;

    public static void Main(string[] args)
    {
        Sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress selectedIP = GetIPAddress();
        Sck.Bind(new IPEndPoint(selectedIP, 1234));
        Sck.Listen(100);

        Socket accept = Sck.Accept();
        buffer = new byte[accept.SendBufferSize];
        int bytesRead = accept.Receive(buffer);
        byte[] formatted = new byte[bytesRead];
        for (int i = 0; i < bytesRead; i++) 
        {
            formatted[i] = buffer[i];
        }

        string strData = Encoding.ASCII.GetString(formatted);
        Console.WriteLine(strData + "\r\n");

        if (strData == "STOP")
        {
            Console.WriteLine("Server closing");
            Console.Read();
            Sck.Close();
            accept.Close();
        }

        string text = "Data receieved!";
        byte[] data = Encoding.ASCII.GetBytes(text);

        Sck.Send(data);

        Console.Read();
        Sck.Close();
        accept.Close();

    }


    private static IPAddress GetIPAddress()
    {
        //Pre : None
        //Post: From a menu showing all IPv4 addresses one is selected and returned
        int counter = 1;
        List<IPAddress> myIP4s = LoadIPv4();
        foreach (IPAddress ip in myIP4s)
        {
            ChColor(ConsoleColor.DarkYellow, ConsoleColor.Black);
            Console.Write($" {counter}: ");
            ChColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine($" {ip} ");
            counter++;
        }
        Console.Write("\n\n Input number : ");
        int choice = 1; //default, only changed if user press a number
        if (Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out choice))
        {
            Console.Clear();
            return myIP4s[choice - 1];
        }
        else
        {
            throw new FormatException();
        }
    }

    private static List<IPAddress> LoadIPv4()
    {
        //Pre : None
        //Post: All IPv4 (= AddressFamily.InterNetwork) addresses
        //      on host is returned in a List
        string hostName = Dns.GetHostName();
        IPAddress[] myIPs = Dns.GetHostEntry(hostName).AddressList;
        List<IPAddress> myIP4s = new List<IPAddress>();
        foreach (IPAddress ip in myIPs)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                myIP4s.Add(ip);
            }
        }
        return myIP4s;
    }

    private static void ChColor(ConsoleColor bg, ConsoleColor fg)
    {
        //Pre : None
        //Post: Changed colors to bg and fg
        Console.BackgroundColor = bg;
        Console.ForegroundColor = fg;
    }
}