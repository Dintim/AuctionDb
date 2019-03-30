using AuctionDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionDb.ViewModels
{
    public class CreateAuctionViewModel
    {        
        public string LotName { get; set; }
        public string LotDescription { get; set; }        
        public decimal InitialCost { get; set; }
        public string CreatedByEmployeeId { get; set; }
        public List<CreateLotAttachViewModel> LotAttachmentVMs { get; set; }
    }
}
