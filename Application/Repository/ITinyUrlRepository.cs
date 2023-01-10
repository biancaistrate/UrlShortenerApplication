using Domain.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository
{
    public interface ITinyUrlRepository
    {
        Task<IEnumerable<TinyUrl>> GetAllAsync(string userIdentifier, CancellationToken cancellationToken);
        Task<IEnumerable<TinyUrl>> GetTopAsync(int top, string userIdentifier, CancellationToken cancellationToken);
        Task<TinyUrl?> GetByIdAsync(int tinyUrlId, string userIdentifier, CancellationToken cancellationToken);
        Task<TinyUrl?> FindByShortForm(Uri tinyUrl, CancellationToken cancellationToken);

        TinyUrl Insert(TinyUrl tinyUrl);
        void Update(TinyUrl tinyUrl);
        void Delete(int tinyUrlId);
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
