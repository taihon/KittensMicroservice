using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KittensMicroservice.Models
{
    public class GetTokenRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        //vague description in assignment - is this field required or optional?
        public int? ExpiresIn { get; set; }
    }
}
