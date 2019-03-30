using AuctionDb.Models;
using AuctionDb.Repositories;
using AuctionDb.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AuctionDb
{
    public class AccountService
    {
        public void CreateOrganization(CreateOrganizationViewModel orgViewModel)
        {
            Organization organization = new Organization()
            {
                Id = Guid.NewGuid().ToString(),
                Name = orgViewModel.OrganizationName
            };
            Employee employee = new Employee()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = orgViewModel.CeoFirstName,
                LastName = orgViewModel.CeoLastName,
                Email = orgViewModel.Email,
                Password = orgViewModel.Password,
                DoB = orgViewModel.DoB,
                OrganizationId = organization.Id
            };

            OrganizationRepository organizationRepository = new OrganizationRepository();
            EmployeeRepository employeeRepository = new EmployeeRepository();

            using (TransactionScope transactionScope=new TransactionScope())
            {
                try
                {
                    organizationRepository.Add(organization);
                    employeeRepository.Add(employee);
                    transactionScope.Complete();
                }
                catch 
                {
                    throw new Exception("Transaction failed");
                }
            }            
        }        


        public void CreateAuction(CreateAuctionViewModel auctionViewModel)
        {
            LotItem lot = new LotItem()
            {
                Id = Guid.NewGuid().ToString(),
                Name = auctionViewModel.LotName,
                Description = auctionViewModel.LotDescription,
                PublishedDate = DateTime.Now,
                InitialCost = auctionViewModel.InitialCost,
                CreatedByEmployeeId = auctionViewModel.CreatedByEmployeeId
            };

            LotItemRepository lotRepository = new LotItemRepository();
            LotAttachmentRepository lotAttachmentRepository = new LotAttachmentRepository();

            using (TransactionScope transactionScope=new TransactionScope())
            {
                try
                {
                    lotRepository.Add(lot);
                    foreach (CreateLotAttachViewModel item in auctionViewModel.LotAttachmentVMs)
                    {
                        LotAttachment lotAttachment = new LotAttachment()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = item.Name,
                            Extension = item.Path.Substring(item.Path.LastIndexOf('.') + 1),
                            Body = File.ReadAllBytes(item.Path),
                            LotItemId = lot.Id
                        };                        
                        lotAttachmentRepository.Add(lotAttachment);
                    }
                    transactionScope.Complete();
                }
                catch 
                {
                    throw new Exception("Transaction failed");
                }
            }
        }

        
    }
}
