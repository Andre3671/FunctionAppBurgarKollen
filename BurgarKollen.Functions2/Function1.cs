using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;


using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BurgarKollenFunctions.Functions2.Services;
using BurgarKollenFunctions.Lib2;
using System.Net;
using System.Xml.Linq;

namespace BurgarKollenFunctions.Functions2
{
    public class Function1
    {
        private readonly DataService _dataService;
        public Function1(DataService dataService)
        {
            _dataService = dataService;
        }

        [FunctionName("GetAllRestaurants")]
        public async Task<HttpResponseData> GetAllRestaurants(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, ILogger log)
        {

            
           

            log.LogInformation("Getting all Restaurants");
            var test = await _dataService.GetAllRestaurantsAsync();
            return req.CreateResponse(HttpStatusCode.OK) ;

        }

        [FunctionName("AddRestaurant")]
        public async Task<HttpResponseData> AddRestaurant(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "/{restaurant:alpha}")] HttpRequestData req,Restaurant restaurant, ILogger log)
        {
            //Restaurant tempRest = new Restaurant()
            //{
            //    Name = req.Body["name"],
            //    Address = req.Query["address"],
            //    Description = req.Query["description"],
            //    Rating = int.Parse(req.Query["rating"]),
            //    ImageLink = req.Query["imagelink"],
            //    City = req.Query["city"]

            //};

            log.LogInformation("Getting all Restaurants");
            var test = await _dataService.CreateRestaurantAsync(restaurant);
            return req.CreateResponse(HttpStatusCode.OK);
        }

        [FunctionName("AddUserRating")]
        public async Task<HttpResponseData> AddUserRating(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var rating = JsonConvert.DeserializeObject<UserRating>(requestBody);
            log.LogInformation("Getting all Restaurants");
            var test = await _dataService.CreateUserRating(rating);
            return req.CreateResponse(HttpStatusCode.OK);
        }

        [FunctionName("EditUserRating")]
        public async Task<HttpResponseData> EditUserRating(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var rating = JsonConvert.DeserializeObject<UserRating>(requestBody);
            var test = await _dataService.EditUserRating(rating);
            return req.CreateResponse(HttpStatusCode.OK);
        }

        //[FunctionName("CheckUserRating")]
        //public async Task<HttpResponseData> CheckUserRating(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, ILogger log)
        //{
        //    var user = req.Query["userid"];
        //     var restaurant = req.Query["restaurantid"];

           
        //    var test = await _dataService.CheckUserRating(user,int.Parse(restaurant));
        //    return req.CreateResponse(HttpStatusCode.OK);
        //}

        //[FunctionName("CheckUserFavorite")]
        //public async Task<HttpResponseData> CheckUserFavorite(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, ILogger log)
        //{
        //    var user = req.Query["userid"];
        //    var restaurant = req.Query["restaurantid"];


        //    var test = await _dataService.CheckUserFavorite(user, int.Parse(restaurant));
        //    return req.CreateResponse(HttpStatusCode.OK);
        //}

        [FunctionName("AddUserFavorite")]
        public async Task<HttpResponseData> AddUserFavorite(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var rating = JsonConvert.DeserializeObject<UserFavorit>(requestBody);
            var test = await _dataService.AddUserFavorite(rating);
            return req.CreateResponse(HttpStatusCode.OK);
        }

        [FunctionName("RemoveUserFavorite")]
        public async Task<HttpResponseData> RemoveUserFavorite(
       [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var rating = JsonConvert.DeserializeObject<UserFavorit>(requestBody);
            var test = await _dataService.RemoveUserFavorite(rating);
            return req.CreateResponse(HttpStatusCode.OK);
        }

        //[FunctionName("GetAllUserFavorite")]
        //public async Task<HttpResponseData> GetAllUserFavorite(
        // [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, ILogger log)
        //{
        //    string userid = req.Query["userid"];

        //    log.LogInformation("Getting all favorites");
        //    var test = await _dataService.GetAllUserFavoritesAsync(userid);
        //    return req.CreateResponse(HttpStatusCode.OK);
        //}

        //[FunctionName("GetAllUserRatings")]
        //public async Task<HttpResponseData> GetAllUserRatings(
        // [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, ILogger log)
        //{
        //    int ResturantId = int.Parse(req.Query["restaurantid"]);

        //    log.LogInformation("Getting all Restaurants");
        //    var test = await _dataService.GetAllUserRatingsAsync(ResturantId);
        //    return req.CreateResponse(HttpStatusCode.OK);
        //}
    }
}
