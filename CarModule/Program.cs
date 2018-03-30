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
            var testCar = new Car("car-1", 100);
            testCar.PointFrom = "A";
            Car.GetProductsFromStorage(testCar);
            foreach (var item in testCar.Products)
            {
                Console.WriteLine(item);
            }
            Console.Read();
        }
        
    }
}


//class MyClass
//{
//    public string transportId;
//    public string storageId;
//    public JToken items;
//}
//static void Main(string[] args)
//{


//    var jobject = JObject.Parse(json);
//    var myclass = new MyClass()
//    {
//        transportId = jobject["transportId"].ToString(),
//        storageId = jobject["storageId"].ToString(),
//        items = jobject["items"],
//    };
//    Console.WriteLine(myclass.transportId);
//    foreach (var item in myclass.items)
//    {
//        Console.WriteLine(item);
//    }
//    Console.Read();

//}
//var json =
//@"
//{
//    'transportId':'car_maxim_pidor',
//    'storageId':'A',
//    'items':[
//                {
//                    'id':'product1',
//                    'from':'A',
//                    'to':'Z',
//                    'route':['A','B','X','Z']
//                },
//                {
//                    'id':'product1',
//                    'from':'A',
//                    'to':'Z',
//                    'route':['A','B','X','Z']
//                }
//            ]
//}";