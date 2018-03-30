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
            Car car1 = new Car("car-1", 1);
            Car car2 = new Car("car-2", 1);
            Car car3 = new Car("car-3", 1);
            Car car4 = new Car("car-4", 1);
            Car car5 = new Car("car-5", 1);
            Car car6 = new Car("car-6", 1);
            Car car7 = new Car("car-7", 1);
            Car car8 = new Car("car-8", 1);

            car1.PointFrom = "A";
            car1.PointTo = "B";

            car2.PointFrom = "B";
            car2.PointTo = "C";

            car3.PointFrom = "B";
            car3.PointTo = "C";

            car4.PointFrom = "C";
            car4.PointTo = "D";

            car5.PointFrom = "D";
            car5.PointTo = "E";

            car6.PointFrom = "A";
            car6.PointTo = "B";

            car7.PointFrom = "A";
            car7.PointTo = "D";

            car8.PointFrom = "A";
            car8.PointTo = "B";
            Thread car1Task = new Thread(() => Car.Work(car1));
            Thread car2Task = new Thread(() => Car.Work(car2));
            Thread car3Task = new Thread(() => Car.Work(car3));
            Thread car4Task = new Thread(() => Car.Work(car4));
            Thread car5Task = new Thread(() => Car.Work(car5));
            Thread car6Task = new Thread(() => Car.Work(car6));
            Thread car7Task = new Thread(() => Car.Work(car7));
            Thread car8Task = new Thread(() => Car.Work(car8));
            car1Task.Start();
            car2Task.Start();
            car3Task.Start();
            car4Task.Start();
            car5Task.Start();
            car6Task.Start();
            car7Task.Start();
            car8Task.Start();
            Console.Read();
        }
        
    }
}