using System;
using System.Collections.Generic;
using System.Text;

namespace Darooha.Data.Dtos.Site.Panel.Order
{
    public class OrderItemToReturnDto
    {
        public string ID { get; set; }
        public string ProductName { get; set; }
        public string ItemCount { get; set; }
        public int ProductTotalPrice { get; set; }
        public string ProductID { get; set; }
    }
}
