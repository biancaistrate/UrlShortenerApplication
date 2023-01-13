using Domain.Common;
using Domain.Master;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class UpdateTinyUrlDTO : BaseEntity<int>
    {
        [Required]
        [MaxLength(300)]
        public string NewOriginalUrl { get; set; } = string.Empty;

        [Required]
        [MaxLength(16)]
        public string NewAlias { get; set; } = string.Empty;
    }
}
