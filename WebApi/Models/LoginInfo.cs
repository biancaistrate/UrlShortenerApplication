using Domain.Common;
using Domain.Master;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace WebApi.Models
{
    public class LoginInfo
    {
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MaxLength(150)]
        public string Password { get; set; }

        [JsonIgnore]
        //[IgnoreDataMember]
        public string PasswordHash
        { 
            get
            {
                if (Password == null)
                    return null;

                // Uses SHA256 to create the hash
                using (var sha = SHA256.Create())
                {
                    // Convert the string to a byte array first, to be processed
                    byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(Password);
                    byte[] hashBytes = sha.ComputeHash(textBytes);

                    // Convert back to a string, removing the '-' that BitConverter adds
                    string hash = BitConverter
                        .ToString(hashBytes)
                        .Replace("-", String.Empty);

                    return hash;
                }
            }
        }

    }
}
