using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionDb.ViewModels
{
    public class CreateOrganizationViewModel
    {
        public string OrganizationName { get; set; }                
        public string CeoFirstName { get; set; }
        public string CeoLastName { get; set; }        
        public string Email { get; set; }
        public DateTime DoB { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}
