using System;
using System.Numerics;
using Nethereum.Web3;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3.Accounts;
using Nethereum.Util;
using System.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Newtonsoft.Json.Linq;
namespace SquigglyBuyBot
{
    public class Web3Handler
    {
        public static string PrivateKey;

        public static async Task<int> GetGas(string method)
        {
            string data = "";
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                try
                {
                    data = await wc.DownloadStringTaskAsync("https://ethgasstation.info/api/ethgasAPI.json");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            var json = JObject.Parse(data);
            return ((int)json[method]) / 10;
        }

        public static async Task<string> GetInAuctionSquiggly(int price = 0)
        {
            var account = new Account(PrivateKey);
            var web3 = new Web3(account, "https://mainnet.infura.io/v3/941d6f5ccab944728642af2ab67aea3a");
            var contract = "0x15be1f472b0fab90f532ee4d51c3e2bcdb24d37b";
            var con = web3.Eth.GetContractHandler(contract);
            var f = con.GetFunction<EndAndSecure>();
            var functionParams = new EndAndSecure();
            functionParams.GasPrice = Web3.Convert.ToWei(await GetGas("fast"), UnitConversion.EthUnit.Gwei);
            functionParams.Gas = new BigInteger(4500000);
            Console.WriteLine(f.GetData(functionParams));
            var tx = await f.SendTransactionAsync(
                functionParams,
                account.Address,
                new HexBigInteger(functionParams.Gas.Value),
                new HexBigInteger(functionParams.GasPrice.Value),
                new HexBigInteger(Web3.Convert.ToWei(price, UnitConversion.EthUnit.Ether)));
            return "https://etherscan.io/tx/" + tx;
        }

        public static async Task<int> GeteTimeOfLastStart()
        {
            Web3 web3;
            web3 = new Web3("https://mainnet.infura.io/v3/b4e2781f02a94a5a96dcf8ce8cab9449");
            var handler = web3.Eth.GetContractQueryHandler<BlockTimeCounter>();
            var param = new BlockTimeCounter();
            var time = await handler.QueryAsync<int>("0x36f379400de6c6bcdf4408b282f8b685c56adc60", param);
            return time;
        }

    }
    [Function("endAndSecure")]
    class EndAndSecure : FunctionMessage
    {
    }

    [Function("blockTimeCounter", "uint256")]
    class BlockTimeCounter : FunctionMessage
    {
    }

}
