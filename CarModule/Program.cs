using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using CarModule;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Net;

namespace CarModule
{
    class Program
    {
        static void Main(string[] args)
        {
            var testCar1 = new Car("car-1", 3);
            testCar1.PointFrom = "A";

            Car.GetProductsFromStorage(testCar1);
            Car.PutProductsToStorage(testCar1);
            foreach (var item in testCar1.Products)
            {
                Console.WriteLine(item);
            }
            Console.Read();
        }
        
    }
}