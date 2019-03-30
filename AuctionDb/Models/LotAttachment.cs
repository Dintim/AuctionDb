using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionDb.Models
{
    public class LotAttachment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public byte[] Body { get; set; }
        public string LotItemId { get; set; }
    }
}
