using System;
using System.Threading.Tasks;
using System.IO;
using System.Text;
namespace SquigglyBuyBot
{
    public class Loop
    {
        public static async Task UpdateServiceCheckLoop()
        {
            while (true)
            {
                int unixTime = Convert.ToInt32(((DateTimeOffset)(DateTime.UtcNow)).ToUnixTimeSeconds());
                Console.Clear();
                int lastAuctionTime = await Web3Handler.GeteTimeOfLastStart();
                Console.WriteLine($"Time before next update :  {(lastAuctionTime - unixTime)} seconds");
                if (unixTime - lastAuctionTime > 0)
                {
                    try
                    {
                        await Web3Handler.GetInAuctionSquiggly();
                        await Task.Delay(60000 * 15);
                    }
                    catch (Exception e) {
                        Console.WriteLine(e.Message);
                    }
                }
                await Task.Delay(60000 * 5);
            }
        }
    }
}
