using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarModule
{
    class MessageToProducts
    {
        public List<String> Products { get; set; }
        public String StorageId { get; set; }
        public String TransportId { get; set; }
        private String ComponentType => "Car";
        public MessageToProducts(List<String> listItems, String storageId, String transportId)
        {
            Products = listItems;
            StorageId = storageId;
            TransportId = transportId;
        }
    }
}
