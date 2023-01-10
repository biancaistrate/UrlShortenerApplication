using Domain.Common;
using Domain.Master;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class UserDTO: LoginInfo
    {
        //[Required]
        //[MaxLength(50)]
        //public string Email { get; set; }

        [Required]
        [MaxLength(300)]
        public string UserName { get; set; }

        //[Required]
        //[MaxLength(150)]
        //public string Password { get; set; }
    }
}
