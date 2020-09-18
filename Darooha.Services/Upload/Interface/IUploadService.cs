using Darooha.Data.Dtos.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Darooha.Services.Upload.Interface
{
    public interface IUploadService
    {
        FileUploadedDTO UploadToCloudinary(IFormFile file);
        FileUploadedDTO RemoveFileFromCloudinary(string publicID);
        FileUploadedDTO RemoveFileFromLocal(string FileName, string WebRootPath, string FilePath);
        Task<FileUploadedDTO> UploadProfilePicToLocal(IFormFile file, string userID, string WebRootPath, string BaseUrl, string UrlUrl = "Files\\Pic\\Profile");
        Task<FileUploadedDTO> UploadProfilePic(IFormFile file, string userID, string WebRootPath, string BaseUrl, string ImageID);
    }
}
