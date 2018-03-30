//using Newtonsoft.Json;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Json;
//using System.Text;

//namespace CarModule
//{
//    [DataContract]
//    class Car
//    {
//        [DataMember]
//        public String Id { get; private set; } //The car's Id, which can help identify the car from others.
//        [DataMember]
//        public Int32 Capacity { get; private set; }//How much items the car can take.
//        [DataMember]
//        public List<Item> Items { get; set; }//Items in the car.
//        public Int32 Speed { get; private set; }//The car's speed.
//        [DataMember]
//        public String PointTo { get; set; }//Point, in which the car goes
//        [DataMember]
//        public String PointFrom { get; set; }//Point, from which car goes.
//        [DataMember]
//        public Int32 RoadPercent { get; set; }//Percentage of travel.
//        private JsonSerializerSettings _jsonSerializerSettings = 
//            new JsonSerializerSettings
//            {
//                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
//            };
//        //Connection to RabbitMQ
//        private ConnectionFactory factory = new ConnectionFactory
//        {
//            UserName = "user",
//            Password = "password",
//            HostName = "10.99.166.197",
//            Port = 5672
//        };
//        Logger logger { get; set; }

//        public Car(String id, Int32 capacity)
//        {
//            Id = id;
//            Capacity = capacity;
//            Items = new List<Item>();
//            Speed = 100;
//            RoadPercent = 0;
//            logger = new Logger(id); 
//        }

//        //
//        public void AskWarehouseForItemsAsync(Car car, String warehouseId)
//        {

//        }
    

//        public void SayPointAboutUnloadingItems()
//        {
//            var items = JsonConvert.SerializeObject(Items, _jsonSerializerSettings);
//            var body = Encoding.UTF8.GetBytes(items);
//            var request = (HttpWebRequest)WebRequest.Create("http://url");

//            request.Method = "GET";
//            request.ContentType = "application/json";
//            request.ContentLength = body.Length;

//            using (Stream stream = request.GetRequestStream())
//            {
//                stream.Write(body, 0, body.Length);
//                stream.Close();
//            }

//            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
//            {
//                response.Close();
//            }
//        }
//        public void SayProductAboutUnloadingItems()
//        {
//            var items = new MessageToProducts(Items, PointTo, Id);//Create class for serialization
//            String unloadItemsMessage = JsonConvert.SerializeObject(items, _jsonSerializerSettings);

//            var body = Encoding.UTF8.GetBytes(unloadItemsMessage);
//            var request = (HttpWebRequest)WebRequest.Create("http://url/changeOwner");

//            request.Method = "POST";
//            request.ContentType = "application/json";
//            request.ContentLength = body.Length;

//            using (Stream stream = request.GetRequestStream())
//            {
//                stream.Write(body, 0, body.Length);
//                stream.Close();
//            }

//            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
//            {
//                response.Close();
//            }
//        }
//        public void UnloadItems()
//        {

//        }
//        public void OnTheRoad()
//        {
//            //IConnection connection = factory.CreateConnection();
//            //IModel channel = connection.CreateModel();
//            SayProductAboutUnloadingItems();
//            //channel.QueueDeclare("QUEUE_NAME", false, false, false, null);
//            //var consumer = new QueueingBasicConsumer(channel);
//            byte[] message_to_visualizer;
//            while (RoadPercent<=100)
//            {
//                String message = JsonConvert.SerializeObject(this, _jsonSerializerSettings);
//                //var sendingMessage = Encoding.UTF8.GetBytes(message);
//                //channel.BasicPublish("", "005", null, sendingMessage);
//                Console.WriteLine(message);
//                logger.WriteLogInFile(message);
//                RoadPercent++;
//                System.Threading.Thread.Sleep(700);//имитация движения
//            }
//            //channel.Close();
//            //connection.Close();
//            RoadPercent = 0;
//            PointTo = "";
//        }
//    }
//}
