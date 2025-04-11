using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EstateMapperClient.Services
{
    public class WindowsSecureStorage : ISecureStorageService
    {
        // 使用Windows DPAPI加密存储
        public void SaveToken(string token)
        {
            var entropy = CreateEntropy();
            var encryptedData = ProtectedData.Protect(
                Encoding.UTF8.GetBytes(token),
                entropy,
                DataProtectionScope.CurrentUser);

            // 存储到安全位置
            var filePath = GetTokenFilePath();
            File.WriteAllBytes(filePath, encryptedData);
            File.WriteAllBytes(filePath + ".entropy", entropy);
        }

        public string GetToken()
        {
            try
            {
                var filePath = GetTokenFilePath();
                if (!File.Exists(filePath)) return null;

                var encryptedData = File.ReadAllBytes(filePath);
                var entropy = File.ReadAllBytes(filePath + ".entropy");

                var decryptedData = ProtectedData.Unprotect(
                    encryptedData,
                    entropy,
                    DataProtectionScope.CurrentUser);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return null;
            }
        }

        public void RemoveToken()
        {
            var filePath = GetTokenFilePath();
            if (File.Exists(filePath)) File.Delete(filePath);
            if (File.Exists(filePath + ".entropy"))
                File.Delete(filePath + ".entropy");
        }

        private byte[] CreateEntropy()
        {
            // 生成随机熵值增强安全性
            var entropy = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(entropy);
            }
            return entropy;
        }

        private string GetTokenFilePath()
        {
            var appData = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(appData, "estate", "auth.token");
        }

        public bool TryParseToken(string token, out JwtPayload payload)
        {
            payload = null;
            try
            {
                var handler = new JwtSecurityTokenHandler();
                if (!handler.CanReadToken(token)) return false;

                var jwtToken = handler.ReadJwtToken(token);
                payload = jwtToken.Payload;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
