using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Integrations
{
    public interface ISspService
    {
        bool VerifyToken(string userName, string userToken);
        string BuildTokenVerificationUrl(string userName, string userToken);
        string GenerateTokenSignature(string userName, string userToken);
    }

    public class FakeSspService : ISspService
    {
        public bool VerifyToken(string userName, string userToken)
        {
            return true;
        }

        public string BuildTokenVerificationUrl(string userName, string userToken)
        {
            return "FAKE";
        }

        public string GenerateTokenSignature(string userName, string userToken)
        {
            return "FAKE";
        }
    }

    public class SspVerifyTokenResult
    {
        public string NAME { get; set; }
        public string STATUS_CODE { get; set; }
        public string STATUS_DESC { get; set; }
    }

    public class SspService : ISspService
    {
        private string tokenVerificationUrl;
        private string channelKey;
        private string actionKey;
        private string encryptionKey;

        private ILogger logger = LogManager.GetLogger("integration");

        public SspService(string tokenVerificationUrl, string channelKey, string actionKey, string encryptionKey)
        {
            this.tokenVerificationUrl = tokenVerificationUrl;
            this.channelKey = channelKey;
            this.actionKey = actionKey;
            this.encryptionKey = encryptionKey;
        }

        public bool VerifyToken(string userName, string userToken)
        {
            using (var client = new WebClient())
            {
                var url = BuildTokenVerificationUrl(userName, userToken);

                logger.Trace($"[SSP Request] Verifying userName: {userName} , userToken: {userToken} , url: {url}");

                var json = client.DownloadString(url);

                logger.Trace($"[SSP Response] {json}");

                var result = JsonConvert.DeserializeObject<SspVerifyTokenResult>(json);

                // Note: StatusCode 99 – Error, 01 – Token Active, 02 – Token Inactive
                return (result.STATUS_CODE == "01");
            }
        }

        public string BuildTokenVerificationUrl(string userName, string userToken)
        {
            // Sample Request Message:
            // http://test.mytnb.com.my:8011/SSO/WebApi/TokenVerification?CHANNEL_KEY=SSP_SSO_HEC&ACTION_KEY=T_VERIFY&USERTOKEN=62CA5E2A8C4F48539AE69BCC966EFD7F&SIGNATURE=mw+dVCiiWt5zCfJ6C3FVqOojWdjVpOkDNhnuOqjsT1J72ajquZ43S6CmJR6LqsXW3W7fG+rTqI9SFEk7xb8u5U1RUzHK0RMrODmR/Q0RuRGIEdpN/vI3pxXof5b7vBOj

            return $"{tokenVerificationUrl}?CHANNEL_KEY={channelKey}&ACTION_KEY={actionKey}&USERTOKEN={userToken}&SIGNATURE={GenerateTokenSignature(userName, userToken)}";
        }

        public string GenerateTokenSignature(string userName, string userToken)
        {
            if (String.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");

            if (String.IsNullOrEmpty(userToken))
                throw new ArgumentNullException("userToken");

            return PHP_AES256_Encrypt(this.encryptionKey, userName.ToUpper() + "|" + userToken.ToUpper());
        }

        public static string PHP_AES256_Encrypt(string passPhrase, string plainText)
        {
            string saltValue = "Ivan Medvedev";
            string hashAlgorithm = "SHA1";
            int passwordIterations = 1000;
            string initVector = "pOWaTbO92LfXbh69JkYzfT7P465TNc0h";
            int keySize = 32;

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(passPhrase, saltValueBytes, passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize);

            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.BlockSize = 256;
            symmetricKey.KeySize = 256;
            symmetricKey.Padding = PaddingMode.Zeros;
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();

            return Convert.ToBase64String(cipherTextBytes);
        }
    }
}
