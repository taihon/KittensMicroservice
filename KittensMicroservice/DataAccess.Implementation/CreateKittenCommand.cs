using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KittensMicroservice.Viewmodels;
using KittensMicroservice.Models;
using KittensMicroservice.Services;

namespace KittensMicroservice.DataAccess.Implementation
{
    public class CreateKittenCommand : ICreateKittenCommand
    {
        IDataService DataService { get; }
        public CreateKittenCommand(IDataService dataService)
        {
            DataService = dataService;
        }
        public async Task ExecuteAsync(CreateKittenRequest request, string owner)
        {
            //use automapper here?
            var kitten = new Kitten { Name = request.Name, Color = request.Color, CreatedBy = owner };
            await DataService.SaveDataAsync(kitten);
        }
    }
}
