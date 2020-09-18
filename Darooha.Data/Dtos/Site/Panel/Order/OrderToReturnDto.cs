using System;
using System.Collections.Generic;
using System.Text;

namespace Darooha.Data.Dtos.Site.Panel.Order
{
    public class OrderToReturnDto
    {
        public string ID { get; set; }
        public short PaymentMethod { get; set; } //1:wallet 2:online 3:cash
        public short Status { get; set; }
        public int TotalPrice { get; set; }
        public string userAddress { get; set; }
        public string DateModified { get; set; }
        public string AddressID { get; set; }
    }
}
