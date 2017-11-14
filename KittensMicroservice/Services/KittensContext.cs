using KittensMicroservice.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KittensMicroservice.Services
{
    public class KittensContext:DbContext
    {
        public KittensContext(DbContextOptions options):base(options)
        {

        }
        public KittensContext()
        {

        }
        public DbSet<Kitten> Kittens { get; set; }
    }
}
