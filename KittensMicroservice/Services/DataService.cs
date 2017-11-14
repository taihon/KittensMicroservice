using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KittensMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace KittensMicroservice.Services
{
    public class DataService : IDataService
    {
        KittensContext DbContext { get; }
        public DataService(KittensContext dbContext) => DbContext = dbContext;
        public async Task<IEnumerable<Kitten>> GetDataAsync()
        {
            return await DbContext.Kittens.ToListAsync();
        }

        public async Task SaveDataAsync(Kitten kitten)
        {
            await DbContext.Kittens.AddAsync(kitten);
            await DbContext.SaveChangesAsync();
        }
    }
}
