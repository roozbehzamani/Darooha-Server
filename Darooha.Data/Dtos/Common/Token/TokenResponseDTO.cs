
using Darooha.Data.Models;

namespace Darooha.Data.Dtos.Common.Token
{
    public class TokenResponseDTO
    {
        public string token { get; set; }
        public string refresh_token { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public Tbl_User user { get; set; }
    }
}
