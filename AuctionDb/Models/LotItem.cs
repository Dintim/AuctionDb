using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionDb.Models
{
    public class LotItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }
        public decimal InitialCost { get; set; }
        public string CreatedByEmployeeId { get; set; }
    }
}
