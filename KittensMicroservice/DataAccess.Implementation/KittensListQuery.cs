using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KittensMicroservice.Viewmodels;
using KittensMicroservice.Services;

namespace KittensMicroservice.DataAccess.Implementation
{
    public class KittensListQuery : IKittensListQuery
    {
        public KittensListQuery(IDataService dataService)
        {
            DataService = dataService;
        }

        private IDataService DataService { get; }

        public async Task<IEnumerable<KittenResponse>> RunAsync()
        {
            return (await DataService.GetDataAsync()).Select(
                k=>new KittenResponse
                {
                    Name = k.Name,
                    Owner = k.CreatedBy,
                    Color = k.Color
                });
        }
    }
}
