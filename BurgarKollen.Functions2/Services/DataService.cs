using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BurgarKollen.Lib2;
using FunctionAppBurgarKollen.Migrations;
using Microsoft.EntityFrameworkCore;

namespace BurgarKollen.Functions2.Services
{
    public class DataService
    {

        private readonly AppDbContext _context;

        public DataService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Restaurant?> GetRestaurantAsync(int id)
        {
            return await _context.Restaurants.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Restaurant>> GetAllRestaurantsAsync()
        {
            var list = await _context.Restaurants.ToListAsync();
            foreach(var restaurant in list)
            {
                var ratings = _context.UserRatings.Where(x => x.ResturantId == restaurant.Id).ToList();
                if(ratings.Count() > 0) { 
                int ratingsum = 0;
                foreach(var rating in ratings)
                {
                    ratingsum = ratingsum + rating.Rating;
                }
                restaurant.Rating = ratingsum / ratings.Count();
                }
                else
                {
                    restaurant.Rating = 0;
                }
            }
            return await _context.Restaurants.ToListAsync();
        }

        public async Task<bool> SlugExistsAsync(string name)
        {
            return await _context.Restaurants.AnyAsync(x => x.Name == name);
        }

        public async Task<Restaurant> CreateRestaurantAsync(Restaurant Restaurant)
        {
            _context.Restaurants.Add(Restaurant);
            await _context.SaveChangesAsync();
            return Restaurant;
        }

        public async Task UpdateRestaurantAsync(Restaurant Restaurant)
        {
            _context.Restaurants.Update(Restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task<UserRating> CreateUserRating(UserRating rating)
        {
            rating.DateAdded = DateTime.Now;
            _context.UserRatings.Add(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<UserRating> EditUserRating(UserRating rating)
        {
            rating.DateAdded = DateTime.Now;
            _context.UserRatings.Update(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<UserRating> CheckUserRating(string user,int restaurant)
        {
           if(_context.UserRatings.Where(x => x.UserId == user && x.ResturantId == restaurant).Any()){
                return _context.UserRatings.Where(x => x.UserId == user && x.ResturantId == restaurant).FirstOrDefault();
            }
            else
            {
                return null;
            }
            
        }

        public async Task<bool> CheckUserFavorite(string user, int restaurant)
        {
            if (_context.UserFavorits.Where(x => x.UserId == user && x.RestaurantId == restaurant).Any())
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<UserFavorit> AddUserFavorite(UserFavorit favorite)
        {
            _context.UserFavorits.Add(favorite);
            await _context.SaveChangesAsync();
            return favorite;

        }

        public async Task<UserFavorit> RemoveUserFavorite(UserFavorit favorite)
        {
          var Userfavorite = _context.UserFavorits.Where(u => u.UserId == favorite.UserId && u.RestaurantId == favorite.RestaurantId).FirstOrDefault();
            _context.UserFavorits.Remove(Userfavorite);
            await _context.SaveChangesAsync();
            return favorite;

        }

        public async Task<List<UserRating>> GetAllUserRatingsAsync(int id)
        {
            return await _context.UserRatings.Where(x => x.ResturantId == id).ToListAsync();
        }

        public async Task<List<UserFavorit>> GetAllUserFavoritesAsync(string id)
        {
            return await _context.UserFavorits.Where(x => x.UserId == id).ToListAsync();
        }
    }
}
