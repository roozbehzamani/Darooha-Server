using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Darooha.Data.Dtos.Site.Panel.Photo
{
    public class PhotoForUserProfileDTO
    {
        public string ImageURL { get; set; }

        public string ImageID { get; set; }

        public IFormFile File { get; set; }
    }
}
