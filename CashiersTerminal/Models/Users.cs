using System.ComponentModel.DataAnnotations;

namespace CashiersTerminal.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public int AccessLevel { get; set; }
    }
}
