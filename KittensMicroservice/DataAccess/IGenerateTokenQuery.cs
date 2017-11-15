using KittensMicroservice.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KittensMicroservice.DataAccess
{
    interface IGetTokenQuery
    {
        Task<object> RunAsync(GetTokenRequest request);
    }
}
