using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CarModule
{
    [DataContract]
    class Car
    {
        [DataMember]
        public String Id { get; private set; } //The car's Id, which can help identify the car from others.
        [DataMember]
        public Int32 Capacity { get; private set; }//How much items the car can take.
        [DataMember]
        public List<String> Items { get; set; }//Items in the car.
        public Int32 Speed { get; private set; }//The car's speed.
        [DataMember]
        public String PointTo { get; set; }//Point, in which the car goes
        [DataMember]
        public String PointFrom { get; set; }//Point, from which car goes.
        [DataMember]
        public Int32 RoadPercent { get; set; }//Percentage of travel.
        //Connection to RabbitMQ
        private ConnectionFactory factory = new ConnectionFactory
        {
            UserName = "user",
            Password = "password",
            HostName = "10.99.166.197",
            Port = 5672
        };
        Logger logger { get; set; }

        public Car(String id, Int32 capacity)
        {
            Id = id;
            Capacity = capacity;
            Items = new List<string>();
            Speed = 100;
            RoadPercent = 0;
            logger = new Logger(id);
        }

        //
        public void TransferGoodsToWarehouse(string warehouseId)
        {

        }
        public void ProductCommunication()
        {

        }
        public void OnTheRoad()
        {
            //IConnection connection = factory.CreateConnection();
            //IModel channel = connection.CreateModel();

            //channel.QueueDeclare("QUEUE_NAME", false, false, false, null);
            //var consumer = new QueueingBasicConsumer(channel);
            //Create a stream to serialize the object to.  
            MemoryStream ms = new MemoryStream();

            //Serializer the User object to the stream.  
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Car));
            byte[] message_to_visualizer;
            while (RoadPercent<=100)
            {
                
                ser.WriteObject(ms, this);
                message_to_visualizer= ms.ToArray();
                var message = Encoding.UTF8.GetString(message_to_visualizer, 0, message_to_visualizer.Length);
                //var sendingMessage = Encoding.UTF8.GetBytes(message);
                //channel.BasicPublish("", "005", null, sendingMessage);
                Console.WriteLine(message);
                logger.WriteLogInFile(message);
                RoadPercent++;
                System.Threading.Thread.Sleep(700);//имитация движения
            }
            //ms.Close();
            //channel.Close();
            //connection.Close();
            RoadPercent = 0;
            PointFrom = PointTo;
            PointTo = "";
        }
    }
}
