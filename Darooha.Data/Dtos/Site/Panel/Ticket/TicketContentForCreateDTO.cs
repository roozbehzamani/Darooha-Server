using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Darooha.Data.Dtos.Site.Panel.Ticket
{
    public class TicketContentForCreateDTO
    {
        [Required]
        [StringLength(1000, MinimumLength = 0)]
        public string Text { get; set; }
        public IFormFile File { get; set; }
    }
}
