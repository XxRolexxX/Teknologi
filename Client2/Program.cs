using System.Net;
using System.Net.Sockets;
using System.Text;

public class Program
{
    static Socket sck;
    public static void Main(string[] args)
    {
        sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress selectedIP = GetIPAddress();
        IPEndPoint localEndPoint = new IPEndPoint(selectedIP, 1234);

        try
        {
            sck.Connect(localEndPoint);
        }
        catch
        {
            Console.WriteLine("Unable to connect");
            Main(args);
        }

        Console.WriteLine("Enter text: ");
        string text = Console.ReadLine();
        byte[] data = Encoding.ASCII.GetBytes(text);

        sck.Send(data);
        Console.WriteLine("Data sent!");
        Console.WriteLine("Press any key");
        Console.ReadLine();
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