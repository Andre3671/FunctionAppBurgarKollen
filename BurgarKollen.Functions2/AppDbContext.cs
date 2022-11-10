
using BurgarKollen.Lib2;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgarKollen.Functions2
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

        }

        public DbSet<Restaurant> Restaurants {get; set;}
        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<UserFavorit> UserFavorits { get; set; }
    }
}
