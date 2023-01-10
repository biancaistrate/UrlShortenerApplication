using Domain.Common;
using Domain.Master;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class TinyUrlDTO
    {
        
            [Required]
            [MaxLength(300)]
            public string OriginalUrl { get; set; } = string.Empty;

            [Required]
            [MaxLength(50)]
            public string Domain { get; set; } = string.Empty;

            [Required]
            [MaxLength(16)]
            public string Alias { get; set; } = string.Empty;

    }
}
