
using BurgarKollenFunctions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgarKollenFunctions
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }


        public DbSet<Restaurant> Restaurants {get; set;}
        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<UserFavorit> UserFavorits { get; set; }
        public DbSet<Restaurant2> Restaurants2 { get; set; }
    }
}
