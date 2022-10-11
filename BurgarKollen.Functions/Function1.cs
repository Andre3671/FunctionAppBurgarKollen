using System.Collections.Generic;
using System.Net;
using BurgarKollen.Lib2;
using FunctionAppBurgarKollen.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BurgarKollen.Functions
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

            //UserRating tempRest = new UserRating()
            //{
            //    ResturantId = int.Parse(req.Query["restaurantid"]),
            //    Rating = int.Parse(req.Query["rating"]),


            //};

            log.LogInformation("Getting all Restaurants");
            var test = await _dataService.CreateUserRating(rating);
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
