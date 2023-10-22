﻿using ArmyServer.Controllers;
using ArmyServer.Utilities.HttpListenserWrapper;
using System.Net;
using System.Text;
using System.Text.Json;
using ArmyServer.Data;
using ArmyServer.Excetions;
using ArmyServer.Models;
using ArmyServer.Services.Friends;
using ArmyServer.Services.Leaderboard;
using ArmyServer.Services.Shop;

namespace ArmyServer
{
    class HttpServer
    {
        public static HttpListener listener;
        public static string url = "http://localhost:8000/";
        public static IConfiguration Configuration { get; private set; }
        // Controllers
        private static readonly ShopRepository ShopRepository = new();
        private static readonly AuthenticationController AuthController = new(new PlayerRepository());
        private static readonly ShopController ShopController = new (new ShopService(ShopRepository, new PlayerRepository()));
        private static readonly FriendsController FriendsController = new (new FriendsService(new FriendRepository()));
        private static readonly LeaderboardController LeaderboardController = new(new LeaderboardService(new LeaderboardRepository()));
        public HttpServer()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

        }

        private static void SetUpRepositories()
        {
            ShopRepository.Add("1",new ShopItem("1", "Cash 60", 999999, 0.99m));
            ShopRepository.Add("2",new ShopItem("2", "Cash 300", 999999, 4.99m));
            ShopRepository.Add("3",new ShopItem("3", "Cash 600", 999999, 9.99m));
            ShopRepository.Add("4",new ShopItem("4", "Cash 1200", 999999, 19.99m));
        }

        public static async Task HandleIncomingConnections()
        {
            bool runServer = true;
            SetUpRepositories();
            
            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                IHttpListenerRequestWrapper req = new HttpListenerRequestWrapper(ctx.Request);
                IHttpListenerResponseWrapper resp = new HttpListenerResponseWrapper(ctx.Response);

                // Print out some info about the request
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();


                if (req.Url.AbsolutePath == "/register" && req.HttpMethod == "GET")
                {
                    AuthController.GuestRegister(req, resp);
                }
                else if (req.Url.AbsolutePath == "/login" && req.HttpMethod == "POST")
                {
                    AuthController.Login(req, resp);
                }
                else if (req.Url.AbsolutePath == "/shop/items" && req.HttpMethod == "GET")
                {
                    ShopController.GetShopItems(req, resp);
                }
                else if (req.Url.AbsolutePath == "/shop/purchase" && req.HttpMethod == "POST")
                {
                    ShopController.PurchaseItem(req, resp);
                }
                else if (req.Url.AbsolutePath.StartsWith("/friends") && req.HttpMethod == "GET")
                {
                    FriendsController.GetFriends(req, resp);
                }
                else if (req.Url.AbsolutePath == "/friends" && req.HttpMethod == "POST")
                {
                    FriendsController.AddFriend(req, resp);
                }
                else if (req.Url.AbsolutePath == "/friends" && req.HttpMethod == "DELETE")
                {
                    FriendsController.RemoveFriend(req, resp);
                }
                else if (req.Url.AbsolutePath == "/leaderboard/top100" && req.HttpMethod == "GET")
                {
                    LeaderboardController.GetTopPlayers(req, resp);
                }
                else if (req.Url.AbsolutePath == "/leaderboard" && req.HttpMethod == "POST")
                {
                    LeaderboardController.AddOrUpdatePlayerScore(req, resp);
                }
                else
                {
                    // Write out to the response stream (asynchronously), then close it
                    string responseBody = JsonSerializer.Serialize(new ErrorResponse("Invalid request."));
                    var data = Encoding.UTF8.GetBytes(responseBody);
                    resp.ContentType = "application/json";
                    resp.ContentEncoding = Encoding.UTF8;
                    resp.StatusCode = (int)HttpStatusCode.BadRequest;
                    resp.OutputStream.Write(data, 0, data.Length);
                    resp.Close();
                }

                
                resp.Close();
            }
        }
        
        public static void SendError(IHttpListenerResponseWrapper resp, string message, HttpStatusCode statusCode)
        {
            string responseBody = JsonSerializer.Serialize(new ErrorResponse(message));
            SendResponse(resp, responseBody, statusCode);
        }
        
        public static void SendResponse(IHttpListenerResponseWrapper resp, string responseBody, HttpStatusCode statusCode)
        {
            var data = Encoding.UTF8.GetBytes(responseBody);
            resp.ContentType = "application/json";
            resp.ContentEncoding = Encoding.UTF8;
            resp.StatusCode = (int)statusCode;
            resp.OutputStream.Write(data, 0, data.Length);
        }


        public static void Main(string[] args)
        {
            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);

            // Handle requests
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            listener.Close();
        }
    }

}
