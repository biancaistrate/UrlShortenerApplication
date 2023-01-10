using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Master
{
    public class TinyUrl : BaseEntity<int>
    {
        [Required]
        [MaxLength(300)]
        public string OriginalUrl { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(16)]
        public string Alias { get; set; } = string.Empty;

        [Required]
        [MaxLength(66)]
        public Uri TinyUri { get; set; }

        [ForeignKey("UserIdentity")]
        public int UserRefId { get; set; }
        public UserIdentity UserIdentity { get; set; }
    }
}
