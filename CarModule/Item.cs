using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarModule
{
    class Item
    {
        public String Id { get; set; }
        public String From { get; set; }
        public String To { get; set; }
        public List<String> Route;
    }
}
