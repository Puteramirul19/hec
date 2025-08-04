using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Entities
{
    public class Encryption
    {
        public static bool CompareToCipher(string plainText, byte[] cipher)
        {
            /* Another way is to encrypt attempted and compare bytes 
               Not sure which one faster. */
            return plainText == Decrypt(cipher);
        }

        protected static SymmetricAlgorithm algorithm = BuildAlgorithm();

        private static SymmetricAlgorithm BuildAlgorithm()
        {
            var algo = RijndaelManaged.Create();

            algo.Key = Encoding.UTF8.GetBytes("tWqQf5S;[@b}Dt7h");
            algo.IV = Encoding.UTF8.GetBytes("G7eQ[+%v%TP$9Hmi");

            Array.Reverse(algo.Key);
            Array.Reverse(algo.IV);

            return algo;
        }

        public static byte[] Encrypt(string clearText)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                CryptoStream cs = new CryptoStream(ms,
                                                    algorithm.CreateEncryptor(),
                                                    CryptoStreamMode.Write);

                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(clearText);
                    sw.Close();
                    return ms.ToArray();
                }
            }
        }

        public static string Decrypt(byte[] cipherText)
        {
            using (MemoryStream ms = new MemoryStream(cipherText))
            {
                CryptoStream cs = new CryptoStream(ms,
                                            algorithm.CreateDecryptor(),
                                            CryptoStreamMode.Read);
                using (StreamReader sr = new StreamReader(cs))
                {
                    string val = sr.ReadToEnd();
                    sr.Close();
                    return val;
                }
            }
        }
    }
}
