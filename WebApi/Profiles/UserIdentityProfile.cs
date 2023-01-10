using AutoMapper;
using Domain.Master;
using WebApi.Models;
using System.Security.Cryptography;

namespace WebApi.Profiles
{
    public class UserIdentityProfile : Profile
    {
        public UserIdentityProfile()
        {
            CreateMap<UserDTO, UserIdentity>( ).ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash));
        }

        private object ToHashString(string text)
        {
            // Uses SHA256 to create the hash
            using (var sha = SHA256.Create())
            {
                // Convert the string to a byte array first, to be processed
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text);
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
