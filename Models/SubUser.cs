using System;
using System.ComponentModel.DataAnnotations;

namespace CriticalConditionBackend.Models
{
    public class SubUser
    {
        [Key]
        public string Code { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string SuperUserEmail { get; set; }
    }
}
