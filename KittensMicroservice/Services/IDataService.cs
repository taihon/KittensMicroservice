using KittensMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KittensMicroservice.Services
{
    public interface IDataService
    {
        Task<IEnumerable<Kitten>> GetDataAsync();
        Task SaveDataAsync(Kitten kitten);
    }
}
