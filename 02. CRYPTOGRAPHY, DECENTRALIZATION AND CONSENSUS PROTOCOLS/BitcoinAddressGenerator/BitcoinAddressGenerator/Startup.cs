using System;
using System.Numerics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace BitcoinAddressGenerator
{
    public class Startup
    {
        private static void Main()
        {
            string hexHash = "0450863AD64A87AE8A2FE83C1AF1A8403CB53F53E486D8511DAD8A04887E5B23522CD470243453A299FA9E77237716103ABC11A1DF38855ED6F2EE187E9C582BA6";
            byte[] pubkey = HexToByte(hexHash);

            byte[] pubkeySha = Sha256(pubkey);

            byte[] pubkeyShaRipe = RipeMD160(pubkeySha);

            byte[] prehashWNetwork = AppendBitcoinNetwork(pubkeyShaRipe, 0);
            byte[] publicHash = Sha256(prehashWNetwork);

            byte[] publicHashHash = Sha256(publicHash);

            byte[] address = ConcatAddress(prehashWNetwork, publicHashHash);

            StringBuilder result = new StringBuilder();

            result.AppendLine("Public Key:" + ByteToHex(pubkey));
            result.AppendLine("Sha Public Key:" + ByteToHex(pubkeySha));
            result.AppendLine("Ripe Sha Public Key:" + ByteToHex(pubkeyShaRipe));
            result.AppendLine("Public Hash:" + ByteToHex(publicHash));
            result.AppendLine("Public Hash Hash:" + ByteToHex(publicHashHash));
            result.AppendLine("Checksum:" + ByteToHex(publicHashHash).Substring(0,4));
            result.AppendLine("Address:" + ByteToHex(address));
            result.Append("Human Address:" + Base58Encode(address));

            Console.WriteLine(result);

        }

        private static string ByteToHex(byte[] arr)
        {
            string hex = BitConverter.ToString(arr);
            return hex;
        }

        private static byte[] HexToByte(string hexString)
        {
            if(hexString.Length % 2 != 0)
            {
                throw new ArgumentException("Invalid Hex");
            }

            byte[] arr = new byte[hexString.Length / 2];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = byte.Parse(hexString.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return arr;
        }

        private static byte[] Sha256(byte[] array)
        {
            SHA256Managed sHA256Managed = new SHA256Managed();
            return sHA256Managed.ComputeHash(array);
        }

        private static byte[] RipeMD160(byte[] arrray)
        {
            RIPEMD160Managed rIPEMD160Managed = new RIPEMD160Managed();
            return rIPEMD160Managed.ComputeHash(arrray);
        }

        private static byte[] AppendBitcoinNetwork(byte[] ripeHash, byte network)
        {
            byte[] extended = new byte[ripeHash.Length + 1];
            extended[0] = network;
            Array.Copy(ripeHash, 0, extended, 1, ripeHash.Length);
            return extended;
        }

        private static byte[] ConcatAddress(byte[] ripeHash, byte[] checkSum)
        {
            byte[] arr = new byte[ripeHash.Length + 4];
            Array.Copy(ripeHash, arr, ripeHash.Length);
            Array.Copy(checkSum, 0, arr, ripeHash.Length, 4);
            return arr;
        }

        public static string Base58Encode(byte[] array)
        {
            string alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            string result = string.Empty;

            BigInteger encodedSize = alphabet.Length;
            BigInteger arrayToInt = 0;

            for (int i = 0; i < array.Length; i++)
            {
                arrayToInt = arrayToInt * 256 + array[i];
            }

            while (arrayToInt > 0)
            {
                int rem = (int)(arrayToInt % encodedSize);
                arrayToInt /= encodedSize;
                result = alphabet[rem] + result;
            }

            for (int i = 0; i < array.Length && array[i] == 0; ++i)
            {
                result = alphabet[0] + result;
            }

            return result;
        }
    }
}
