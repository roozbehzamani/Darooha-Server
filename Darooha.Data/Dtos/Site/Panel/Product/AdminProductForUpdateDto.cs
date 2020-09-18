using System;
using System.Collections.Generic;
using System.Text;

namespace Darooha.Data.Dtos.Site.Panel.Product
{
    public class AdminProductForUpdateDto
    {
        public string ProductName { get; set; }

        public int ProductPrice { get; set; }

        public int ProductCount { get; set; }

        public bool IsEnable { get; set; }

        public string Size { get; set; }

        public string Code { get; set; }

        public short GenderOfConsumer { get; set; } //true: male false: female

        public string EnclosureType { get; set; } //نوع محفظه

        public string ManufacturingCountry { get; set; }

        public string ManufacturerCompany { get; set; }

        public string WebAddress { get; set; }

        public string Features { get; set; }

        public string MethodUse { get; set; }

        public string Indications { get; set; }

        public string Warnings { get; set; }

        public string Maintenance { get; set; }

        public bool IsSpecial { get; set; }

        public string ScientificName { get; set; }
    }
}
