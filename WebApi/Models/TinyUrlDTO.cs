using Domain.Common;
using Domain.Master;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class TinyUrlDTO
    {
        
            [Required]
            [MaxLength(300)]
            public string OriginalUrl { get; set; } = string.Empty;

            [MaxLength(16)]
            public string Alias { get; set; } = string.Empty;

    }
}
