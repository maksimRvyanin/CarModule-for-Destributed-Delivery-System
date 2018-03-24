using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using CarModule;
using System.Threading;

namespace CarModule
{
    class Program
    {
        public static string WriteFromObject()
        {
            //Create User object.  
            Car car = new Car("car2", 42);
            car.Items.Add("itemid=12312341324, from=Russia, to=China");
            car.Items.Add("itemid=12312341324, from=Russia, to=China");
            car.Items.Add("itemid=12312341324, from=Russia, to=China");
            car.Items.Add("itemid=12312341324, from=Russia, to=China");
            //Create a stream to serialize the object to.  
            MemoryStream ms = new MemoryStream();

            // Serializer the User object to the stream.  
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Car));
            ser.WriteObject(ms, car);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }
        static void Main(string[] args)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                try
                {
                    Car car = new Car("car1", 90);
                    car.PointFrom = "Warehouse1";
                    car.PointTo = "Warehouse2";
                    car.Items.Add("itemid=12312341324, from=Russia, to=China");
                    car.Items.Add("itemid=12312341324, from=Russia, to=China");
                    car.Items.Add("itemid=12312341324, from=Russia, to=China");
                    car.Items.Add("itemid=12312341324, from=Russia, to=China");

                    Car car2 = new Car("car2", 90);
                    car.PointFrom = "Warehouse3";
                    car.PointTo = "Warehouse4";
                    car.Items.Add("itemid=12312341324, from=Russia, to=China");
                    car.Items.Add("itemid=12312341324, from=Russia, to=China");
                    car.Items.Add("itemid=12312341324, from=Russia, to=China");
                    car.Items.Add("itemid=12312341324, from=Russia, to=China");

                    Thread Th1 = new Thread(new ThreadStart(car.OnTheRoad));
                    Thread Th2 = new Thread(new ThreadStart(car2.OnTheRoad));
                    Th1.Start();
                    Th2.Start();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    Console.Read();
                }
            }
        }
    }
}
