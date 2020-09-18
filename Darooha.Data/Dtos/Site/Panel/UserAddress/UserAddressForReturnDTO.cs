using System;
using System.Collections.Generic;
using System.Text;

namespace Darooha.Data.Dtos.Site.Panel.UserAddress
{
    public class UserAddressForReturnDTO
    {
        public string ID { get; set; }
        public string AddressName { get; set; }
        public string Address { get; set; }
        public string AddressLatLng { get; set; }
    }
}
