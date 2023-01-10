using Domain.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Repository
{
    public interface IUserRepository
    {
        Task<UserIdentity?> GetByEmail(string email, CancellationToken cancellationToken);
        Task<UserIdentity?> GetByEmailAndSecret(string email, string password, CancellationToken cancellationToken);
        Task Create(UserIdentity userIdentity, CancellationToken cancellationToken);
    }
}
