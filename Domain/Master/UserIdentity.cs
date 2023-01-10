using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Master
{
    public class UserIdentity : BaseEntity<int>
    {
        [Required]
        [MaxLength(300)]
        public string UserName { get; set; }
       
        [Required]
        [MaxLength(300)]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [MaxLength(300)]
        public string PasswordHash { get; set; }
    }
}
