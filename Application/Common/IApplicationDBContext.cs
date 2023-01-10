using Domain.Master;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public interface IApplicationDBContext
    {
        public DbSet<TinyUrl> TinyUrls { get; set; }
        public DbSet<UserIdentity> UserIdentities { get; set; }

        Task<int> SaveAsync(CancellationToken cancellationToken);
    }
}
