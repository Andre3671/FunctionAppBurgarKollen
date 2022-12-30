using System.Net;
using System.Net.Http.Json;
using BurgarKollen.Lib2;
using BurgarKollen_Lib2;
using BurgarKollenFunctions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace BurgarKollenFunctions
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly DataService _dataService;
        public Function1(DataService dataService, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _dataService = dataService;
        }


        [Function("GetRestaurant")]
        public async Task<HttpResponseData> GetRestaurant(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req)
        {
            var resturant = await req.ReadAsStringAsync();
            Restaurant? rest = _dataService.GetRestaurant(resturant ?? "");

            var resp = req.CreateResponse(HttpStatusCode.OK);
            await resp.WriteAsJsonAsync(rest);
            return resp;
        }


        [Function("GetAllRestaurants")]
        public async Task<HttpResponseData> GetAllRestaurants(
             [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("Getting all Restaurants");
            var test = await _dataService.GetAllRestaurantsAsync();
            var resp = req.CreateResponse(HttpStatusCode.OK);
            await resp.WriteAsJsonAsync(test);
            return resp;

        }



        [Function("AddRestaurant")]
        public async Task<HttpResponseData> AddTestRestaurant(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req)
        {
            var resturant = await req.ReadFromJsonAsync<Restaurant>();


            _logger.LogInformation("Getting all Restaurants");
            var test = await _dataService.CreateRestaurantAsync(resturant);
            return req.CreateResponse(HttpStatusCode.OK);
        }



        [Function("AddUserRating")]
        public async Task<HttpResponseData> AddUserRating(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req)
        {
            var rating = await req.ReadFromJsonAsync<UserRating>();
            //var rating = JsonConvert.DeserializeObject<UserRating>(requestBody);
            _logger.LogInformation("Getting all Restaurants");
            var test = await _dataService.CreateUserRating(rating);
            return req.CreateResponse(HttpStatusCode.OK);
        }

        [Function("EditUserRating")]
        public async Task<HttpResponseData> EditUserRating(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, ILogger log)
        {
            var rating = await req.ReadFromJsonAsync<UserRating>();
            var test = await _dataService.EditUserRating(rating);
            return req.CreateResponse(HttpStatusCode.OK);

        }



        [Function("CheckUserRating")]
        public async Task<HttpResponseData> CheckUserRating(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, ILogger log)
        {
            //     var user = req.Query["userid"];
            //    var restaurant = req.Query["restaurantid"];


            //    var test = await _dataService.CheckUserRating(user, int.Parse(restaurant));

            return req.CreateResponse(HttpStatusCode.OK);
        }

        [Function("CheckUserFavorite")]
        public async Task<HttpResponseData> CheckUserFavorite(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, ILogger log)
        {
            //     var user = req.Query["userid"];
            //     var restaurant = req.Query["restaurantid"];


            //    var test = await _dataService.CheckUserFavorite(user, int.Parse(restaurant));
            return req.CreateResponse(HttpStatusCode.OK);
        }

        [Function("AddUserFavorite")]
        public async Task<Userfavorit> AddUserFavorite(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            using var reader = new StreamReader(req.Body);
            var bodyContent = await reader.ReadToEndAsync();
            if(bodyContent != null)
            {
                Userfavorit favorit = Newtonsoft.Json.JsonConvert.DeserializeObject<Userfavorit>(bodyContent);
                var fav = await _dataService.GetUserFavoriteAsync(favorit.UserId);

                if (fav == null)
                {
                    List<Restaurant> list = new List<Restaurant>();
                    if (favorit.Restaurants is not null)
                    {
                        list.AddRange(favorit.Restaurants);

                    }
                    await _dataService.AddUserFavorite(new Userfavorit { UserId = favorit.UserId, Restaurants = list });
                }
                else
                {
                    foreach (var favRest in favorit.Restaurants)
                    {
                        if (!fav.Restaurants.Any(x => x.Id == favRest.Id))
                        {
                            fav.Restaurants.Add(favRest);
                        }
                    }
                    await _dataService.AddUserFavorite(fav);
                }
                HttpResponseData req2;

               
                return fav;
            }






           
            return null;
        }

        [Function("RemoveUserFavorite")]
        public async Task<HttpResponseData> RemoveUserFavorite(
       [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "RemoveUserFavorite/{favorit}")] HttpRequestData req, Userfavorit favorit)
        {
            //var favorit = await req.ReadFromJsonAsync<Userfavorit>();
            var fav = await _dataService.GetUserFavoriteAsync(favorit.UserId);
            if (fav != null)
            {
                foreach (var favRest in favorit.Restaurants)
                {
                    if (!fav.Restaurants.Any(x => x.Id == favRest.Id))
                    {
                        fav.Restaurants.Remove(favRest);
                    }
                }
                await _dataService.RemoveUserFavorite(fav);
            }

            return req.CreateResponse(HttpStatusCode.OK);
        }

        [Function("GetAllUserFavorite")]
        public async Task<HttpResponseData> GetAllUserFavorite(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "GetAllUserFavorite/User/{userid}")] HttpRequestData req,string userid)
        {
            //var userid = await req.ReadAsStringAsync();
            //     string userid = req.Query["userid"];
            if (!String.IsNullOrEmpty(userid)) {
                _logger.LogInformation("Getting all favorites");
                Userfavorit fav = await _dataService.GetUserFavoriteAsync(userid);
                var resp = req.CreateResponse(HttpStatusCode.OK);
                await resp.WriteAsJsonAsync(fav);
                return resp;

            }
            else
            {
                 
                var resp = req.CreateResponse(HttpStatusCode.OK);
                await resp.WriteAsJsonAsync(new Userfavorit() { Id = "", Restaurants = new List<Restaurant>(), UserId = "" });

                return resp;
            }
           
        }

        [Function("GetAllUserRatings")]
        public async Task<HttpResponseData> GetAllUserRatings(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req)
        {
            // int ResturantId = int.Parse(req.Query["restaurantid"]);
            var ResturantId = await req.ReadAsStringAsync();
            _logger.LogInformation("Getting all Restaurants");
            var test = await _dataService.GetAllUserRatingsAsync(ResturantId);
            var resp = req.CreateResponse(HttpStatusCode.OK);
            await resp.WriteAsJsonAsync(test);
            return resp;
        }


    }


}


