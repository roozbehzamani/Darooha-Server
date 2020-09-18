using System.ComponentModel.DataAnnotations;

namespace Darooha.Data.Dtos.Site.Panel.Notification
{
    public class NotificationForUpdateDTO
    {
        [Required]
        public bool EnterEmail { get; set; }
        [Required]
        public bool EnterSms { get; set; }
        [Required]
        public bool EnterTelegram { get; set; }

        [Required]
        public bool ExitEmail { get; set; }
        [Required]
        public bool ExitSms { get; set; }
        [Required]
        public bool ExitTelegram { get; set; }


        [Required]
        public bool TicketEmail { get; set; }
        [Required]
        public bool TicketSms { get; set; }
        [Required]
        public bool TicketTelegram { get; set; }

        [Required]
        public bool LoginEmail { get; set; }
        [Required]
        public bool LoginSms { get; set; }
        [Required]
        public bool LoginTelegram { get; set; }
    }
}
