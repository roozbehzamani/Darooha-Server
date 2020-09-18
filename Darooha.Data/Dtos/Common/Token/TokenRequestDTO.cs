using System.ComponentModel.DataAnnotations;

namespace Darooha.Data.Dtos.Common.Token
{
    public class TokenRequestDTO
    {
        [Required]
        public string GrantType { get; set; } //password || refresh_token
        public string ClientId { get; set; }
        [Required]
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
        public string Password { get; set; }
        public bool IsRemember { get; set; } = false;
    }
}
