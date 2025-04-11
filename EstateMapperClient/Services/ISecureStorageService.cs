using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateMapperClient.Services
{
    public interface ISecureStorageService
    {
        bool TryParseToken(string token, out JwtPayload payload);
        void SaveToken(string token);
        string GetToken();
        void RemoveToken();
    }
}
