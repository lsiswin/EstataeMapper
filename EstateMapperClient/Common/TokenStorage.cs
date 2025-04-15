using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EstateMapperClient
{
    public static class TokenStorage
    {
        // 存储到本地文件（加密）
        private static readonly string TokenFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "EstateMaper",
        "token.dat");

        //保存Token
        public static void SaveToken(string token)
        {
            var encryptedData = ProtectedData.Protect(
            Encoding.UTF8.GetBytes(token),
            optionalEntropy: null,
            scope: DataProtectionScope.CurrentUser);
            if (!Directory.Exists(Path.GetDirectoryName(TokenFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(TokenFilePath));
            }
            File.WriteAllBytes(TokenFilePath, encryptedData);
        }
        //读取token
        public static string ReadToken()
        {
            if (!File.Exists(TokenFilePath))
            {
                return null;
            }
            var encryptedData = File.ReadAllBytes(TokenFilePath);
            var decryptedData = ProtectedData.Unprotect(
                encryptedData,
                optionalEntropy: null,
                scope: DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decryptedData);
        }
        //删除token
        public static void DeleteToken()
        {
            if (File.Exists(TokenFilePath))
            {
                File.Delete(TokenFilePath);
            }
        }
    }
}
