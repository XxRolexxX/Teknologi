public class Program
{

    public static void Main(string[] args)
    {
        string mySum1 = "", mySum2 = "", mySum3 = "";

        Thread thread1 = new Thread(() => mySum1 = Work.DoWork());
        Thread thread2 = new Thread(() => mySum2 = Work.DoWork());
        Thread thread3 = new Thread(() => mySum3 = Work.DoWork());

        thread1.Start();
        thread2.Start();
        thread3.Start();

        thread1.Join();
        thread2.Join();
        thread3.Join();

        Console.WriteLine($"Thread 1 Sum: {mySum1}");
        Console.WriteLine($"Thread 2 Sum: {mySum2}");
        Console.WriteLine($"Thread 3 Sum: {mySum3}");

        Console.WriteLine($"MainSum: {Work.MainSum}");
    }
}


public class Work
{
    private static readonly object lockObj = new object();

    public static int MainSum = 0;

    public static string DoWork()
    {
        int mySum = 0;
        Random random = new Random();

        for (int i = 0; i < 1000; i++)
        {
            int tal = random.Next(0, 9);
            mySum += tal;

            lock (lockObj)
            {
                MainSum += tal;
            }
            
        }

        return mySum.ToString();
    }
}