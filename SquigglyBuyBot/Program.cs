using System;

namespace SquigglyBuyBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Web3Handler.PrivateKey = args[0];
            Loop.UpdateServiceCheckLoop().GetAwaiter().GetResult();
        }
    }
}
