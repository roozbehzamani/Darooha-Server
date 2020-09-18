using System;
using System.Collections.Generic;
using System.Text;

namespace Darooha.Data.Dtos.Site.Panel.Product
{
    public class AdminProductListForReturnDto
    {
        public string ID { get; set; }

        public string ProductName { get; set; }

        public int ProductPrice { get; set; }

        public int ProductCount { get; set; }

        public string ScientificName { get; set; }

        public int SumPoint { get; set; } = 0;

        public int CommentCount { get; set; } = 0;
    }
}
