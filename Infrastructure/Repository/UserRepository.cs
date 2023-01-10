using Application.Common;
using Application.Repository;
using Domain.Master;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IApplicationDBContext _context;

        public UserRepository(IApplicationDBContext context)
        {
            _context = context;
        }
        public async Task Create(UserIdentity userIdentity, CancellationToken cancellationToken)
        {
           await _context.UserIdentities.AddAsync(userIdentity, cancellationToken);
           await _context.SaveAsync(cancellationToken);
        }

        public async Task<UserIdentity?> GetByEmail(string email, CancellationToken cancellationToken)
        {
            return await _context.UserIdentities.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<UserIdentity?> GetByEmailAndSecret(string email, string password, CancellationToken cancellationToken)
        {
            return await _context.UserIdentities.FirstOrDefaultAsync(x => x.Email == email && x.PasswordHash == password);
        }
    }
}
