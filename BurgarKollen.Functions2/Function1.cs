using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BurgarKollen.Functions2.Services;
using BurgarKollen.Lib2;
using System.Net;
using System.Xml.Linq;

namespace BurgarKollen.Functions2
{
    public class Function1
    {
        private readonly DataService _dataService;
        public Function1(DataService dataService)
        {
            _dataService = dataService;
        }

        [FunctionName("GetAllRestaurants")]
        public async Task<IActionResult> GetAllRestaurants(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {



            log.LogInformation("Getting all Restaurants");
            var test = await _dataService.GetAllRestaurantsAsync();
            return new OkObjectResult(test);

        }

        [FunctionName("AddRestaurant")]
        public async Task<IActionResult> AddRestaurant(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            Restaurant tempRest = new Restaurant()
            {
                Name = req.Query["name"],
                Address = req.Query["address"],
                Description = req.Query["description"],
                Rating = int.Parse(req.Query["rating"]),
                ImageLink = req.Query["imagelink"],
                City = req.Query["city"]

            };

            log.LogInformation("Getting all Restaurants");
            var test = await _dataService.CreateRestaurantAsync(tempRest);
            return new OkObjectResult(test);
        }

        [FunctionName("AddUserRating")]
        public async Task<IActionResult> AddUserRating(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var rating = JsonConvert.DeserializeObject<UserRating>(requestBody);
            log.LogInformation("Getting all Restaurants");
            var test = await _dataService.CreateUserRating(rating);
            return new OkObjectResult(test);
        }

        [FunctionName("EditUserRating")]
        public async Task<IActionResult> EditUserRating(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var rating = JsonConvert.DeserializeObject<UserRating>(requestBody);
            var test = await _dataService.EditUserRating(rating);
            return new OkObjectResult(test);
        }

        [FunctionName("CheckUserRating")]
        public async Task<IActionResult> CheckUserRating(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var user = req.Query["userid"];
             var restaurant = req.Query["restaurantid"];

           
            var test = await _dataService.CheckUserRating(user,int.Parse(restaurant));
            return new OkObjectResult(test);
        }

        [FunctionName("CheckUserFavorite")]
        public async Task<IActionResult> CheckUserFavorite(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var user = req.Query["userid"];
            var restaurant = req.Query["restaurantid"];


            var test = await _dataService.CheckUserFavorite(user, int.Parse(restaurant));
            return new OkObjectResult(test);
        }

        [FunctionName("AddUserFavorite")]
        public async Task<IActionResult> AddUserFavorite(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var rating = JsonConvert.DeserializeObject<UserFavorit>(requestBody);
            var test = await _dataService.AddUserFavorite(rating);
            return new OkObjectResult(test);
        }

        [FunctionName("RemoveUserFavorite")]
        public async Task<IActionResult> RemoveUserFavorite(
       [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var rating = JsonConvert.DeserializeObject<UserFavorit>(requestBody);
            var test = await _dataService.RemoveUserFavorite(rating);
            return new OkObjectResult(test);
        }

        [FunctionName("GetAllUserFavorite")]
        public async Task<IActionResult> GetAllUserFavorite(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            string userid = req.Query["userid"];

            log.LogInformation("Getting all favorites");
            var test = await _dataService.GetAllUserFavoritesAsync(userid);
            return new OkObjectResult(test);
        }

        [FunctionName("GetAllUserRatings")]
        public async Task<IActionResult> GetAllUserRatings(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            int ResturantId = int.Parse(req.Query["restaurantid"]);

            log.LogInformation("Getting all Restaurants");
            var test = await _dataService.GetAllUserRatingsAsync(ResturantId);
            return new OkObjectResult(test);
        }
    }
}
