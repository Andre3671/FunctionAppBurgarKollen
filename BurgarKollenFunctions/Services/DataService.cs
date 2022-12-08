using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BurgarKollen.Lib2;
using BurgarKollen_Lib2;

namespace BurgarKollenFunctions.Services
{
    public class DataService
    {

        private readonly MongoDataService _context;

        public DataService(MongoDataService context)
        {
            _context = context;
        }

        public Restaurant? GetRestaurant(string id)
        {
            return _context.GetQuerableCollection<Restaurant>().FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<Restaurant>> GetAllRestaurantsAsync()
        {
            var list = await _context.GetAllAsync<Restaurant>();
            foreach (var restaurant in list)
            {
                var ratings = _context.GetQuerableCollection<UserRating>().Where(x => x.Restaurant.Id == restaurant.Id).ToList();
                if (ratings.Count() > 0)
                {
                    int ratingsum = 0;
                    foreach (var rating in ratings)
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
            return list;
        }

        public async Task<bool> SlugExistsAsync(string name)
        {
            return _context.GetQuerableCollection<Restaurant>().Any(x => x.Name == name);
        }

        public async Task<Restaurant> CreateRestaurantAsync(Restaurant Restaurant)
        {
            await _context.AddUpdateAsync(Restaurant);
            // await _context.SaveChangesAsync();
            return Restaurant;
        }

        public async Task UpdateRestaurantAsync(Restaurant Restaurant)
        {
            await _context.AddUpdateAsync(Restaurant);
            //await _context.SaveChangesAsync();
        }

        public async Task<UserRating> CreateUserRating(UserRating rating)
        {
            rating.DateAdded = DateTime.Now;
            await _context.AddUpdateAsync(rating);

            return rating;
        }

        public async Task<UserRating> EditUserRating(UserRating rating)
        {
            rating.DateAdded = DateTime.Now;
            await _context.AddUpdateAsync(rating);
            return rating;
        }

        public UserRating? CheckUserRating(string user, string restaurant)
        {

           
            if (_context.GetQuerableCollection<UserRating>().Where(x => x.UserId == user && x.Restaurant.Id == restaurant).Any())
            {
                return _context.GetQuerableCollection<UserRating>().Where(x => x.UserId == user && x.Restaurant.Id == restaurant).FirstOrDefault();
            }
            else
            {
                return null;
            }

        }

        public async Task<bool> CheckUserFavorite(string user, string restaurant)
        {
            var fav = await GetUserFavoriteAsync(user);
            if (fav.Restaurants.Any(x => x.Id == restaurant))
            {
                return true;
            }
            else
            {
                return false;
            }
            

        }

        public async Task<Userfavorit> AddUserFavorite(Userfavorit favorite)
        {
            await _context.AddUpdateAsync(favorite);

            return favorite;

        }

        public async Task<Userfavorit> RemoveUserFavorite(Userfavorit favorite)
        {
           
            await _context.AddUpdateAsync(favorite);

            return favorite;

        }

        public async Task<List<UserRating>> GetAllUserRatingsAsync(string id)
        {
            return _context.GetQuerableCollection<UserRating>().Where(x => x.Restaurant.Id == id).ToList();
        }

        public async Task<Userfavorit?> GetUserFavoriteAsync(string id)
        {
            return _context.GetQuerableCollection<Userfavorit>().FirstOrDefault(x => x.UserId == id);
        }
    }
}
