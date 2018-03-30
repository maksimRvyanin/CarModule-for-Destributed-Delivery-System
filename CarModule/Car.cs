using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CarModule
{
    class Car
    {
        public String transportId { get; private set; } //The car's Id, which can help identify the car from others.
        public Int32 Capacity { get; set; }//How much items the car can take.
        public List<Product> Products { get; set; }//products in the car.
        public Int32 Speed { get; private set; }//The car's speed, which depends on road length.
        public String PointTo { get; set; }//Point, in which the car goes
        public String PointFrom { get; set; }//Point, from which car goes.
        public Int32 RoadPercent { get; set; }//Percentage of travel.
        //Setting for lowercase
        public static JsonSerializerSettings _jsonSerializerSettings =
            new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
        //Connection to RabbitMQ
        private ConnectionFactory factory = new ConnectionFactory
        {
            UserName = "test",
            Password = "Zxvcasfd",
            HostName = "51.144.119.126",
            Port = 15672
        };
        public Car(String id, Int32 capacity)
        {
            transportId = id;
            Capacity = capacity;
            Products = new List<Product>();
            Speed = 10;
            RoadPercent = 0;
        }
        #region Communication with StorageService
        /// <summary>
        /// Параметры запроса на забор товаров у склада: "capacity": int (nullable),"accessiblePoints": [str] (nullable)
        /// </summary>
        public static void GetProductsFromStorage(Car car)
        {
            var requestString = @"http://188.225.9.3/storages/" + car.PointFrom + "/products/prepare";
            var json = "{\"capacity\": " + car.Capacity + "}";
            while (!car.Products.Any())
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestString);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "*/*";
                var body = Encoding.UTF8.GetBytes(json);
                using (Stream stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(body, 0, json.Length);
                    stream.Close();
                }
                string responseJson;
                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        responseJson = streamReader.ReadToEnd();
                    }
                    responseJson = responseJson.Substring(12, responseJson.Length - 13);
                    car.Products = JsonConvert.DeserializeObject<List<Product>>(responseJson);
                }
                catch (Exception e)
                {
                    //Если ошибка, то просто съедаем и работаем дальше, но в лог записываем
                    Console.WriteLine(e);
                }
            }
            car.PointTo = car.Products[0].Destination;
        }
        public static void PutProductsToStorage(Car car)
        {
            var requestString = @"http://188.225.9.3/storages/" + car.PointTo + "/products";
            var productsJson = JsonConvert.SerializeObject(car.Products, _jsonSerializerSettings);
            string json = "{\"products\": " + productsJson + ", \"transportId\": \"" + car.transportId + "\"}";
            while (car.Products.Any())
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestString);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "*/*";
                var body = Encoding.UTF8.GetBytes(json);
                using (Stream stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(body, 0, json.Length);
                    stream.Close();
                }
                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    if(httpResponse.StatusCode.ToString()=="OK")
                    {
                        Console.WriteLine("All goods were put in storage {0}", car.PointTo);
                    }
                    car.Products.Clear();
                }
                catch (Exception e)
                {
                    //Если ошибка, то просто съедаем и работаем дальше, но в лог записываем
                    Console.WriteLine(e);
                }
            }
            car.PointFrom = car.PointTo;
        }
        #endregion

        #region Communication with ProductService
        public void InformAboutUnloadingOfProducts()
        {

        }
        public void InformAboutloadingOfProducts()
        {

        }
        #endregion

        #region Communication with Visualizer
        public void SayToVisualizerAboutCurrentPosition()
        {

        }
        #endregion

        public static void Work(Car car)
        {

        }
    }
}
