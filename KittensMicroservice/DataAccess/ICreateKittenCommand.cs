using KittensMicroservice.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KittensMicroservice.DataAccess
{
    interface ICreateKittenCommand
    {
        Task ExecuteAsync(CreateKittenRequest request);
    }
}
