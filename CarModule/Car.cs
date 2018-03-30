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
        public Int32 Capacity { get; private set; }//How much items the car can take.
        public List<Product> Products { get; set; }//products in the car.
        public Int32 Speed { get; private set; }//The car's speed.
        public String PointTo { get; set; }//Point, in which the car goes
        public String PointFrom { get; set; }//Point, from which car goes.
        public Int32 RoadPercent { get; set; }//Percentage of travel.
        //Setting for lowercase
        private JsonSerializerSettings _jsonSerializerSettings =
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
            string requestString = @"http://188.225.9.3/storages/" + car.PointFrom + "/products/prepare";
            var json = @"{
                            'capacity':" + car.Capacity + @",
                            'accessiblePoints': ''
                         }";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestString);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentLength = json.Length;
            var body = Encoding.UTF8.GetBytes(json);
            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(body, 0, json.Length);
                stream.Close();
            }
            string responseJson;
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                responseJson = streamReader.ReadToEnd();
                //Now you have your response.
                //or false depending on information in the response
            }
            responseJson = responseJson.Substring(12, responseJson.Length-13);
            car.Products = JsonConvert.DeserializeObject<List<Product>>(responseJson);
        }
        public void PutProductsToStorage()
        {

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
    }
}
