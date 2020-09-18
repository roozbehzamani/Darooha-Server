using Darooha.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Darooha.Data.Dtos.Site.App.Product
{
    public class SpecialProductForReturnDto
    {
        public string ID { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public string FirstImageUrl { get; set; }
        public string SecendImageUrl { get; set; }
        public string ScientificName { get; set; }
        public int SumPoint { get; set; }
        public int CommentCount { get; set; }
        public string ManufacturingCountry { get; set; }
        public string ManufacturerCompany { get; set; }
    }
}
