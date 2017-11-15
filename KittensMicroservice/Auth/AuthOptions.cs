using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KittensMicroservice.Auth
{
    public class AuthOptions
    {
        public const string Issuer = "ikit-mita";
        public const string Audience = "ikit-mita";
        const string Key = "ikitmita-micro-service";
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
