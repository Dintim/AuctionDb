using AuctionDb.Models;
using AuctionDb.Repositories;
using AuctionDb.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionDb
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateLotAttachViewModel attach1 = new CreateLotAttachViewModel()
            {
                Name = "Аудиторский отчет за 2016 год",
                Path = @"C:\Users\dm\Desktop\Lafore.pdf"
            };
            CreateLotAttachViewModel attach2 = new CreateLotAttachViewModel()
            {
                Name = "Аудиторский отчет за 2017 год",
                Path = @"C:\Users\dm\Desktop\111.txt"
            };
            CreateLotAttachViewModel attach3 = new CreateLotAttachViewModel()
            {
                Name = "Аудиторский отчет за 2018 год",
                Path = @"C:\Users\dm\Desktop\555.xls"
            };

            List<CreateLotAttachViewModel> attachments = new List<CreateLotAttachViewModel>();
            attachments.Add(attach1);
            attachments.Add(attach2);
            attachments.Add(attach3);

            CreateAuctionViewModel auctionViewModel = new CreateAuctionViewModel()
            {
                LotName = "Закуп строительных материалов",
                LotDescription = "Закуп стройматериалов для строительства коровника",
                InitialCost = (decimal)45145000.45,
                CreatedByEmployeeId = "0E9F38D3-1DD6-4D40-B9A5-972CBA84037B",
                LotAttachmentVMs = attachments
            };

            AccountService service = new AccountService();
            service.CreateAuction(auctionViewModel);

            
        }
    }
}
