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
using System.Threading;

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
        //logger
        Logger logger;
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
            logger = new Logger(id);
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
                    stream.Write(body, 0, body.Length);
                    stream.Close();
                }
                string responseJson;
                try
                {
                    Thread.Sleep(1000);
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
                    Console.WriteLine(e);
                    car.logger.Log(e.ToString());
                }
            }

            car.PointTo = car.Products[0].Destination; //Точка назначения сооттветсвует конечной точке товаров
            //Форматируем для логера
            string productsStr = String.Join(", \n", car.Products);
            string log = String.Format("{0} GET from STORAGE \"{1}\" PRODUCTS:\n{2}", car.transportId, car.PointFrom, productsStr);
            Console.WriteLine(log);
            car.logger.Log(log);

        }
        public static void PutProductsToStorage(Car car)
        {
            var requestString = @"http://188.225.9.3/storages/" + car.PointTo + "/products";
            //выкладываем товар, значит уже им не владеем
            foreach (var product in car.Products)
            {
                product.Source = "";
            }
            var productsJson = JsonConvert.SerializeObject(car.Products, _jsonSerializerSettings);
            string json = "{\"products\": " + productsJson + ", \"transportId\": \"" + car.transportId + "\"}";
            string httpResult = "";
            string productsStr = "";
            while (httpResult!="OK")
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestString);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "*/*";
                var body = Encoding.UTF8.GetBytes(json);
                using (Stream stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                }
                try
                {
                    using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        httpResult = httpResponse.StatusCode.ToString();
                    }
                    if (httpResult == "OK")
                    {
                        Console.WriteLine("All goods were put in storage {0}", car.PointTo);
                    }
                    productsStr = String.Join(", \n", car.Products);
                    car.Products.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            string log = String.Format("{0} POST to STORAGE \"{1}\" PRODUCTS:\n{2} \n", car.transportId, car.PointTo, productsStr);
            Console.WriteLine(log);
            car.logger.Log(log);
            car.PointFrom = car.PointTo;
            log = String.Format("{0} are waiting in {1} for products", car.transportId, car.PointFrom);
            Thread.Sleep(3000);
        }
        #endregion

        #region Communication with ProductService
        public static void InformAboutUnloadingOfProducts(Car car)
        {
            var requestString = @"http://188.225.9.3/products/owner";
            foreach (var product in car.Products)
            {
                product.Source = "";
            }
            var productsJson = JsonConvert.SerializeObject(car.Products, _jsonSerializerSettings);
            string json = "{\"products\": " + productsJson + "}";
            string StatusCode = "bad";
            string productsStr= "";
            while (StatusCode!="OK")
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestString);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";
                httpWebRequest.Accept = "*/*";
                var body = Encoding.UTF8.GetBytes(json);
                using (Stream stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Flush();
                    stream.Close();
                }
                try
                {
                    using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        StatusCode = httpResponse.StatusCode.ToString();
                    }
                    if (StatusCode == "OK")
                    {
                        productsStr = String.Join(", \n", car.Products);
                        string log = String.Format("{0} PUT to PRODUCTS_SERVICE that PRODUCTS:\n{1} \n are in STORAGE \"{2}\"", car.transportId, productsStr, car.PointTo);
                        Console.WriteLine(log);
                        car.logger.Log(log);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    car.logger.Log(e.ToString());
                }
            }
            
        }
        public static void InformAboutloadingOfProducts(Car car)
        {
            var requestString = @"http://188.225.9.3/products/owner/" + car.transportId;
            foreach (var product in car.Products)
            {
                product.Source = car.transportId;
            }
            var productsJson = JsonConvert.SerializeObject(car.Products, _jsonSerializerSettings);
            string json = "{\"products\": " + productsJson + "}";
            string result = "bad";
            while (result!="OK")
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestString);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";
                httpWebRequest.Accept = "*/*";
                var body = Encoding.UTF8.GetBytes(json);
                using (Stream stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(body, 0, body.Length);
                    stream.Close();
                }
                try
                {
                    using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        result = httpResponse.StatusCode.ToString();
                    }
                    if (result == "OK")
                    {
                        string productsStr = String.Join(", \n", car.Products);
                        string log = String.Format("{0} PUT to PRODUCTS_SERVICE that PRODUCTS:\n{1} \n are taken from STORAGE \"{2}\"", car.transportId, productsStr, car.PointFrom);
                        Console.WriteLine(log);
                        car.logger.Log(log);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        #endregion

        #region Communication with Visualizer
        public static void SayToVisualizerAboutCurrentPosition(Car car)
        {

        }
        #endregion

        public static void Work(Car car)
        {
            while (true)
            {
                GetProductsFromStorage(car);
                InformAboutloadingOfProducts(car);
                InformAboutUnloadingOfProducts(car);
                PutProductsToStorage(car);
            }
        }
    }
}
