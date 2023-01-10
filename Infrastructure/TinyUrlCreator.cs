using Application;
using Domain.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class TinyUrlCreator : ITinyUrlCreator
    {
        private const int _maxSize = 10;
        public Uri Create(Uri domainUri, string alias = null)
        {
            var builder = new UriBuilder();

            builder.Scheme=domainUri.Scheme;
            builder.Host=domainUri.Host;
            builder.Port=domainUri.Port;
            builder.Path = string.IsNullOrEmpty(alias) ? GetRandomAlias() : alias;
            return builder.Uri;
        }

        private string GetRandomAlias()
        {
            Random rd = new Random();

            string allowedChars = "123456789abcdefghijkmnopqrstuvwxyz";
            char[] chars = new char[_maxSize];

            for (int i = 0; i < _maxSize; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}
