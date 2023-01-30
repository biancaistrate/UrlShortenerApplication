using Domain.Common;
using Domain.Master;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class TinyUrlDTO
    {

        public string OriginalUrl { get; set; } = string.Empty;

        public string Alias { get; set; } = string.Empty;

    }
}
