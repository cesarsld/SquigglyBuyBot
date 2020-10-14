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
            int safetyCheck = 0;
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
                        safetyCheck++;
                        Console.WriteLine(e.Message);
                        if (safetyCheck == 5)
                            return;
                    }
                }
                await Task.Delay(60000 * 5);
            }
        }
    }
}
