using System;
using System.Collections.Generic;

namespace CarModule
{
    class Product
    {
        public String Id { get; set; }
        public String Source { get; set; }
        public String Destination { get; set; }
        public List<String> Route { get; set; }

        public override string ToString()
        {
            string allRoutes = "[";
            foreach (var point in Route)
            {
                if (point != Route[0] && point != Route[Route.Count - 1])
                    allRoutes += ", " + point;
                else
                    allRoutes += point;
            }
            allRoutes += "]";
            return String.Format("Id={0}, Source={1}, Destination={2}, Route={3}", Id, Source, Destination, allRoutes);
        }
    }
}
