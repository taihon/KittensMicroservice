using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KittensMicroservice.Viewmodels
{
    public class CreateKittenRequest
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
